using Autofac;
using Autofac.Extensions.DependencyInjection;
using Manobra.Infra.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SDK.DependencyInjection;
using SDK.DependencyInjection.AutoFac;
using SDK.DependencyInjection.Interfaces;
using System;
using System.IO;
using System.Reflection;
using System.Text.Json;

namespace Manobra.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        public ILifetimeScope AutofacContainer { get; private set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            
            #region Conection

            services.AddEntityFrameworkSqlServer()
                   .AddDbContext<ManobraContext>(
                       options => options.UseSqlServer(
                           Configuration.GetConnectionString("ManobraConnectionString")));

            #endregion    

            services.AddControllersWithViews();
            services.AddMvc(options => options.EnableEndpointRouting = false);

            services.AddMemoryCache();
            services.AddSession();
            services.AddMvc();
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
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseSession();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Carro}/{action=Index}/{id?}");
            });             
    
        }
    }
}
