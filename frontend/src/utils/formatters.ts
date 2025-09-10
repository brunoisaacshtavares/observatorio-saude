export const formatNumber = (n: number | null | undefined) => (typeof n === "number" ? n.toLocaleString("pt-BR") : "—");

export type UFCode = 11 | 12 | 13 | 14 | 15 | 16 | 17 | 21 | 22 | 23 | 24 | 25 | 26 | 27 | 28 | 29 | 31 | 32 | 33 | 35 | 41 | 42 | 43 | 50 | 51 | 52 | 53;

export type UFInfo = {
  sigla: string;
  nome: string;
  regiao: "Norte" | "Nordeste" | "Centro-Oeste" | "Sudeste" | "Sul";
  populacao: number;
};

export const UF_METADATA: Record<UFCode, UFInfo> = {
  11: { sigla: "RO", nome: "Rondônia", regiao: "Norte", populacao: 1600000 },
  12: { sigla: "AC", nome: "Acre", regiao: "Norte", populacao: 900000 },
  13: { sigla: "AM", nome: "Amazonas", regiao: "Norte", populacao: 4200000 },
  14: { sigla: "RR", nome: "Roraima", regiao: "Norte", populacao: 650000 },
  15: { sigla: "PA", nome: "Pará", regiao: "Norte", populacao: 8800000 },
  16: { sigla: "AP", nome: "Amapá", regiao: "Norte", populacao: 900000 },
  17: { sigla: "TO", nome: "Tocantins", regiao: "Norte", populacao: 1600000 },
  21: { sigla: "MA", nome: "Maranhão", regiao: "Nordeste", populacao: 7000000 },
  22: { sigla: "PI", nome: "Piauí", regiao: "Nordeste", populacao: 3300000 },
  23: { sigla: "CE", nome: "Ceará", regiao: "Nordeste", populacao: 9200000 },
  24: { sigla: "RN", nome: "Rio Grande do Norte", regiao: "Nordeste", populacao: 3500000 },
  25: { sigla: "PB", nome: "Paraíba", regiao: "Nordeste", populacao: 4000000 },
  26: { sigla: "PE", nome: "Pernambuco", regiao: "Nordeste", populacao: 9600000 },
  27: { sigla: "AL", nome: "Alagoas", regiao: "Nordeste", populacao: 3300000 },
  28: { sigla: "SE", nome: "Sergipe", regiao: "Nordeste", populacao: 2300000 },
  29: { sigla: "BA", nome: "Bahia", regiao: "Nordeste", populacao: 14500000 },
  31: { sigla: "MG", nome: "Minas Gerais", regiao: "Sudeste", populacao: 21000000 },
  32: { sigla: "ES", nome: "Espírito Santo", regiao: "Sudeste", populacao: 4100000 },
  33: { sigla: "RJ", nome: "Rio de Janeiro", regiao: "Sudeste", populacao: 17300000 },
  35: { sigla: "SP", nome: "São Paulo", regiao: "Sudeste", populacao: 46000000 },
  41: { sigla: "PR", nome: "Paraná", regiao: "Sul", populacao: 11600000 },
  42: { sigla: "SC", nome: "Santa Catarina", regiao: "Sul", populacao: 7300000 },
  43: { sigla: "RS", nome: "Rio Grande do Sul", regiao: "Sul", populacao: 11400000 },
  50: { sigla: "MS", nome: "Mato Grosso do Sul", regiao: "Centro-Oeste", populacao: 2800000 },
  51: { sigla: "MT", nome: "Mato Grosso", regiao: "Centro-Oeste", populacao: 3600000 },
  52: { sigla: "GO", nome: "Goiás", regiao: "Centro-Oeste", populacao: 7200000 },
  53: { sigla: "DF", nome: "Distrito Federal", regiao: "Centro-Oeste", populacao: 3100000 },
};

export function ufFromCode(uf?: number | null) {
  if (!uf || !(uf in UF_METADATA)) return null;
  return UF_METADATA[uf as UFCode];
}
