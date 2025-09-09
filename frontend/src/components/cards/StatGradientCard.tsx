import { ReactElement } from "react";

type Props = {
  title: string;
  value: string | number;
  sublabel?: string;
  gradientFrom: string;
  gradientTo: string;
  icon?: ReactElement;
};

export default function StatGradientCard({ title, value, sublabel, gradientFrom, gradientTo, icon }: Props) {
  return (
    <div
      className="rounded-xl px-4 py-4 text-white flex items-center justify-between shadow-sm"
      style={{
        background: `linear-gradient(90deg, ${gradientFrom}, ${gradientTo})`,
      }}
    >
      <div>
        <p className="text-xs/5 opacity-90">{title}</p>
        <p className="text-2xl font-semibold">{value}</p>
        {sublabel ? <p className="text-xs/5 opacity-90 mt-1">{sublabel}</p> : null}
      </div>
      {icon ? (
        <div className="w-10 h-10 rounded-xl bg-white/10 flex items-center justify-center">
          {}
          {}
          {icon && (typeof icon === "object" ? { ...icon, props: { ...icon.props, color: "white" } } : icon)}
        </div>
      ) : null}
    </div>
  );
}
