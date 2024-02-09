namespace ProcessadorTarefasWebAPI.Entities.Interfaces
{
    public enum EstadoTarefa
    {
        Criada = 0,
        Agendada,
        EmExecucao,
        EmPausa,
        Cancelada,
        Concluida
    }
}