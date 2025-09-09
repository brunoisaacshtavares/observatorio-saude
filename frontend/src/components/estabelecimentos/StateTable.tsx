import { formatNumber } from "../../utils/formatters";

export type Row = {
  uf: string;
  estado: string;
  regiao: "Norte" | "Nordeste" | "Centro-Oeste" | "Sudeste" | "Sul" | string;
  estabelecimentos: number;
  populacao: number;
  estPor100k: number;
};

type Props = {
  rows: Row[];
  selectedUF?: string | null;
  onSelectUF?: (uf: string | null) => void;
};

function getDotColorByRegion(regiao: Row["regiao"]): string {
  switch (regiao) {
    case "Sudeste":
      return "#004F6D";
    case "Sul":
      return "#004F7D";
    case "Nordeste":
      return "#FFD166";
    case "Norte":
      return "#E63946";
    case "Centro-Oeste":
      return "#004F7D";
    default:
      return "#004F6D";
  }
}

export default function StateTable({ rows, selectedUF, onSelectUF }: Props) {
  const sorted = [...rows].sort((a, b) => b.estabelecimentos - a.estabelecimentos);

  return (
    <div className="overflow-x-auto">
      <table className="min-w-full text-sm">
        <thead className="text-left bg-slate-50 border-y border-slate-100">
          <tr className="text-slate-500">
            <th className="py-3 pl-5 pr-3 font-medium">Estado</th>
            <th className="py-3 px-3 font-medium">Região</th>
            <th className="py-3 px-3 font-medium">Estabelecimentos</th>
            <th className="py-3 px-3 font-medium">População</th>
            <th className="py-3 px-3 font-medium">Est./100k hab.</th>
          </tr>
        </thead>
        <tbody>
          {sorted.map((r) => {
            const dotColor = getDotColorByRegion(r.regiao);
            const selected = selectedUF === r.uf;
            return (
              <tr key={r.uf} className={`border-b border-slate-100 hover:bg-slate-50 transition ${selected ? "bg-slate-50" : ""}`} onClick={() => onSelectUF?.(selected ? null : r.uf)}>
                <td className="py-3 pl-5 pr-3">
                  <div className="flex items-center gap-2">
                    <span className="inline-block w-2.5 h-2.5 rounded-full" style={{ backgroundColor: dotColor }} aria-hidden />
                    <span className="text-slate-800">{r.estado}</span>
                  </div>
                </td>
                <td className="py-3 px-3 text-slate-600">{r.regiao}</td>
                <td className="py-3 px-3 text-slate-800">{formatNumber(r.estabelecimentos)}</td>
                <td className="py-3 px-3 text-slate-800">{formatNumber(r.populacao)}</td>
                <td className="py-3 px-3 text-slate-800">{r.estPor100k.toFixed(1)}</td>
              </tr>
            );
          })}
        </tbody>
      </table>
    </div>
  );
}
