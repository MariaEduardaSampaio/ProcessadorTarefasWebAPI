using ProcessadorTarefasWebAPI.Entities.Interfaces;
using ProcessadorTarefasWebAPI.Entities;
using ProcessadorTarefasWebAPI.Services.Interfaces;
using Microsoft.Extensions.Options;

namespace ProcessadorTarefasWebAPI.Services
{
    internal class GerenciadorTarefas : IGerenciadorTarefas
    {
        private static int ContadorId = 0;
        private readonly IRepository<Tarefa> _repository;
        private readonly int _quantidadeTarefasEstaticas;
        private readonly int _quantidadeMinimaSubtarefas;
        private readonly int _quantidadeMaximaSubtarefas;
        private readonly int _tempoMinimoExecucaoSubtarefa;
        private readonly int _tempoMaximoExecucaoSubtarefa;

        public GerenciadorTarefas(IRepository<Tarefa> repository, IOptionsSnapshot<ConfigOptions> configOptions)
        {
            _repository = repository;
            _quantidadeTarefasEstaticas = configOptions.Value.QuantidadeTarefasEstaticas;
            _quantidadeMinimaSubtarefas = configOptions.Value.QuantidadeMinimaSubtarefas;
            _quantidadeMaximaSubtarefas = configOptions.Value.QuantidadeMaximaSubtarefas;
            _tempoMinimoExecucaoSubtarefa = configOptions.Value.TempoMinimoExecucaoSubtarefa;
            _tempoMaximoExecucaoSubtarefa = configOptions.Value.TempoMaximoExecucaoSubtarefa;

            if (!_repository.GetAll().Any())
            {
                for (int i = 0; i < _quantidadeTarefasEstaticas; i++)
                    CriarTarefa();
            }
        }

        public Task<Tarefa> CriarTarefa()
        {
            var tarefa = new Tarefa()
            {
                Id = ContadorId++,
                Estado = (int)EstadoTarefa.Criada,
                IniciadaEm = default,
                EncerradaEm = default,
                SubtarefasExecutadas = new List<Subtarefa>(),
                SubtarefasPendentes = CriarSubtarefas(),
            };

            _repository.Add(tarefa);

            return Task.FromResult(tarefa);
        }

        public Task Cancelar(int idTarefa)
        {
            var tarefa = _repository.GetById(idTarefa);
            if (tarefa!.Estado == EstadoTarefa.Criada ||
                tarefa.Estado == EstadoTarefa.Agendada)
            {
                Tarefa tarefaCancelada = new()
                {
                    Id = idTarefa,
                    IniciadaEm = default,
                    EncerradaEm = DateTime.Now,
                    Estado = EstadoTarefa.Cancelada,
                    SubtarefasExecutadas = tarefa.SubtarefasExecutadas,
                    SubtarefasPendentes = tarefa.SubtarefasPendentes
                };

                _repository.Update(tarefaCancelada);

                return Task.FromResult(tarefaCancelada);
            }
            else
            {
                throw new ArgumentException("Não é possível cancelar uma tarefa que não tenha sido criada ou agendada.");
            }
        }

        public Task<Tarefa> Consultar(int idTarefa)
        {
            var taskModel = _repository.GetById(idTarefa);
            return Task.FromResult(taskModel)!;
        }

        public IEnumerable<Subtarefa> CriarSubtarefas()
        {
            List<Subtarefa> subtasks = new();
            
            int quantidadeTarefas = new Random().Next(_quantidadeMinimaSubtarefas, _quantidadeMaximaSubtarefas);

            for (int i = 0; i < quantidadeTarefas; i++)
            {
                int duracao = new Random().Next(_tempoMinimoExecucaoSubtarefa, _tempoMaximoExecucaoSubtarefa);
                Subtarefa novaSubtask = new Subtarefa(TimeSpan.FromSeconds(duracao));
                subtasks.Add(novaSubtask);
            }

            return subtasks.AsEnumerable();
        }

        public Task<IEnumerable<Tarefa>> ListarAtivas()
        {
            var tasks = _repository.GetAll().Where(t => t.Estado != EstadoTarefa.Cancelada && t.Estado != EstadoTarefa.Concluida);
            return Task.FromResult(tasks);
        }

        public Task<IEnumerable<Tarefa>> ListarInativas()
        {
            var tasks = _repository.GetAll().Where(t => t.Estado == EstadoTarefa.Cancelada || t.Estado == EstadoTarefa.Concluida);
            return Task.FromResult(tasks);
        }
    }
}
