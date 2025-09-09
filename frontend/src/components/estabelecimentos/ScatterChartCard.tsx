import { ResponsiveContainer, ScatterChart, Scatter, XAxis, YAxis, CartesianGrid, Tooltip, ZAxis } from "recharts";

type Props = {
  title: string;
  data: { estado: string; populacao: number; estabelecimentos: number }[];
};

export default function ScatterChartCard({ title, data }: Props) {
  const enriched = data.map((d) => ({
    ...d,
    z: Math.max(6, Math.min(24, (d.estabelecimentos / 4500) * 24)),
  }));

  return (
    <div className="card p-4">
      <p className="text-sm font-medium text-slate-700 mb-3">{title}</p>
      <div className="h-64 rounded-lg bg-gradient-to-tr from-slate-50 to-emerald-50">
        <ResponsiveContainer width="100%" height="100%">
          <ScatterChart margin={{ top: 10, right: 10, bottom: 10, left: 0 }}>
            <CartesianGrid strokeDasharray="3 3" />
            <XAxis type="number" dataKey="populacao" name="População" tickFormatter={(v) => `${Math.round(v / 1_000_000)}M`} tickLine={false} axisLine={false} fontSize={12} />
            <YAxis type="number" dataKey="estabelecimentos" name="Estabelecimentos" tickLine={false} axisLine={false} fontSize={12} />
            <ZAxis type="number" dataKey="z" range={[6, 24]} />
            <Tooltip cursor={{ strokeDasharray: "3 3" }} formatter={(v: any, name: any, props: any) => (name === "populacao" ? `${(v / 1_000_000).toFixed(2)}M` : v)} labelFormatter={() => ""} />
            <Scatter data={enriched} fill="#00A67D" />
          </ScatterChart>
        </ResponsiveContainer>
      </div>
    </div>
  );
}
