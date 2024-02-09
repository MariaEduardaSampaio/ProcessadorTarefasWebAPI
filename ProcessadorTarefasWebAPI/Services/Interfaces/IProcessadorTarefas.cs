using ProcessadorTarefasWebAPI.Entities;

namespace ProcessadorTarefasWebAPI.Services.Interfaces
{
    public interface IProcessadorTarefas
    {
        Task ProcessarTarefas();
        Task IniciarTarefa(Tarefa tarefa);
        Task CancelarTarefa(int idTarefa);
        Task Encerrar();
    }
}
