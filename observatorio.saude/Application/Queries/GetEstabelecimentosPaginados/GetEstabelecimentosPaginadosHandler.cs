using MediatR;
using observatorio.saude.Domain.Entities;
using observatorio.saude.Domain.Interface;
using observatorio.saude.Domain.Utils;

namespace observatorio.saude.Application.Queries.GetEstabelecimentosPaginados;

public class GetEstabelecimentosPaginadosHandler(IEstabelecimentoRepository estabelecimentoRepository)
    : IRequestHandler<GetEstabelecimentosPaginadosQuery, PaginatedResult<Estabelecimento>>
{
    private readonly IEstabelecimentoRepository _estabelecimentoRepository = estabelecimentoRepository;

    public async Task<PaginatedResult<Estabelecimento>> Handle(GetEstabelecimentosPaginadosQuery request,
        CancellationToken cancellationToken)
    {
        var pagedResultFromDb =
            await _estabelecimentoRepository.GetPagedWithDetailsAsync(request.PageNumber, request.PageSize);

        var estabelecimentosDto = pagedResultFromDb.Items.Select(e => new Estabelecimento
        {
            CodCnes = e.CodCnes,
            DataExtracao = e.DataExtracao,

            Caracteristicas = e.CaracteristicaEstabelecimento != null
                ? new CaracteristicaEstabelecimento
                {
                    CodUnidade = e.CaracteristicaEstabelecimento.CodUnidade,
                    NmRazaoSocial = e.CaracteristicaEstabelecimento.NmRazaoSocial,
                    NmFantasia = e.CaracteristicaEstabelecimento.NmFantasia,
                    NumCnpj = e.CaracteristicaEstabelecimento.NumCnpj,
                    NumCnpjEntidade = e.CaracteristicaEstabelecimento.NumCnpjEntidade,
                    Email = e.CaracteristicaEstabelecimento.Email,
                    NumTelefone = e.CaracteristicaEstabelecimento.NumTelefone
                }
                : null,

            Localizacao = e.Localizacao != null
                ? new Localizacao
                {
                    CodUnidade = e.Localizacao.CodUnidade,
                    CodCep = e.Localizacao.CodCep,
                    Endereco = e.Localizacao.Endereco,
                    Numero = e.Localizacao.Numero,
                    Bairro = e.Localizacao.Bairro,
                    Latitude = e.Localizacao.Latitude,
                    Longitude = e.Localizacao.Longitude,
                    CodIbge = e.Localizacao.CodIbge,
                    CodUf = e.Localizacao.CodUf
                }
                : null,

            Organizacao = e.Organizacao != null
                ? new Organizacao
                {
                    CodCnes = e.Organizacao.CodCnes,
                    TpUnidade = e.Organizacao.TpUnidade,
                    TpGestao = e.Organizacao.TpGestao,
                    DscrEsferaAdministrativa = e.Organizacao.DscrEsferaAdministrativa,
                    DscrNivelHierarquia = e.Organizacao.DscrNivelHierarquia,
                    DscrNaturezaOrganizacao = e.Organizacao.DscrNaturezaOrganizacao
                }
                : null,

            Turno = e.Turno != null
                ? new Turno
                {
                    CodTurnoAtendimento = e.Turno.CodTurnoAtendimento,
                    DscrTurnoAtendimento = e.Turno.DscrTurnoAtendimento
                }
                : null,

            Servico = e.Servico != null
                ? new Servico
                {
                    CodCnes = e.Servico.CodCnes,
                    FazAtendimentoAmbulatorialSus = e.Servico.StFazAtendimentoAmbulatorialSus,
                    TemCentroCirurgico = e.Servico.StCentroCirurgico,
                    TemCentroObstetrico = e.Servico.StCentroObstetrico,
                    TemCentroNeonatal = e.Servico.StCentroNeonatal,
                    FazAtendimentoHospitalar = e.Servico.StAtendimentoHospitalar,
                    TemServicoApoio = e.Servico.StServicoApoio,
                    FazAtendimentoAmbulatorial = e.Servico.StAtendimentoAmbulatorial
                }
                : null
        }).ToList();

        return new PaginatedResult<Estabelecimento>(
            estabelecimentosDto,
            pagedResultFromDb.CurrentPage,
            pagedResultFromDb.PageSize,
            pagedResultFromDb.TotalCount
        );
    }
}