import IconBadge from "../common/IconBadge";
import { ReactElement } from "react";

type Props = {
  label: string;
  value: string;
  icon: ReactElement;
  kind?: "email" | "phone" | "address";
};

export default function ContactItem({ label, value, icon, kind }: Props) {
  const palette = kind === "email" ? { bg: "bg-[#dbeafe]", icon: "text-[#2563eb]" } : kind === "phone" ? { bg: "bg-[#dcfce7]", icon: "text-[#16a34a]" } : { bg: "bg-[#fef3c7]", icon: "text-[#d97706]" };

  const content =
    kind === "email" ? (
      <a href={`mailto:${value}`} className="text-slate-700 hover:underline">
        {value}
      </a>
    ) : kind === "phone" ? (
      <a href={`tel:${value.replace(/\D/g, "")}`} className="text-slate-700 hover:underline">
        {value}
      </a>
    ) : (
      <span className="text-slate-700">{value}</span>
    );

  return (
    <div className="p-4 flex items-start gap-3">
      <IconBadge icon={icon} bgClass={`${palette.bg}`} iconClassName={`${palette.icon}`} rounded="rounded-xl" size={44} iconSize={22} />
      <div>
        <p className="text-sm font-medium text-slate-900">{label}</p>
        <p className="text-sm">{content}</p>
      </div>
    </div>
  );
}
