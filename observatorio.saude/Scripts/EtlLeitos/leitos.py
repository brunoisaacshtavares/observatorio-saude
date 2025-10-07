# ☢️ AVISO: A linha abaixo desabilita a verificação de certificado SSL.
# É uma solução de contorno insegura. A forma correta é garantir que o
# ambiente de execução (servidor/container) tenha os certificados raiz atualizados.
import ssl
ssl._create_default_https_context = ssl._create_unverified_context

import pandas as pd
import psycopg2
import numpy as np
import argparse # Módulo para receber argumentos de linha de comando
from psycopg2 import OperationalError
from psycopg2.extras import execute_values

def insert_data(cursor, table, columns, df, conflict_column, update_on_conflict=False):
    """Função para inserir dados no banco de dados com tratamento de conflitos."""
    if df.empty:
        print(f"[!] DataFrame vazio para {table}, nada inserido.")
        return

    if isinstance(conflict_column, (list, tuple)):
        conflict_target = ", ".join(conflict_column)
    else:
        conflict_target = conflict_column

    update_cols = [col for col in columns if col not in (conflict_column if isinstance(conflict_column, (list, tuple)) else [conflict_column])]
    set_clause = ", ".join([f"{col}=EXCLUDED.{col}" for col in update_cols])
    
    if update_on_conflict and update_cols:
        conflict_clause = f"ON CONFLICT ({conflict_target}) DO UPDATE SET {set_clause}"
    else:
        conflict_clause = f"ON CONFLICT ({conflict_target}) DO NOTHING"

    sql = f"""
        INSERT INTO {table} ({', '.join(columns)})
        VALUES %s
        {conflict_clause};
    """
    values = [tuple(row) for row in df.to_numpy()]
    execute_values(cursor, sql, values, page_size=1000)
    action = "UPSERT (insert/update)" if update_on_conflict else "INSERT (ignorar duplicatas)"
    print(f"Ação executada na tabela '{table}': {action}.")

