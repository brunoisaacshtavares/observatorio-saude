#!/usr/bin/env python
# coding: utf-8

# In[1]:


import requests
import pandas as pd
import polars as pl
import zipfile
import io
import numpy as np
from datetime import date, datetime, timedelta
import psycopg2
from psycopg2 import OperationalError
from psycopg2.extras import execute_values
import argparse

# In[2]:

def main(args):
    conn_params = {
        "host": args.host,
        "port": args.port,
        "user": args.user,
        "password": args.password,
        "dbname": args.dbname
    }

# In[22]:

    url_zip = "https://s3.sa-east-1.amazonaws.com/ckan.saude.gov.br/CNES/cnes_estabelecimentos_csv.zip"
    response = requests.get(url_zip)
    response.raise_for_status()

    zip_in_memory = io.BytesIO(response.content)

    nome_csv = "cnes_estabelecimentos.csv"  

    with zipfile.ZipFile(zip_in_memory) as zf:
            print(f"Extraindo '{nome_csv}' do ZIP...")
            with zf.open(nome_csv) as csvfile:
                    df = pd.read_csv(csvfile, sep=';', encoding="latin-1", dtype=str)


    # In[23]:


    df["CO_UNIDADE"] = df["CO_UNIDADE"].astype(str)
    df.CO_CNES = df.CO_CNES.astype("Int64")
    df['CO_UF'] = df['CO_UF'].astype("Int64")
    df['CO_IBGE'] = df['CO_IBGE'].astype("Int64")
    df['NU_CNPJ_MANTENEDORA'] = df['NU_CNPJ_MANTENEDORA'].astype("Int64")
    df['CO_NATUREZA_ORGANIZACAO'] = df['CO_NATUREZA_ORGANIZACAO'].astype("Int64")
    df['CO_NIVEL_HIERARQUIA'] = df['CO_NIVEL_HIERARQUIA'].astype("Int64")
    df['CO_ATIVIDADE'] = df['CO_ATIVIDADE'].astype("Int64")
    df['TP_UNIDADE'] = df['TP_UNIDADE'].astype("Int64")
    df['CO_CEP'] = df['CO_CEP'].astype("Int64")
    df['CO_TURNO_ATENDIMENTO'] = df['CO_TURNO_ATENDIMENTO'].astype("Int64")
    df['NU_CNPJ'] = df['NU_CNPJ'].astype("Int64")
    df['CO_NATUREZA_JUR'] = df['CO_NATUREZA_JUR'].astype("Int64")
    df['CO_MOTIVO_DESAB'] = df['CO_MOTIVO_DESAB'].astype("Int64")
    df["DATA_EXTRACAO"] = datetime.today()


    # In[24]:


    lista_float = ['NU_LATITUDE', 'NU_LONGITUDE']
    for coluna in lista_float:
        df[coluna] = df[coluna].replace({pd.NA: 0})
        df[coluna] = df[coluna].replace({np.nan: 0})
        df[coluna] = df[coluna].str.replace(',', '.', regex=False)
        df[coluna] = pd.to_numeric(df[coluna], errors='coerce')

    condicoes_bool = [
        df['CO_AMBULATORIAL_SUS'] == "SIM",
        df['CO_AMBULATORIAL_SUS'] == "NAO"
    ]

    valores_bool = [
        True,
        False
    ]

    df['CO_AMBULATORIAL_SUS'] = np.select(condicoes_bool, valores_bool, default=np.nan)

    lista_booleanos = ['ST_CENTRO_CIRURGICO', 'ST_CENTRO_OBSTETRICO', 'ST_CENTRO_NEONATAL',
        'ST_ATEND_HOSPITALAR', 'ST_SERVICO_APOIO', 'ST_ATEND_AMBULATORIAL', 'CO_AMBULATORIAL_SUS']

    for i in lista_booleanos:
        df[i] = df[i].astype(bool)

    df.replace({pd.NA: None}, inplace=True)
    df.replace({np.nan: None}, inplace=True)

    df.NU_ENDERECO = df.NU_ENDERECO.replace({"S/N": None}).replace(" ","")
    df.NU_ENDERECO = df.NU_ENDERECO.str.replace(" ","", regex=False)
    df.NU_ENDERECO = df.NU_ENDERECO.astype("Int64")


    # In[25]:


    df.replace({pd.NA: None}, inplace=True)
    df.replace({np.nan: None}, inplace=True)


    # In[26]:


    df_estabelecimento = df[['CO_UNIDADE', 'NO_RAZAO_SOCIAL', 'NO_FANTASIA', 'NU_CNPJ', 'NU_CNPJ_MANTENEDORA', 'NO_EMAIL', 'NU_TELEFONE', 'CO_MOTIVO_DESAB']]

    df_localizacao = df[['CO_UNIDADE', 'CO_CEP', 'NO_LOGRADOURO', 'NU_ENDERECO', 'NO_BAIRRO', 'NU_LATITUDE', 'NU_LONGITUDE', 'CO_IBGE', 'CO_UF']]

    df_organizacao = df[['CO_CNES', 'TP_UNIDADE', 'TP_GESTAO', 'CO_ESFERA_ADMINISTRATIVA', 'DS_ESFERA_ADMINISTRATIVA', 'CO_NATUREZA_JUR', 'CO_ATIVIDADE', 'CO_NIVEL_HIERARQUIA', 
                                'DS_NIVEL_HIERARQUIA', 'CO_NATUREZA_ORGANIZACAO', 'DS_NATUREZA_ORGANIZACAO']]

    df_turno = df[['CO_TURNO_ATENDIMENTO', 'DS_TURNO_ATENDIMENTO']]
    df_turno = df_turno.drop_duplicates()
    df_turno['CO_TURNO_ATENDIMENTO'] = df_turno['CO_TURNO_ATENDIMENTO'].replace({pd.NA: 0}).astype(int)

    df_servicos = df[['CO_CNES', 'ST_CENTRO_CIRURGICO', 'ST_CENTRO_OBSTETRICO', 'ST_CENTRO_NEONATAL',
        'ST_ATEND_HOSPITALAR', 'ST_SERVICO_APOIO', 'ST_ATEND_AMBULATORIAL', 'CO_AMBULATORIAL_SUS']]

    df_fato = df[['CO_CNES', 'CO_TURNO_ATENDIMENTO', 'CO_UNIDADE', "DATA_EXTRACAO"]]
    df_fato['CO_TURNO_ATENDIMENTO'] = df_fato['CO_TURNO_ATENDIMENTO'].replace({pd.NA: 0}).astype(int)


    # In[27]:


    def insert_data(cursor, table, columns, df_sep, conflict_column, update_on_conflict=False):

        if df.empty:
            print(f"[!] DataFrame vazio para {table}, nada inserido.")
            return

        if update_on_conflict:
            update_cols = [col for col in columns if col != conflict_column]
            set_clause = ", ".join([f"{col}=EXCLUDED.{col}" for col in update_cols])
            conflict_clause = f"ON CONFLICT ({conflict_column}) DO UPDATE SET {set_clause}"
        else:
            conflict_clause = f"ON CONFLICT ({conflict_column}) DO NOTHING"

        sql = f"""
            INSERT INTO {table} ({', '.join(columns)})
            VALUES %s
            {conflict_clause};
        """
        values = [tuple(row) for row in df_sep.to_numpy()]
        execute_values(cursor, sql, values, page_size=1000)
        action = "UPSERT (insert/update)" if update_on_conflict else "INSERT (ignorar duplicatas)"
        print(action)


    # In[11]:


    tables_config = {
        "dim_Estabelecimento": (
            ["cod_unidade", "nm_razao_social", "nm_fantasia", "num_cnpj",
            "num_cnpj_entidade", "email", "num_telefone", "cod_motivo_desab"],
            df_estabelecimento,
            "cod_unidade",
            True
        ),
        "dim_Localizacao": (
            ["cod_unidade", "cod_cep", "endereco", "numero", "bairro",
            "latitude", "longitude", "cod_ibge", "cod_uf"],
            df_localizacao,
            "cod_unidade",
            True
        ),
        "dim_Organizacao": (
            ["cod_cnes", "tp_unidade", "tp_gestao", "cod_esfera_administrativa",
            "dscr_esfera_administrativa", "cod_natureza_jur", "cod_atividade",
            "cod_nivel_hierarquia", "dscr_nivel_hierarquia", "cod_natureza_organizacao",
            "dscr_natureza_organizacao"],
            df_organizacao,
            "cod_cnes",
            True
        ),
        "dim_Turno": (
            ["cod_turno_atendimento", "dscr_turno_atendimento"],
            df_turno,
            "cod_turno_atendimento",
            True  
        ),
        "dim_Servicos": (
            ["cod_cnes", "st_faz_atendimento_ambulatorial_sus","st_centro_cirurgico", 
            "st_centro_obstetrico" ,"st_centro_neonatal" ,"st_atendimento_hospitalar",
            "st_servico_apoio" ,"st_atendimento_ambulatorial"],
            df_servicos,
            "cod_cnes",
            True
        ),
        "fato_Estabelecimento": (
            ["cod_cnes", "cod_turno_atendimento", "cod_unidade", "data_extracao"],
            df_fato,
            "cod_cnes",
            True
        )
    }

    try:
        with psycopg2.connect(**conn_params) as conn:
            with conn.cursor() as cursor:
                for table, (columns, df_sep, conflict_column, update_on_conflict) in tables_config.items():
                    print(f"Processando tabela: {table}")
                    insert_data(cursor, table, columns, df_sep, conflict_column, update_on_conflict)
            conn.commit()
            print("Dados inseridos com sucesso.")
    except OperationalError as e:
        print(f"Erro de conexão: {e}")
    except psycopg2.Error as e:
        print(f"Ocorreu um erro ao inserir dados: {e}")

if __name__ == "__main__":
    parser = argparse.ArgumentParser(description="ETL de estabelecimentos de saúde para o banco de dados.")
    parser.add_argument("--host", required=True, help="Endereço do servidor do banco de dados.")
    parser.add_argument("--port", required=True, help="Porta do servidor do banco de dados.")
    parser.add_argument("--dbname", required=True, help="Nome do banco de dados.")
    parser.add_argument("--user", required=True, help="Usuário para conexão com o banco.")
    parser.add_argument("--password", required=True, help="Senha para conexão com o banco.")
    
    args = parser.parse_args()
    
    main(args)