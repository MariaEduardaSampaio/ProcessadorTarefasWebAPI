using ProcessadorTarefasWebAPI.Entities.Interfaces;
using ProcessadorTarefasWebAPI.Entities;
using ProcessadorTarefasWebAPI.Services.Interfaces;
using System.Diagnostics;
using Microsoft.Extensions.Options;

namespace ProcessadorTarefasWebAPI.Services
{
    public class ProcessadorTarefas : IProcessadorTarefas
    {
        private readonly IRepository<Tarefa> _repository;
        private readonly int _tarefasExecutadasEmParalelo;

        public ProcessadorTarefas(IRepository<Tarefa> repository, IOptionsSnapshot<ConfigOptions> configOptions)
        {
            _tarefasExecutadasEmParalelo = configOptions.Value.TarefasExecutadasEmParalelo;
            _repository = repository;
        }

        public Task CancelarTarefa(int id)
        {
            var tarefaEmExecucao = _repository.GetById(id);

            Console.WriteLine($"Tarefa {tarefaEmExecucao.Id} cancelada.");
            tarefaEmExecucao.Estado = EstadoTarefa.Cancelada;
            _repository.Update(tarefaEmExecucao);

            return Task.CompletedTask;
        }

        public Task Encerrar()
        {
            var tarefasEmExecucao = _repository.GetByStatus(EstadoTarefa.EmExecucao);

            foreach (var item in tarefasEmExecucao)
            {
                Console.WriteLine($"Tarefa {item.Id} em pausa.");
                item.Estado = EstadoTarefa.EmPausa;
                _repository.Update(item);
            }

            return Task.CompletedTask;
        }

        public void AgendarTarefas()
        {
            var tarefas = _repository.GetAll().ToList();
            int quantidadeTarefasAgendadas = _repository.GetByStatus(EstadoTarefa.Agendada).Count();

            foreach (var tarefa in tarefas)
            {
                quantidadeTarefasAgendadas = _repository.GetByStatus(EstadoTarefa.Agendada).Count();
                if ((tarefa.Estado.Equals(EstadoTarefa.Criada) && quantidadeTarefasAgendadas < _tarefasExecutadasEmParalelo)
                    || tarefas.All(tarefa => tarefa.Estado.Equals(EstadoTarefa.Agendada)))
                {
                    tarefa.Estado = EstadoTarefa.Agendada;
                    _repository.Update(tarefa);
                    ImprimirTarefa(tarefa);
                }
            }
        }

        public async Task ProcessarTarefas()
        {
            while (true)
            {
                AgendarTarefas();
                IEnumerable<Tarefa> tarefas = _repository.GetByStatus(EstadoTarefa.Agendada).Concat(_repository.GetByStatus(EstadoTarefa.EmPausa));
                Queue<Tarefa> tarefasParaProcessar = new Queue<Tarefa>(tarefas);

                if (tarefasParaProcessar.Count == 0)
                    throw new Exception("Não existem mais tarefas que podem ser processadas.");

                var tasksEmExecucao = new List<Task>();

                while (tasksEmExecucao.Count < _tarefasExecutadasEmParalelo && tarefasParaProcessar.Count > 0)
                {
                    Tarefa tarefa = tarefasParaProcessar.Dequeue();
                    tasksEmExecucao.Add(IniciarTarefa(tarefa));
                }

                await Task.WhenAll(tasksEmExecucao);
            }
        }

        public void ImprimirTarefa(Tarefa tarefa)
        {
            int progresso = 0;

            int totalSubtarefasExecutadas = tarefa.SubtarefasExecutadas!.Count();
            int totalSubtarefas = tarefa.SubtarefasPendentes!.Count() + tarefa.SubtarefasExecutadas!.Count();

            if (totalSubtarefas != 0)
            {
                progresso = (int)((totalSubtarefasExecutadas / (double)totalSubtarefas) * 100);
            }
            Console.WriteLine($"\n___________________________________________________________________________");
            Console.WriteLine($"Tarefa {tarefa.Id}\t - Estado: {tarefa.Estado}");
            Console.WriteLine($"Iniciada em: {tarefa.IniciadaEm}\t - Encerrada em: {tarefa.EncerradaEm}");
            Console.WriteLine($"Progresso: {progresso}%");
        }

        public async Task IniciarSubtarefa(Subtarefa subtarefa)
        {
            await Task.Delay(subtarefa.Duracao);
        }

        public async Task IniciarTarefa(Tarefa tarefa)
        {
            tarefa.Estado = EstadoTarefa.EmExecucao;
            tarefa.IniciadaEm = DateTime.Now;
            _repository.Update(tarefa);
            ImprimirTarefa(tarefa);

            int contadorSubtarefas = 0;
            var duracaoExecucaoTarefa = new Stopwatch();
            var duracaoExecucaoSubtarefa = new Stopwatch();
            duracaoExecucaoTarefa.Start();
            int totalSubtarefas = tarefa.SubtarefasPendentes!.Count();

            foreach (var subtarefa in tarefa.SubtarefasPendentes!)
            {
                contadorSubtarefas++;
                Console.WriteLine($"\n => Tarefa {tarefa.Id} => Subtarefa {contadorSubtarefas} iniciada.");

                duracaoExecucaoSubtarefa.Restart();
                await IniciarSubtarefa(subtarefa);
                duracaoExecucaoSubtarefa.Stop();

                var subtarefaExecutada = tarefa.SubtarefasPendentes.First(subtarefaPendente => subtarefaPendente.Equals(subtarefa));

                List<Subtarefa> subtarefasPendentes = tarefa.SubtarefasPendentes.ToList();
                subtarefasPendentes.Remove(subtarefaExecutada);
                tarefa.SubtarefasPendentes = subtarefasPendentes;

                List<Subtarefa> subtarefasExecutadas = tarefa.SubtarefasExecutadas!.ToList();
                subtarefasExecutadas.Add(subtarefaExecutada);
                tarefa.SubtarefasExecutadas = subtarefasExecutadas;
                ImprimirTarefa(tarefa);
                Console.WriteLine($"Subtarefa ({contadorSubtarefas} de {totalSubtarefas}) concluída em {duracaoExecucaoSubtarefa.Elapsed.TotalSeconds:F2} segundos.");
                Console.WriteLine($"___________________________________________________________________________\n");
            }

            duracaoExecucaoTarefa.Stop();
            tarefa.Estado = EstadoTarefa.Concluida;
            tarefa.EncerradaEm = DateTime.Now;
            _repository.Update(tarefa);
            ImprimirTarefa(tarefa);
            Console.WriteLine($"Concluída em {duracaoExecucaoTarefa.Elapsed.TotalSeconds:F2} segundos!");
            Console.WriteLine($"___________________________________________________________________________\n");
            await Task.Delay(200);
        }
    }
}
