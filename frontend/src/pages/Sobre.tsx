import PageHeader from "../components/common/PageHeader";
import DataSourceCard from "../components/about/DataSourceCard";
import FeatureCard from "../components/about/FeatureCard";
import TeamMemberCard from "../components/about/TeamMemberCard";
import ContactItem from "../components/about/ContactItem";
import StatTile from "../components/about/StatTile";
import IconBadge from "../components/common/IconBadge";

import { Shield, Eye, Database, Globe, UserRound, Mail, Phone, MapPin } from "lucide-react";

export default function Sobre() {
  return (
    <div className="space-y-4">
      <PageHeader title="Sobre o Observatório" description="O Observatório de Saúde do Brasil é uma plataforma dedicada ao monitoramento e análise dos dados do sistema de saúde nacional, promovendo transparência e apoiando a tomada de decisões baseadas em evidências." />

      <section className="grid gap-4 md:grid-cols-2">
        <div className="card p-5">
          <div className="flex items-center gap-3">
            <IconBadge icon={<Shield />} bgClass="bg-[#004F6D]" rounded="rounded-md" size={44} />
            <p className="font-semibold text-slate-900">Nossa Missão</p>
          </div>
          <p className="text-sm text-slate-600 mt-2">Democratizar o acesso aos dados de saúde pública, fornecendo informações precisas e atualizadas sobre estabelecimentos de saúde e capacidade hospitalar para gestores, pesquisadores e cidadãos.</p>
        </div>

        <div className="card p-5">
          <div className="flex items-center gap-3">
            <IconBadge icon={<Eye />} bgClass="bg-[#00A67D]" rounded="rounded-md" size={44} />
            <p className="font-semibold text-slate-900">Nossa Visão</p>
          </div>
          <p className="text-sm text-slate-600 mt-2">Ser a principal referência em análise e visualização de dados de saúde no Brasil, contribuindo para um sistema de saúde mais eficiente, equitativo e transparente.</p>
        </div>
      </section>

      <section className="card p-0 overflow-hidden">
        <div className="px-5 py-4">
          <h3 className="text-lg font-semibold text-slate-900">Fontes de Dados</h3>
        </div>

        <div className="grid gap-4 md:grid-cols-2 p-4">
          <DataSourceCard title="CNES" description="Cadastro Nacional de Estabelecimentos de Saúde." linkHref="https://cnes.datasus.gov.br/" icon={<Database />} iconBg="bg-[#004F6D]/10" iconClassName="text-[#004F6D]" linkClass="text-[#004F6D]" />

          <DataSourceCard title="OpenDataSUS" description="Plataforma de dados abertos do Sistema Único de Saúde." linkHref="https://opendatasus.saude.gov.br/" icon={<Globe />} iconBg="bg-[#00A67D]/10" iconClassName="text-[#00A67D]" linkClass="text-[#00A67D]" />
        </div>
      </section>

      <section className="card p-0 overflow-hidden">
        <div className="px-5 py-4">
          <h3 className="text-lg font-semibold text-slate-900">Funcionalidades Principais</h3>
        </div>
        <div className="grid gap-4 md:grid-cols-3 p-4">
          <FeatureCard title="Dados em Tempo Real" description="Informações atualizadas regularmente diretamente das fontes oficiais." icon={<Database />} iconBg="bg-[#004F6D]" iconSize={72} />
          <FeatureCard title="Cobertura Nacional" description="Dados de todos os estados e regiões do Brasil em uma única plataforma." icon={<Globe />} iconBg="bg-[#00A67D]" iconSize={72} />
          <FeatureCard title="Transparência Total" description="Acesso livre e gratuito a informações públicas de saúde." icon={<Shield />} iconBg="bg-amber-400" iconSize={72} />
        </div>
      </section>

      <section className="card p-0 overflow-hidden">
        <div className="px-5 py-4">
          <h3 className="text-lg font-semibold text-slate-900">Nossa Equipe</h3>
        </div>
        <div className="grid gap-4 sm:grid-cols-2 lg:grid-cols-3 xl:grid-cols-6 p-4">
          <TeamMemberCard name="Bruno Isaac" role="QA/Infra" expertise="Qualidade de Software e Infraestrutura" icon={<UserRound />} />
          <TeamMemberCard name="Eduardo Xavier" role="Dev FullStack" expertise="Desenvolvimento Full-Stack" icon={<UserRound />} />
          <TeamMemberCard name="Enzo" role="UX/UI" expertise="Experiência do Usuário" icon={<UserRound />} />
          <TeamMemberCard name="Otávio" role="DBA" expertise="Administração de Banco de Dados" icon={<UserRound />} />
          <TeamMemberCard name="Murilo" role="Dev FullStack" expertise="Desenvolvimento Full-Stack" icon={<UserRound />} />
          <TeamMemberCard name="Rafael" role="Project Manager" expertise="Gestão de Projetos" icon={<UserRound />} />
        </div>
      </section>

      <section className="card p-0 overflow-hidden">
        <div className="px-5 py-4">
          <h3 className="text-lg font-semibold text-slate-900">Contato</h3>
        </div>
        <div className="grid gap-4 md:grid-cols-3 p-4">
          <ContactItem label="Email" value="contato@observatoriosaude.br" icon={<Mail />} kind="email" />
          <ContactItem label="Telefone" value="(61) 3361-2425" icon={<Phone />} kind="phone" />
          <ContactItem label="Localização" value="Brasília, DF – Brasil" icon={<MapPin />} kind="address" />
        </div>
      </section>

      <section className="card p-0 overflow-hidden">
        <div className="bg-gradient-to-r from-[#F9FAFB] to-[#EFF6FF]">
          <div className="px-5 py-4">
            <h3 className="text-lg font-semibold text-slate-900 text-center">Estatísticas da Plataforma</h3>
          </div>
          <div className="grid gap-4 md:grid-cols-4 p-4">
            <StatTile value="50K+" label="Usuários mensais" valueClassName="text-[#004F6D]" />
            <StatTile value="1.2M+" label="Consultas realizadas" valueClassName="text-[#00A67D]" />
            <StatTile value="27" label="Estados cobertos" valueClassName="text-[#FFD166]" />
            <StatTile value="99.9%" label="Disponibilidade" accent="red" />
          </div>
        </div>
      </section>
    </div>
  );
}