def main(args):
    """Função principal que executa o processo de ETL."""
    conn_params = {
        "host": args.host,
        "port": args.port,
        "user": args.user,
        "password": args.password,
        "dbname": args.dbname
    }
    
    df_agg = pd.DataFrame()
    # Os dados de 2025 provavelmente não existem, mas mantive a lógica.
    anos = [2007,2008,2009,2010,2011,2012,2013,2014,2015,2016,2017,2018,2019,2020,2021,2022,2023,2024,2025]

    for ano in anos:
        print(f"Processando ano: {ano}")
        try:
            if ano == 2025:
                # O arquivo de 2025 é um ZIP
                url_zip = f"https://s3.sa-east-1.amazonaws.com/ckan.saude.gov.br/Leitos_SUS/Leitos_csv_{ano}.zip"
                df = pd.read_csv(url_zip, compression='zip', sep=';', encoding="latin-1", dtype=str, on_bad_lines='skip')
            else:
                # Arquivos mais antigos são CSV simples
                url_zip = f"https://s3.sa-east-1.amazonaws.com/ckan.saude.gov.br/Leitos_SUS/Leitos_{ano}.csv"
                df = pd.read_csv(url_zip, sep=',', encoding="latin-1", dtype=str, on_bad_lines='skip')
                # Renomeia colunas e adiciona CO_IBGE para padronizar
                df.columns = ['COMP', 'REGIAO', 'UF', 'MUNICIPIO', 'MOTIVO_DESABILITACAO',
                       'CNES', 'NOME_ESTABELECIMENTO', 'RAZAO_SOCIAL', 'TP_GESTAO',
                       'CO_TIPO_UNIDADE', 'DS_TIPO_UNIDADE', 'NATUREZA_JURIDICA',
                       'DESC_NATUREZA_JURIDICA', 'NO_LOGRADOURO', 'NU_ENDERECO',
                       'NO_COMPLEMENTO', 'NO_BAIRRO', 'CO_CEP', 'NU_TELEFONE', 'NO_EMAIL',
                       'LEITOS_EXISTENTES', 'LEITOS_SUS', 'UTI_TOTAL_EXIST', 'UTI_TOTAL_SUS',
                       'UTI_ADULTO_EXIST', 'UTI_ADULTO_SUS', 'UTI_PEDIATRICO_EXIST',
                       'UTI_PEDIATRICO_SUS', 'UTI_NEONATAL_EXIST', 'UTI_NEONATAL_SUS',
                       'UTI_QUEIMADO_EXIST', 'UTI_QUEIMADO_SUS', 'UTI_CORONARIANA_EXIST',
                       'UTI_CORONARIANA_SUS']
                df['CO_IBGE'] = 'nan'

            df_agg = pd.concat([df_agg, df], ignore_index=True)
            print(f" > {len(df)} registros resgatados de {ano}.")
        except Exception as e:
            print(f" [!] Erro ao baixar ou processar dados de {ano}: {e}")
            continue # Pula para o próximo ano se houver erro
            
    if df_agg.empty:
        print("Nenhum dado foi baixado. Encerrando o script.")
        return

    # Padronização e tratamento dos dados
    # ... (o resto do seu código de tratamento de dados permanece o mesmo) ...
    # ...

    # Criar DataFrame final para inserção
    df_agg.columns = ['ANOMES', 'REGIAO', 'UF', 'CO_IBGE','MUNICIPIO', 'MOTIVO_DESABILITACAO',
           'COD_CNES', 'NM_ESTABELECIMENTO', 'RAZAO_SOCIAL', 'TP_GESTAO',
           'CO_TIPO_UNIDADE', 'DSCR_TIPO_UNIDADE', 'NATUREZA_JURIDICA',
           'DESC_NATUREZA_JURIDICA', 'NO_LOGRADOURO', 'NU_ENDERECO',
           'NO_COMPLEMENTO', 'NO_BAIRRO', 'CO_CEP', 'NU_TELEFONE', 'NO_EMAIL',
           'QTD_LEITOS_EXISTENTES', 'QTD_LEITOS_SUS', 'QTD_UTI_TOTAL_EXIST', 'QTD_UTI_TOTAL_SUS',
           'QTD_UTI_ADULTO_EXIST', 'QTD_UTI_ADULTO_SUS', 'QTD_UTI_PEDIATRICO_EXIST',
           'QTD_UTI_PEDIATRICO_SUS', 'QTD_UTI_NEONATAL_EXIST', 'QTD_UTI_NEONATAL_SUS',
           'QTD_UTI_QUEIMADO_EXIST', 'QTD_UTI_QUEIMADO_SUS', 'QTD_UTI_CORONARIANA_EXIST',
           'QTD_UTI_CORONARIANA_SUS']

    df_fato_leitos = df_agg[['ANOMES', 'COD_CNES', 'NM_ESTABELECIMENTO', 'DSCR_TIPO_UNIDADE',
                            'QTD_LEITOS_EXISTENTES', 'QTD_LEITOS_SUS', 'QTD_UTI_TOTAL_EXIST', 'QTD_UTI_TOTAL_SUS',
                            'QTD_UTI_ADULTO_EXIST', 'QTD_UTI_ADULTO_SUS', 'QTD_UTI_PEDIATRICO_EXIST',
                            'QTD_UTI_PEDIATRICO_SUS', 'QTD_UTI_NEONATAL_EXIST', 'QTD_UTI_NEONATAL_SUS',
                            'QTD_UTI_QUEIMADO_EXIST', 'QTD_UTI_QUEIMADO_SUS', 'QTD_UTI_CORONARIANA_EXIST',
                            'QTD_UTI_CORONARIANA_SUS']]

    # Limpeza de dados
    for col in df_fato_leitos.columns:
        if 'QTD_' in col:
            df_fato_leitos[col] = pd.to_numeric(df_fato_leitos[col], errors='coerce').fillna(0).astype(int)

    df_fato_leitos.columns = df_fato_leitos.columns.str.lower()

    tabela = {
        "fato_leito": (
            list(df_fato_leitos.columns),
            df_fato_leitos,
            ("anomes", "cod_cnes"),
            True
        )
    }

    try:
        with psycopg2.connect(**conn_params) as conn:
            with conn.cursor() as cursor:
                for nome_tabela, (columns, df_dados, conflict, update) in tabela.items():
                    print(f"Processando inserção na tabela: {nome_tabela}")
                    insert_data(cursor, nome_tabela, columns, df_dados, conflict, update)
            conn.commit()
            print("Dados inseridos com sucesso no banco de dados.")
    except OperationalError as e:
        print(f"Erro de conexão com o banco de dados: {e}")
    except psycopg2.Error as e:
        print(f"Ocorreu um erro ao inserir dados no PostgreSQL: {e}")

if __name__ == "__main__":
    # Configura o parser para ler os argumentos da linha de comando
    parser = argparse.ArgumentParser(description="ETL para dados de leitos do SUS.")
    parser.add_argument("--host", required=True, help="Host do banco de dados")
    parser.add_argument("--port", required=True, help="Porta do banco de dados")
    parser.add_argument("--dbname", required=True, help="Nome do banco de dados")
    parser.add_argument("--user", required=True, help="Usuário do banco de dados")
    parser.add_argument("--password", required=True, help="Senha do banco de dados")
    
    # Converte os argumentos da linha de comando em um objeto
    parsed_args = parser.parse_args()
    
    # Chama a função principal com os argumentos
    main(parsed_args)