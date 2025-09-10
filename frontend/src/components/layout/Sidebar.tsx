import { NavLink } from "react-router-dom";
import { BarChart2, Bed, Building2, Home, Info } from "lucide-react";

const linkDefault = "relative flex w-full items-center gap-3 pl-4 pr-0 py-2 " + "rounded-none text-slate-700 hover:bg-slate-100 transition";

const linkActive = "relative flex w-full items-center gap-3 pl-4 pr-0 py-2 " + "bg-[#004F6D] text-white font-semibold rounded-none hover:bg-[#004F6D] " + "after:content-[''] after:absolute after:top-0 after:right-0 " + "after:h-full after:w-1.5 after:bg-[#00A67D]";

export default function Sidebar() {
  return (
    <aside
      className="w-64 shrink-0 bg-white border-r border-slate-200
                 h-[70vh] self-start overflow-hidden flex flex-col
                 shadow-[0_10px_18px_rgba(0,0,0,0.06)]"
    >
      <div className="p-4">
        <h1 className="text-lg font-semibold">Observatório de Saúde</h1>
        <p className="text-xs text-slate-500">Brasil</p>
      </div>

      <div className="h-px w-full bg-slate-200 mb-2" />

      <nav className="px-0 py-1 space-y-1">
        <NavLink to="/" end className={({ isActive }) => (isActive ? linkActive : linkDefault)}>
          <Home size={18} /> Visão Geral
        </NavLink>

        <NavLink to="/estabelecimentos" className={({ isActive }) => (isActive ? linkActive : linkDefault)}>
          <Building2 size={18} /> Estabelecimentos
        </NavLink>

        <NavLink to="/hospitais-leitos" className={({ isActive }) => (isActive ? linkActive : linkDefault)}>
          <Bed size={18} /> Hospitais & Leitos
        </NavLink>

        <NavLink to="/analises" className={({ isActive }) => (isActive ? linkActive : linkDefault)}>
          <BarChart2 size={18} /> Análises
        </NavLink>

        <NavLink to="/sobre" className={({ isActive }) => (isActive ? linkActive : linkDefault)}>
          <Info size={18} /> Sobre
        </NavLink>
      </nav>

      <div className="mt-auto p-4 text-xs text-slate-500">
        <p>Dados: CNES e SUS</p>
        <p>Atualizado em tempo real</p>
      </div>
    </aside>
  );
}
