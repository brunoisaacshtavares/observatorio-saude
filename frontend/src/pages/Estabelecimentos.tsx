import PageHeader from "../components/common/PageHeader";
import SearchBar from "../components/estabelecimentos/SearchBar";
import RankingBarChart from "../components/estabelecimentos/RankingBarChart";
import ScatterChartCard from "../components/estabelecimentos/ScatterChartCard";
import StateTable from "../components/estabelecimentos/StateTable";
import StatGradientCard from "../components/cards/StatGradientCard";
import { Building2, UsersRound, Filter } from "lucide-react";
import { useMemo, useState } from "react";
import { formatNumber } from "../utils/formatters";

const SAMPLE_STATES = [
  { uf: "SP", estado: "São Paulo", regiao: "Sudeste", estabelecimentos: 4256, populacao: 46463132 },
  { uf: "MG", estado: "Minas Gerais", regiao: "Sudeste", estabelecimentos: 3124, populacao: 21411923 },
  { uf: "RJ", estado: "Rio de Janeiro", regiao: "Sudeste", estabelecimentos: 2845, populacao: 17463349 },
  { uf: "BA", estado: "Bahia", regiao: "Nordeste", estabelecimentos: 2567, populacao: 14882584 },
  { uf: "PR", estado: "Paraná", regiao: "Sul", estabelecimentos: 2324, populacao: 11597484 },
  { uf: "RS", estado: "Rio Grande do Sul", regiao: "Sul", estabelecimentos: 2043, populacao: 11468630 },
  { uf: "PE", estado: "Pernambuco", regiao: "Nordeste", estabelecimentos: 1876, populacao: 9874793 },
  { uf: "CE", estado: "Ceará", regiao: "Nordeste", estabelecimentos: 1654, populacao: 9240580 },
  { uf: "PA", estado: "Pará", regiao: "Norte", estabelecimentos: 1432, populacao: 8777124 },
  { uf: "SC", estado: "Santa Catarina", regiao: "Sul", estabelecimentos: 1298, populacao: 7338473 },
  { uf: "MA", estado: "Maranhão", regiao: "Nordeste", estabelecimentos: 1152, populacao: 7153492 },
  { uf: "GO", estado: "Goiás", regiao: "Centro-Oeste", estabelecimentos: 1087, populacao: 7260589 },
];

export default function Estabelecimentos() {
  const [q, setQ] = useState("");
  const [estadoSelecionado, setEstadoSelecionado] = useState<string | null>(null);

  const filtered = useMemo(() => {
    const base = [...SAMPLE_STATES];
    if (!q) return base;
    const term = q.toLowerCase();
    return base.filter((s) => s.estado.toLowerCase().includes(term) || s.regiao.toLowerCase().includes(term) || s.uf.toLowerCase().includes(term));
  }, [q]);

  const totalNacional = useMemo(() => SAMPLE_STATES.reduce((acc, s) => acc + s.estabelecimentos, 0), []);

  const mediaNacional = Math.round(totalNacional / SAMPLE_STATES.length);
  const estabPor100k = (s: (typeof SAMPLE_STATES)[number]) => s.estabelecimentos / (s.populacao / 100_000);

  const coberturaMedia = useMemo(() => {
    const media = SAMPLE_STATES.reduce((acc, s) => acc + estabPor100k(s), 0) / SAMPLE_STATES.length;
    return media;
  }, []);

  return (
    <div className="space-y-4">
      {}
      <PageHeader title="Estabelecimentos de Saúde" description="Análise detalhada dos estabelecimentos cadastrados no CNES por região." />

      {}
      <SearchBar
        value={q}
        onChange={setQ}
        onClear={() => setQ("")}
        rightButtons={[
          {
            label: "Todos os Estados",
            variant: "ghost",
            onClick: () => {
              setEstadoSelecionado(null);
              setQ("");
            },
          },
          {
            label: "Filtrar",
            variant: "primary",
            icon: <Filter size={16} />,
            onClick: () => {
              alert("Filtros (em breve)");
            },
          },
        ]}
      />

      {}
      <section className="grid gap-4 lg:grid-cols-2">
        <RankingBarChart
          title="Ranking de Estados por Número de Estabelecimentos"
          data={SAMPLE_STATES.map((s) => ({
            estado: s.estado,
            estabelecimentos: s.estabelecimentos,
          }))}
        />

        <ScatterChartCard
          title="Relação População x Estabelecimentos"
          data={SAMPLE_STATES.map((s) => ({
            estado: s.estado,
            populacao: s.populacao,
            estabelecimentos: s.estabelecimentos,
          }))}
        />
      </section>

      {}
      <section className="card p-0 overflow-hidden">
        <div className="px-5 py-4">
          <h3 className="text-base md:text-lg font-semibold text-slate-900">Detalhes por Estado</h3>
        </div>
        <StateTable
          rows={filtered.map((s) => ({
            ...s,
            estPor100k: estabPor100k(s),
          }))}
          onSelectUF={setEstadoSelecionado}
          selectedUF={estadoSelecionado}
        />
      </section>

      {}
      <section className="grid gap-4 md:grid-cols-3">
        <StatGradientCard title="Total Nacional" value={formatNumber(totalNacional)} sublabel="Estabelecimentos" gradientFrom="#004F6D" gradientTo="#003A52" icon={<Building2 />} />
        <StatGradientCard title="Média Nacional" value={formatNumber(mediaNacional)} sublabel="Por estado" gradientFrom="#00A67D" gradientTo="#008A67" icon={<UsersRound />} />
        <StatGradientCard title="Cobertura Média" value={coberturaMedia.toFixed(1)} sublabel="Est./100k hab." gradientFrom="#FFD166" gradientTo="#E6BC5A" icon={<Filter />} />
      </section>
    </div>
  );
}
