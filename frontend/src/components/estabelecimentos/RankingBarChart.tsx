import { BarChart, Bar, XAxis, YAxis, CartesianGrid, ResponsiveContainer, Tooltip } from "recharts";

type Props = {
  title: string;
  data: { estado: string; estabelecimentos: number }[];
};

export default function RankingBarChart({ title, data }: Props) {
  return (
    <div className="card p-4">
      <p className="text-sm font-medium text-slate-700 mb-3">{title}</p>
      <div className="h-64">
        <ResponsiveContainer width="100%" height="100%">
          <BarChart data={data} margin={{ top: 10, right: 10, bottom: 0, left: 0 }}>
            <CartesianGrid strokeDasharray="3 3" vertical={false} />
            <XAxis dataKey="estado" tickLine={false} axisLine={false} fontSize={12} />
            <YAxis tickLine={false} axisLine={false} fontSize={12} />
            <Tooltip />
            <Bar dataKey="estabelecimentos" radius={[4, 4, 0, 0]} fill="#004F6D" />
          </BarChart>
        </ResponsiveContainer>
      </div>
    </div>
  );
}
