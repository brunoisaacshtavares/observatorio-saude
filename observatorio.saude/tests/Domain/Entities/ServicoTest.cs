using Xunit;
using FluentAssertions;
using System.ComponentModel.DataAnnotations;
using observatorio.saude.Domain.Entities;

namespace observatorio.saude.tests.Domain.Entities;

public class ServicoTests
{
    private Servico CriarEntidadeValida()
    {
        return new Servico
        {
            CodCnes = 1234567,
            TemCentroCirurgico = true,
            FazAtendimentoAmbulatorialSus = true,
            TemCentroObstetrico = false
        };
    }
    
    private (bool IsValid, ICollection<ValidationResult> Results) ValidarModelo(Servico entidade)
    {
        var validationResults = new List<ValidationResult>();
        var context = new ValidationContext(entidade, serviceProvider: null, items: null);
        var isValid = Validator.TryValidateObject(entidade, context, validationResults, validateAllProperties: true);
        return (isValid, validationResults);
    }

    [Fact]
    public void Entidade_ComDadosValidos_DeveSerConsideradaValida()
    {
        var entidade = CriarEntidadeValida();
        
        var (isValid, results) = ValidarModelo(entidade);
        
        isValid.Should().BeTrue();
        results.Should().BeEmpty();
    }

    [Fact]
    public void CodCnes_QuandoForDefault_DeveSerInvalido()
    {
        var entidade = CriarEntidadeValida();
        entidade.CodCnes = 0;
        
        var (isValid, results) = ValidarModelo(entidade);
        
        isValid.Should().BeFalse();
        results.Should().HaveCount(1);
        results.First().MemberNames.Should().Contain(nameof(Servico.CodCnes));
    }
}