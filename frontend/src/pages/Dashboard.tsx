import Header from "../components/layout/Header";
import StatCard from "../components/cards/StatCard";
import { Building2, Bed, TrendingUp, MapPin } from "lucide-react";
import { useQuery } from "@tanstack/react-query";
import { getEstabelecimentos } from "../services/establishments";
import { formatNumber } from "../utils/formatters";
import EstablishmentsBarChart from "../components/charts/EstablishmentsBarChart";
import MapPlaceHolder from "../components/map/MapPlaceHolder";
import UpdatesList from "../components/news/UpdatesList";

export default function Dashboard() {
  const { data, isLoading, isError } = useQuery({
    queryKey: ["estabelecimentos-total"],
    queryFn: () => getEstabelecimentos(1, 1),
  });

  const totalEstabs = data?.totalCount ?? 0;

  const updates = [
    {
      title: "Novos dados CNES disponibilizados",
      description: "Atualização mensal com dados de dezembro de 2024.",
      date: "2025-09-07",
      color: "green" as const,
    },
    {
      title: "Expansão de leitos em São Paulo",
      description: "Aumento de 5% na capacidade hospitalar.",
      date: "2025-09-04",
      color: "amber" as const,
    },
    {
      title: "Relatório regional Norte publicado",
      description: "Análise detalhada da cobertura na região Norte.",
      date: "2025-09-02",
      color: "sky" as const,
    },
  ];

  return (
    <div className="space-y-4">
      <Header />

      {}
      <section className="grid gap-4 md:grid-cols-4">
        <StatCard label="Total de Estabelecimentos" value={isLoading ? "…" : isError ? "Erro" : formatNumber(totalEstabs)} icon={<Building2 />} iconBgClass="bg-[#004F6D]" hint="Cadastrados no CNES" />

        <StatCard label="Total de Leitos" value="—" hint="Aguardando endpoint de leitos" icon={<Bed />} iconBgClass="bg-[#00A67D]" />

        <StatCard label="Melhor Cobertura" value="—" hint="Aguardando métrica de cobertura" icon={<TrendingUp />} iconBgClass="bg-[#22C55E]" />

        <StatCard label="Menor Cobertura" value="—" hint="Aguardando métrica de cobertura" icon={<MapPin />} iconBgClass="bg-amber-400" />
      </section>

      {}
      <section className="grid gap-4 lg:grid-cols-2">
        <EstablishmentsBarChart />
        <MapPlaceHolder />
      </section>

      {}
      <section>
        <UpdatesList items={updates} />
      </section>
    </div>
  );
}
