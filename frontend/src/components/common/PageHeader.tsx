import React from "react";

type Props = {
  title: string;
  description?: string;
  className?: string;
};

export default function PageHeader({ title, description, className = "" }: Props) {
  return (
    <div className={"px-5 py-5 bg-gradient-to-r from-[#004F6D] to-[#00A67D] text-white rounded-xl " + className}>
      <h2 className="text-xl md:text-2xl font-semibold">{title}</h2>
      {description ? <p className="text-sm md:text-base opacity-90 mt-1">{description}</p> : null}
    </div>
  );
}
