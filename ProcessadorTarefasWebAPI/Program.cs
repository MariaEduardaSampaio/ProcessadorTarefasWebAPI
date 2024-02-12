
using ProcessadorTarefasWebAPI.Entities.Interfaces;
using ProcessadorTarefasWebAPI.Entities;
using ProcessadorTarefasWebAPI.Services.Interfaces;
using ProcessadorTarefasWebAPI.Repository;
using ProcessadorTarefasWebAPI.Services;
using Microsoft.Extensions.Configuration;

namespace ProcessadorTarefasWebAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();

            string basePath = AppDomain.CurrentDomain.BaseDirectory;
            string configurationPath = Path.Combine(basePath, "AppSettings.json").Replace("\\bin\\Debug\\net8.0", "");

            IConfiguration config = new ConfigurationBuilder()
                .AddJsonFile(configurationPath, optional: true, reloadOnChange: true)
                .Build();

            builder.Services.AddSingleton<IConfiguration>(_ => config);
            builder.Services.AddSingleton<IRepository<Tarefa>, ProcessadorTarefasRepository>();
            builder.Services.AddSingleton<IGerenciadorTarefas, GerenciadorTarefas>();
            builder.Services.AddSingleton<IProcessadorTarefas, ProcessadorTarefas>();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }

    }
}
