using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using FluentValidation;
using FluentValidation.AspNetCore;
using APICotacoes.Models;
using APICotacoes.Validators;

namespace APICotacoes
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers()
                .AddFluentValidation();

            services.AddTransient<IValidator<CotacaoMoeda>, CotacaoMoedaValidator>();
            
            services.AddSwaggerGen(c => {

                c.SwaggerDoc("v1",
                    new OpenApiInfo
                    {
                        Title = "Cotações de Moedas Estrangeiras - Testes - Azure Service Bus",
                        Version = "v1",
                        Description = "Exemplo de API REST criada com o ASP.NET Core 3.1 para o envio de valores de moedas estrangeiras a uma Fila do Azure Service Bus",
                        Contact = new OpenApiContact
                        {
                            Name = "Renato Groffe",
                            Url = new Uri("https://github.com/renatogroffe")
                        }
                    });
            });

            services.AddApplicationInsightsTelemetry(Configuration);

            services.AddAzureAppConfiguration();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseAzureAppConfiguration();

            app.UseCors(builder => builder.AllowAnyMethod()
                                          .AllowAnyOrigin()
                                          .AllowAnyHeader());
                                          
            app.UseSwagger();
            app.UseSwaggerUI(c => {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Cotações - Testes - Azure Service Bus");
                c.RoutePrefix = string.Empty;
            });

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}