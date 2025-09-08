using System.ComponentModel.DataAnnotations;

namespace observatorio.saude.Domain.Entities
{
    /// <summary>
    /// Representa informações organizacionais do estabelecimento de saúde, como tipo de unidade, gestão e natureza.
    /// </summary>
    public class Organizacao
    {
        /// <summary>
        /// Código CNES do estabelecimento.
        /// </summary>
        [Required]
        [Display(Name = "Código CNES", Description = "Código único do estabelecimento no Cadastro Nacional de Estabelecimentos de Saúde.")]
        public long CodCnes { get; set; }

        /// <summary>
        /// Tipo de unidade.
        /// </summary>
        [Display(Name = "Tipo de Unidade", Description = "Código representando o tipo da unidade de saúde.")]
        public long? TpUnidade { get; set; }

        /// <summary>
        /// Tipo de gestão.
        /// </summary>
        [Display(Name = "Tipo de Gestão", Description = "Código do tipo de gestão (Municipal, Estadual, Federal).")]
        public char? TpGestao { get; set; }

        /// <summary>
        /// Descrição da esfera administrativa.
        /// </summary>
        [Display(Name = "Esfera Administrativa", Description = "Esfera administrativa à qual a unidade pertence.")]
        public string? DscrEsferaAdministrativa { get; set; }

        /// <summary>
        /// Descrição do nível hierárquico.
        /// </summary>
        [Display(Name = "Nível Hierárquico", Description = "Nível hierárquico dentro da organização de saúde.")]
        public string? DscrNivelHierarquia { get; set; }

        /// <summary>
        /// Descrição da natureza da organização.
        /// </summary>
        [Display(Name = "Natureza da Organização", Description = "Tipo de natureza organizacional do estabelecimento.")]
        public string? DscrNaturezaOrganizacao { get; set; }
    }
}