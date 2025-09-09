import { ReactElement, cloneElement, isValidElement } from "react";

type Props = {
  icon: ReactElement;
  bgClass?: string;
  size?: number;
  rounded?: string;
  iconClassName?: string;
  iconSize?: number;
};

export default function IconBadge({ icon, bgClass = "bg-slate-300", size = 40, rounded = "rounded-lg", iconClassName, iconSize }: Props) {
  const innerSize = iconSize ?? Math.round(size * 0.45);

  return (
    <span className={`inline-flex items-center justify-center ${rounded} ${bgClass}`} style={{ width: size, height: size }}>
      {isValidElement(icon)
        ? cloneElement(icon, {
            size: innerSize,
            className: iconClassName ?? "text-white",
          })
        : icon}
    </span>
  );
}
