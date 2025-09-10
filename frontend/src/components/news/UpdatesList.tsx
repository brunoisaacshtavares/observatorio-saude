type Item = {
  title: string;
  description: string;
  date: string;
  color?: "green" | "amber" | "sky";
};

type Props = {
  items: Item[];
  title?: string;
};

function colorDot(c: Item["color"]) {
  switch (c) {
    case "amber":
      return "bg-amber-500";
    case "sky":
      return "bg-sky-500";
    default:
      return "bg-emerald-500";
  }
}

function relativePtBR(iso: string) {
  const d = new Date(iso);
  const now = new Date();
  const ms = now.getTime() - d.getTime();
  const days = Math.floor(ms / (1000 * 60 * 60 * 24));
  const hours = Math.floor(ms / (1000 * 60 * 60));

  if (days <= 0) return `Há ${Math.max(1, hours)} hora${hours > 1 ? "s" : ""}`;
  if (days === 1) return "Há 1 dia";
  if (days < 7) return `Há ${days} dias`;
  if (days < 14) return "Há 1 semana";
  const weeks = Math.floor(days / 7);
  return `Há ${weeks} semana${weeks > 1 ? "s" : ""}`;
}

export default function UpdatesList({ items, title = "Atualizações Recentes" }: Props) {
  return (
    <div className="card overflow-hidden">
      <div className="px-5 py-4 border-b border-slate-100">
        <h3 className="text-base font-semibold text-slate-900">{title}</h3>
      </div>

      <ul className="divide-y divide-slate-100">
        {items.map((it, idx) => (
          <li key={idx} className="flex items-start gap-3 px-5 py-4">
            <span className={`mt-2 h-2.5 w-2.5 rounded-full ${colorDot(it.color)}`} />
            <div className="flex-1">
              <p className="font-medium text-slate-900">{it.title}</p>
              <p className="text-sm text-slate-600">{it.description}</p>
              <p className="text-xs text-slate-400 mt-1">{relativePtBR(it.date)}</p>
            </div>
          </li>
        ))}
      </ul>
    </div>
  );
}
