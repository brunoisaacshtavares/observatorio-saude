import IconBadge from "../common/IconBadge";
import { ReactElement } from "react";

type Props = {
  name: string;
  role: string;
  expertise?: string;
  icon?: ReactElement;
  iconBgClass?: string;
  iconSize?: number;
};

export default function TeamMemberCard({ name, role, expertise, icon, iconBgClass, iconSize = 60 }: Props) {
  return (
    <div className="p-4 text-center">
      <div className="flex justify-center mb-2">
        <IconBadge icon={icon!} bgClass={iconBgClass ?? "bg-gradient-to-br from-[#004F6D] to-[#00A67D]"} rounded="rounded-full" size={iconSize} />
      </div>
      <p className="font-medium text-slate-900">{name}</p>
      <p className="text-xs text-slate-600">{role}</p>
      {expertise ? <p className="text-xs text-slate-400">{expertise}</p> : null}
    </div>
  );
}
