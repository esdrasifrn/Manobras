using Autofac;
using Autofac.Extensions.DependencyInjection;
using Manobra.Infra.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using SDK.DependencyInjection;
using SDK.DependencyInjection.AutoFac;
using SDK.DependencyInjection.Interfaces;
using System;
using System.IO;
using System.Reflection;
using System.Text.Json;

namespace Manobra.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public ILifetimeScope AutofacContainer { get; private set; }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            #region Conection

            services.AddEntityFrameworkSqlServer()
                   .AddDbContext<ManobraContext>(
                       options => options.UseSqlServer(
                           Configuration.GetConnectionString("ManobraConnectionString")));

            #endregion    

            services.AddSwaggerGen(opt =>
            {
                opt.SwaggerDoc("v1", new OpenApiInfo { Title = "ManobraAPI", Version = "v1" });

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                opt.IncludeXmlComments(xmlPath);
            });

            services.AddControllers();
        }

        public void ConfigureContainer(ContainerBuilder builder) // chamado pelo autofac
        {
            var sdkContainerBuilder = new AutofacContainerBuilder(builder);

            sdkContainerBuilder.RegisterScoped<ISdkContainer>(c => new AutofacContainer(this.AutofacContainer));

            Application.DependencyInjection.RegisterDependencies(sdkContainerBuilder);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            this.AutofacContainer = app.ApplicationServices.GetAutofacRoot();
            SdkDI.SetGlobalResolver(this.AutofacContainer.Resolve<ISdkContainer>());

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            app.UseSwaggerUI(c => {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Manobra V1");
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
