export const formatNumber = (n: number | null | undefined) => (typeof n === "number" ? n.toLocaleString("pt-BR") : "—");
