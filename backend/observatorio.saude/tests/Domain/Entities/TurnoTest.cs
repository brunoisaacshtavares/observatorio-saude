using Xunit;
using FluentAssertions;
using System.ComponentModel.DataAnnotations;
using observatorio.saude.Domain.Entities;

namespace observatorio.saude.tests.Domain.Entities;

public class TurnoTest
{
    private Turno CriarEntidadeValida()
    {
        return new Turno
        {
            CodTurnoAtendimento = 1,
            DscrTurnoAtendimento = "MANHA"
        };
    }
    
    private (bool IsValid, ICollection<ValidationResult> Results) ValidarModelo(Turno entidade)
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
    public void CodTurnoAtendimento_QuandoForDefault_DeveSerInvalido()
    {
        var entidade = CriarEntidadeValida();
        entidade.CodTurnoAtendimento = 0;
        
        var (isValid, results) = ValidarModelo(entidade);
        
        isValid.Should().BeFalse();
        results.Should().HaveCount(1);
        results.First().MemberNames.Should().Contain(nameof(Turno.CodTurnoAtendimento));
    }
}