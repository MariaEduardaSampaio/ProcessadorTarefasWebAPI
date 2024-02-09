using Microsoft.AspNetCore.Mvc;
using ProcessadorTarefasWebAPI.Services.Interfaces;

namespace ProcessadorTarefasWebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProcessadorTarefasController : Controller
    {
        private readonly IGerenciadorTarefas _gerenciadorTarefas;
        private readonly IProcessadorTarefas _processadorTarefas;
        public ProcessadorTarefasController(IGerenciadorTarefas gerenciadorTarefas, IProcessadorTarefas processadorTarefas)
        {
            _gerenciadorTarefas = gerenciadorTarefas;
            _processadorTarefas = processadorTarefas;
        }

        [HttpPost("AdicionarTarefa", Name = "Adiciona tarefa à lista de tarefas")]
        public async Task<IActionResult> CriarTarefa()
        {
            try
            {
                var tarefa = await _gerenciadorTarefas.CriarTarefa();

                return Ok(tarefa);
            }
            catch (Exception ex)
            {
                return BadRequest($"Erro ao criar tarefa: {ex.Message}");
            }
        }



        [HttpGet("CancelarTarefaCriadaOuAgendada/{id}", Name = "Cancelar tarefa criada ou agendada")]
        public async Task<IActionResult> CancelarTarefaCriadaOuAgendada(int id)
        {
            try
            {
                await _gerenciadorTarefas.Cancelar(id);
                return Ok("Tarefa cancelada com sucesso!");
            }
            catch (Exception ex)
            {
                return BadRequest($"Erro ao cancelar tarefa: {ex.Message}");
            }
        }



        [HttpGet("CancelarTarefasEmExecucao/{id}", Name = "Cancelar tarefa em execução")]
        public async Task<IActionResult> CancelarTarefasEmExecucao(int id)
        {
            try
            {
                await _processadorTarefas.CancelarTarefa(id);
                return Ok("Tarefa em execução cancelada com sucesso.");
            }
            catch (Exception ex)
            {
                return BadRequest($"Erro ao cancelar tarefa: {ex.Message}");
            }
        }



        [HttpGet("ListarTarefasAtivas", Name = "Listar tarefas ativas")]
        public async Task<IActionResult> ListarTarefasAtivas()
        {
            try
            {
                var tarefas = await _gerenciadorTarefas.ListarAtivas();
                return Ok(tarefas);
            }
            catch (Exception ex)
            {
                return BadRequest($"Erro ao listar tarefas ativas: {ex.Message}");
            }
        }



        [HttpGet("ListarTarefasInativas", Name = "Listar tarefas inativas")]
        public async Task<IActionResult> ListarTarefasInativas()
        {
            try
            {
                var tarefas = await _gerenciadorTarefas.ListarInativas();
                return Ok(tarefas);
            }
            catch (Exception ex)
            {
                return BadRequest($"Erro ao listar tarefas inativas: {ex.Message}");
            }
        }



        [HttpGet("ExecutarTarefas", Name = "Executar tarefas em paralelo de forma assíncrona")]
        public async Task<IActionResult> ExecutarTarefas()
        {
            try
            {
                await _processadorTarefas.ProcessarTarefas();
                return Ok("Tarefas processadas com sucesso!");

            } catch(Exception ex)
            {

                return BadRequest($"Ocorreu um erro ao processar as tarefas: {ex.Message}");
            }
        }


        [HttpGet("EncerrarExecucao", Name = "Encerrar execução de tarefas.")]
        public async Task<IActionResult> EncerrarExecucao()
        {
            try
            {
                await _processadorTarefas.Encerrar();
                return Ok("Processo encerrado com sucesso!");
            }
            catch (Exception ex)
            {
                return BadRequest($"Ocorreu um erro ao encerrar a execução: {ex.Message}");
            }
        }
    }
}
