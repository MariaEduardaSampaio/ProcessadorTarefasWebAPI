using ProcessadorTarefasWebAPI.Entities;
using ProcessadorTarefasWebAPI.Entities.Interfaces;
using System.Xml.Linq;

namespace ProcessadorTarefasWebAPI.Repository
{
    public class ProcessadorTarefasRepository : IRepository<Tarefa>
    {
        private List<Tarefa> Tarefas { get; set; }
        public ProcessadorTarefasRepository()
        {
            Tarefas = new();
        }

        public void Add(Tarefa entity)
        {
            Tarefas.Add(entity);
        }

        public void Delete(int id)
        {
            var tarefa = GetById(id);
            if (tarefa != null)
                Tarefas.Remove(tarefa);
            else
                throw new ArgumentException("Não existe tarefa com este ID.");
        }

        public IEnumerable<Tarefa> GetAll()
        {
            return Tarefas;
        }

        public IEnumerable<Tarefa> GetByStatus(EstadoTarefa status)
        {
            return Tarefas.Where(t => t.Estado == status);
        }

        public Tarefa? GetById(int id)
        {
            return Tarefas.FirstOrDefault(t => t.Id == id);
        }

        public void Update(Tarefa tarefaAtualizada)
        {
            var tarefaAntiga = Tarefas.FirstOrDefault(t => t.Id == tarefaAtualizada.Id);
            Tarefas.Remove(tarefaAntiga!);
            Tarefas.Add(tarefaAtualizada);
            Tarefas = Tarefas.OrderBy(t => t.Id).ToList();
        }
    }
}
