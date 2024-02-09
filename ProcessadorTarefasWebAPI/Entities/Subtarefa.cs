using ProcessadorTarefasWebAPI.Entities.Interfaces;

namespace ProcessadorTarefasWebAPI.Entities
{
    public class Subtarefa : ISubtarefa
    {
        public TimeSpan Duracao { get; set; }
        public Subtarefa(TimeSpan duracao)
        {
            Duracao = duracao;
        }
    }
}
