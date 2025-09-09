import IconBadge from "../common/IconBadge";
import { ReactElement } from "react";

type Props = {
  title: string;
  description: string;
  linkHref: string;
  linkLabel?: string;
  icon: ReactElement;
  iconBg?: string;
  iconClassName?: string;
  linkClass?: string;
};

export default function DataSourceCard({ title, description, linkHref, linkLabel = "Acessar fonte →", icon, iconBg, iconClassName, linkClass }: Props) {
  return (
    <div className="card p-4 md:p-5 flex items-start gap-3 border border-slate-300">
      <IconBadge icon={icon} bgClass={iconBg ?? "bg-slate-100"} iconClassName={iconClassName} size={44} rounded="rounded-xl" />
      <div className="flex-1">
        <h4 className="font-medium text-slate-900">{title}</h4>
        <p className="text-sm text-slate-600">{description}</p>
        <a href={linkHref} target="_blank" rel="noreferrer" className={`text-sm font-medium inline-block mt-2 hover:underline ${linkClass ?? "text-slate-700"}`}>
          {linkLabel}
        </a>
      </div>
    </div>
  );
}
