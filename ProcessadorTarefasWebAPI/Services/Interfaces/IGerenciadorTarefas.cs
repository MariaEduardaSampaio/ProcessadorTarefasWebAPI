using ProcessadorTarefasWebAPI.Entities;

namespace ProcessadorTarefasWebAPI.Services.Interfaces
{
    public interface IGerenciadorTarefas
    {
        Task Cancelar(int idTarefa);
        Task<Tarefa> Consultar(int idTarefa);
        Task<Tarefa> CriarTarefa();
        IEnumerable<Subtarefa> CriarSubtarefas();
        Task<IEnumerable<Tarefa>> ListarAtivas();
        Task<IEnumerable<Tarefa>> ListarInativas();
    }
}
