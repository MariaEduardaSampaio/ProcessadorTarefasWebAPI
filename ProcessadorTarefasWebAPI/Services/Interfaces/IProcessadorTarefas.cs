using ProcessadorTarefasWebAPI.Entities;

namespace ProcessadorTarefasWebAPI.Services.Interfaces
{
    public interface IProcessadorTarefas
    {
        Task ProcessarTarefas();
        Task IniciarTarefa(Tarefa tarefa);
        Task ImprimirTarefas();
        void ImprimirTarefa(Tarefa tarefa);
        void CancelarTarefasEmExecucao();
        Task Encerrar();
        Task AgendarTarefas();
    }
}
