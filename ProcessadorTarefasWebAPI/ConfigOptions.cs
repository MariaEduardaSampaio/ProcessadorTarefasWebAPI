using ProcessadorTarefasWebAPI.ConfigValidations;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace ProcessadorTarefasWebAPI
{
    public class ConfigOptions
    {
        [Required(ErrorMessage = "O campo 'TempoMinimoExecucaoSubtarefa' é obrigatório.")]
        [DisallowNull]
        [MinLength(1, ErrorMessage = "O campo deve ser no mínimo 1.")]
        public int TempoMinimoExecucaoSubtarefa { get; set; }




        [Required(ErrorMessage = "O campo 'TempoMaximoExecucaoSubtarefa' é obrigatório.")]
        [DisallowNull]
        [MinLength(1, ErrorMessage = "O campo deve ser no mínimo 1.")]
        [MinLowerThanMax("TempoMinimoExecucaoSubtarefa", "TempoMaximoExecucaoSubtarefa")]
        public int TempoMaximoExecucaoSubtarefa { get; set; }




        [Required(ErrorMessage = "O campo 'TarefasExecutadasEmParalelo' é obrigatório.")]
        [DisallowNull]
        [MinLength(1, ErrorMessage = "O campo deve ser no mínimo 1.")]
        public int TarefasExecutadasEmParalelo { get; set; }



        [Required(ErrorMessage = "O campo 'QuantidadeMinimaSubtarefas' é obrigatório.")]
        [DisallowNull]
        [MinLength(1, ErrorMessage = "O campo deve ser no mínimo 1.")]
        public int QuantidadeMinimaSubtarefas { get; set; }



        [Required(ErrorMessage = "O campo 'QuantidadeMaximaSubtarefas' é obrigatório.")]
        [DisallowNull]
        [MinLength(1, ErrorMessage = "O campo deve ser no mínimo 1.")]
        [MinLowerThanMax("QuantidadeMinimaSubtarefas", "QuantidadeMaximaSubtarefas")]
        public int QuantidadeMaximaSubtarefas { get; set; }



        [Required(ErrorMessage = "O campo 'Name' é obrigatório.")]
        [DisallowNull]
        [MinLength(1, ErrorMessage = "O campo deve ser no mínimo 1.")]
        public int QuantidadeTarefasEstaticas { get; set; }
    }
}
