
using Microsoft.AspNetCore.Mvc;
using ProcessadorTarefasWebAPI.Entities.Interfaces;
using ProcessadorTarefasWebAPI.Entities;
using ProcessadorTarefasWebAPI.Services;
using ProcessadorTarefasWebAPI.Services.Interfaces;
using ProcessadorTarefasWebAPI.Repository;
using Microsoft.Extensions.Configuration;

namespace ProcessadorTarefasWebAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();
            builder.Services.AddSingleton<IRepository<Tarefa>, ProcessadorTarefasRepository>();
            builder.Services.Configure<ConfigOptions>(builder.Configuration.GetSection("options"));

            //builder.Services.AddOptions<ConfigOptions>()
            //    .Bind(builder.Configuration.GetSection(ConfigOptions.options)).ValidateDataAnnotations();
            builder.Services.AddScoped<IGerenciadorTarefas, GerenciadorTarefas>();
            builder.Services.AddScoped<IProcessadorTarefas, ProcessadorTarefas>();
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
