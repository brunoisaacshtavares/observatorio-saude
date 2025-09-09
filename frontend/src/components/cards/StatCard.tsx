import React, { ReactNode, isValidElement, cloneElement } from "react";

type Props = {
  label: string;
  value: string | number;
  hint?: string;
  icon?: ReactNode;
  iconBgClass?: string;
};

export default function StatCard({ label, value, hint, icon, iconBgClass }: Props) {
  return (
    <div className="card p-4">
      <div className="flex items-start justify-between">
        <p className="card-head">{label}</p>

        {icon ? (
          <span className={`inline-flex h-10 w-10 items-center justify-center rounded-lg ${iconBgClass ?? "bg-slate-300"}`}>
            {isValidElement(icon)
              ? cloneElement(icon, {
                  size: 20,
                  className: "text-white",
                })
              : icon}
          </span>
        ) : null}
      </div>

      <p className="card-value mt-2">{value}</p>
      {hint ? <p className="text-xs text-slate-500 mt-1">{hint}</p> : null}
    </div>
  );
}
