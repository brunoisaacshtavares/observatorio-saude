import { api } from "./api";
import type { Paginado, EstabelecimentoItem } from "../types/cnes";

export async function getEstabelecimentos(page = 1, pageSize = 10) {
  const { data } = await api.get<Paginado<EstabelecimentoItem>>(`/api/estabelecimentos?page=${page}&pageSize=${pageSize}`);
  return data;
}

/** Apenas para estimativa rápida do chart (amostra) */
export async function getTopUFByAmostra(limit = 300) {
  // busca algumas páginas para ter massa mínima
  const pageSize = Math.min(100, limit);
  const pages = Math.ceil(limit / pageSize);

  const counts = new Map<number, number>();
  for (let p = 1; p <= pages; p++) {
    const resp = await getEstabelecimentos(p, pageSize);
    for (const it of resp.items) {
      const uf = it.localizacao.codUf ?? -1;
      if (uf === -1) continue;
      counts.set(uf, (counts.get(uf) ?? 0) + 1);
    }
  }

  const arr = [...counts.entries()]
    .map(([uf, qty]) => ({ uf, qty }))
    .sort((a, b) => b.qty - a.qty)
    .slice(0, 10);

  return arr;
}
