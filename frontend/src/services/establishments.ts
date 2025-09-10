import { api } from "./api";
import type { Paginado, EstabelecimentoItem } from "../types/cnes";
import { UF_METADATA } from "../utils/formatters";

export async function getEstabelecimentos(page = 1, pageSize = 10) {
  const { data } = await api.get<Paginado<EstabelecimentoItem>>(`/api/v1/Estabelecimento?pageNumber=${page}&pageSize=${pageSize}`);
  return data;
}

export async function getTopUFByAmostra(limit = 300) {
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

export async function getUFCountsByAmostra(limit = 500) {
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

  return [...counts.entries()].map(([uf, qty]) => ({ uf, qty, meta: UF_METADATA[uf as keyof typeof UF_METADATA] })).sort((a, b) => b.uf - a.uf);
}

export async function getEstabelecimentosAmostra(limit = 600) {
  const items: EstabelecimentoItem[] = [];
  const pageSize = Math.min(100, limit);
  let page = 1;
  while (items.length < limit) {
    const resp = await getEstabelecimentos(page, pageSize);
    items.push(...resp.items);
    if (resp.items.length < pageSize) break;
    page += 1;
  }
  return items.slice(0, limit);
}
