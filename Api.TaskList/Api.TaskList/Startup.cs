using Api.TaskList.Repositories;
using Api.TaskList.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;

namespace Api.TaskList
{
    public class Startup
    {
        public IHostingEnvironment _hostingEnvironment { get; private set; }
        public IConfiguration _configuration { get; private set; }

        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
            _hostingEnvironment = env;
            _configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            // Configurando o serviço de documentação do Swagger
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1",
                new Info
                {
                    Title = "TaskList",
                    Version = "v1",
                    Description = "Cadastro de Tarefas.",
                    Contact = new Contact
                    {
                        Name = "Gabriel Borchardt",
                        Url = "https://github.com/gabrielborchardt"
                    }
                });
            });

            services.AddTransient<ITaskService, TaskService>();
            services.AddTransient<ITaskRepository, TaskRepository>();
            services.AddSingleton<IConfiguration>(_configuration);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseCors(builder => builder
           .AllowAnyOrigin()
           .AllowAnyMethod()
           .AllowAnyHeader()
           .AllowCredentials());

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "TaskList"); // IIS express local
            });

            app.UseMvc();
        }
    }
}
