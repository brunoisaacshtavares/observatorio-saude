import type { ReactNode } from "react";
import { Search } from "lucide-react";

type ButtonSpec = {
  label: string;
  onClick?: () => void;
  icon?: ReactNode;
  variant?: "ghost" | "primary";
  className?: string;
};

type Props = {
  value: string;
  onChange: (v: string) => void;
  onClear?: () => void;
  rightButtons?: ButtonSpec[];
};

export default function SearchBar({ value, onChange, onClear, rightButtons = [] }: Props) {
  return (
    <section className="card p-3">
      <div className="flex flex-col gap-3 md:flex-row md:items-center">
        {}
        <div className="flex-1">
          <div className="relative">
            <Search className="absolute left-3 top-1/2 -translate-y-1/2 text-slate-400" size={18} />
            <input value={value} onChange={(e) => onChange(e.target.value)} placeholder="Buscar por estado ou cidade..." className="w-full pl-10 pr-10 py-2 rounded-lg border border-slate-200 focus:outline-none focus:ring-2 focus:ring-slate-200" />
            {value && (
              <button className="absolute right-3 top-1/2 -translate-y-1/2 text-slate-400 hover:text-slate-600" onClick={onClear} aria-label="Limpar">
                ×
              </button>
            )}
          </div>
        </div>

        {}
        <div className="flex gap-2 md:justify-end">
          {rightButtons.map((b, i) => {
            const base = b.variant === "primary" ? "inline-flex items-center gap-2 px-3 py-2 rounded-lg text-white bg-[#004F6D] hover:opacity-95 transition" : "inline-flex items-center gap-2 px-3 py-2 rounded-lg border border-slate-200 text-slate-700 hover:bg-slate-50 transition";
            return (
              <button key={i} onClick={b.onClick} className={`${base} ${b.className ?? ""} overflow-hidden text-ellipsis whitespace-nowrap`}>
                {b.icon} {b.label}
              </button>
            );
          })}
        </div>
      </div>
    </section>
  );
}
