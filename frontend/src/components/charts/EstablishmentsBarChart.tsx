import { useQuery } from "@tanstack/react-query";
import { getTopUFByAmostra } from "../../services/establishments";
import { BarChart, Bar, XAxis, YAxis, Tooltip, ResponsiveContainer } from "recharts";

function ufLabel(uf: number) {
  const map: Record<number, string> = {
    11: "RO",
    12: "AC",
    13: "AM",
    14: "RR",
    15: "PA",
    16: "AP",
    17: "TO",
    21: "MA",
    22: "PI",
    23: "CE",
    24: "RN",
    25: "PB",
    26: "PE",
    27: "AL",
    28: "SE",
    29: "BA",
    31: "MG",
    32: "ES",
    33: "RJ",
    35: "SP",
    41: "PR",
    42: "SC",
    43: "RS",
    50: "MS",
    51: "MT",
    52: "GO",
    53: "DF",
  };
  return map[uf] ?? String(uf);
}

export default function EstablishmentsBarChart() {
  const { data, isLoading, isError } = useQuery({
    queryKey: ["top-uf-amostra"],
    queryFn: () => getTopUFByAmostra(300),
  });

  return (
    <div className="card p-4 h-[360px]">
      <h4 className="card-head mb-3">Estados com Maior Número de Estabelecimentos (amostra)</h4>
      {isLoading ? (
        <p className="text-sm text-slate-500">Carregando…</p>
      ) : isError ? (
        <p className="text-sm text-red-600">Erro ao carregar dados do gráfico.</p>
      ) : data && data.length ? (
        <ResponsiveContainer width="100%" height="100%">
          <BarChart data={data.map((d) => ({ uf: ufLabel(d.uf), estabelecimentos: d.qty }))}>
            <XAxis dataKey="uf" />
            <YAxis />
            <Tooltip />
            <Bar dataKey="estabelecimentos" />
          </BarChart>
        </ResponsiveContainer>
      ) : (
        <p className="text-sm text-slate-500">Sem dados suficientes.</p>
      )}
    </div>
  );
}
