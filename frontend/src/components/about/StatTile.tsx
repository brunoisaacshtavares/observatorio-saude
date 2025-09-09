type Props = {
  value: string;
  label: string;
  valueClassName?: string;
  accent?: "green" | "red" | "slate";
};

export default function StatTile({ value, label, valueClassName, accent = "green" }: Props) {
  const fallback = accent === "red" ? "text-rose-600" : accent === "slate" ? "text-slate-700" : "text-emerald-600";

  return (
    <div className="p-5 text-center">
      <p className={`text-2xl font-semibold ${valueClassName ?? fallback}`}>{value}</p>
      <p className="text-sm text-slate-600 mt-1">{label}</p>
    </div>
  );
}
