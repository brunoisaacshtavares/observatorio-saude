using System.ComponentModel;
using MediatR;
using observatorio.saude.Domain.Dto;
using observatorio.saude.Domain.Enums;
using observatorio.saude.Domain.Utils;

namespace observatorio.saude.Application.Queries.GetDetalhesLeitosPaginados;

/// <summary>
///     Representa uma requisição paginada para buscar leitos hospitalares.
/// </summary>
public class GetDetalhesLeitosPaginadosQuery : IRequest<PaginatedResult<LeitosHospitalarDetalhadoDto>>
{
    /// <summary>
    ///     O número da página a ser retornada. O padrão é 1.
    /// </summary>
    [DefaultValue(1)]
    public int PageNumber { get; set; } = 1;

    /// <summary>
    ///     O número de itens por página. O padrão é 10.
    /// </summary>
    [DefaultValue(10)]
    public int PageSize { get; set; } = 10;

    /// <summary>
    ///     Filtra a busca pelo nome do estabelecimento (busca parcial).
    /// </summary>
    public string? Nome { get; set; }

    /// <summary>
    ///     Filtra a busca pelo Código CNES do estabelecimento.
    /// </summary>
    public long? CodCnes { get; set; }

    /// <summary>
    ///     O ano para o qual a ocupação deve ser calculada.
    ///     Caso não seja informado, o ano atual será utilizado.
    /// </summary>
    public int? Ano { get; set; }

    /// <summary>
    ///     O código UF (Unidade Federativa) para filtrar os estabelecimentos. Por exemplo: "SP", "RJ", "MG".
    /// </summary>
    public string? Uf { get; set; }

    /// <summary>
    ///     O Tipo de leito para filtrar os estabelecimentos. Por exemplo: UtiAdulto, UtiNeonatal, etc.
    /// </summary>
    public TipoLeito? Tipo { get; set; }

    /// <summary>
    ///     O ano e o mês buscado concatenado em uma string ex: 08/2023 => 202308
    /// </summary>
    public long? Anomes { get; set; }
}