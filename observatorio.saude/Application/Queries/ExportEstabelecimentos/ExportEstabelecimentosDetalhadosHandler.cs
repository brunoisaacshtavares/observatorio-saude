using System.Runtime.CompilerServices;
using MediatR;
using observatorio.saude.Application.Services.Clients;
using observatorio.saude.Domain.Dto;
using observatorio.saude.Domain.Interface;

namespace observatorio.saude.Application.Queries.ExportEstabelecimentos;

public class StreamEstabelecimentosDetalhadosHandler(
    IEstabelecimentoRepository estabelecimentoRepository,
    IIbgeApiClient ibgeApiClient)
    : IRequestHandler<ExportEstabelecimentosDetalhadosQuery, IAsyncEnumerable<ExportEstabelecimentoDto>>
{
    public async Task<IAsyncEnumerable<ExportEstabelecimentoDto>> Handle(
        ExportEstabelecimentosDetalhadosQuery request, CancellationToken cancellationToken)
    {
        var ufs = await ibgeApiClient.FindUfsAsync();
        var mapaUfs = ufs.ToDictionary(u => u.Id, u => u.Sigla);

        List<long>? codUfs = null;

        if (request.Uf != null)
        {
            var ufsMapeadas = ufs
                .Where(u => request.Uf.Contains(u.Sigla, StringComparer.OrdinalIgnoreCase))
                .ToList();
            if (ufsMapeadas.Count > 0) codUfs = ufsMapeadas.Select(u => u.Id).ToList();
        }

        var streamDoRepositorio = estabelecimentoRepository.StreamAllForExportAsync(codUfs);

        return ProcessarStream(streamDoRepositorio, mapaUfs, cancellationToken);
    }

    private async IAsyncEnumerable<ExportEstabelecimentoDto> ProcessarStream(
        IAsyncEnumerable<ExportEstabelecimentoDto> inputStream,
        Dictionary<long, string> mapaUfs,
        [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        await foreach (var item in inputStream.WithCancellation(cancellationToken))
        {
            if (item.CodUfParaMapeamento.HasValue)
                item.Uf = mapaUfs.GetValueOrDefault(item.CodUfParaMapeamento.Value, "");

            yield return item;
        }
    }
}