import IconBadge from "../common/IconBadge";
import { ReactElement } from "react";

type Props = {
  title: string;
  description: string;
  icon: ReactElement;
  iconBg?: string;
  iconSize?: number;
};

export default function FeatureCard({ title, description, icon, iconBg = "bg-[#004F6D]", iconSize = 72 }: Props) {
  return (
    <div className="p-5 text-center">
      <div className="flex justify-center mb-3">
        <IconBadge icon={icon} bgClass={iconBg} rounded="rounded-full" size={iconSize} />
      </div>
      <p className="font-semibold text-slate-900">{title}</p>
      <p className="text-sm text-slate-600">{description}</p>
    </div>
  );
}
