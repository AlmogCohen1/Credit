using Entities.Contracts.BLL;
using Entities.Contracts.DAL;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace CreditServer
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages();
            AddProjectsToService(services , "DAL.dll", typeof(IDAL));
            AddProjectsToService(services, "BLL.dll", typeof(IBLL));

        }

        public void AddProjectsToService(IServiceCollection i_Service, string i_LibraryName, Type i_Type)
        {
            //typeof(Program).Assembly.GetTypes().Where(x => x.IsClass && !x.IsAbstract && x.GetInterfaces());
            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;

            //var g = Directory.GetFiles(Directory.GetCurrentDirectory(), "*.dll");
            //IEnumerable<Type>  types = Assembly.GetExecutingAssembly().GetTypes().Where(x => x.IsClass && !x.IsAbstract);
            var asm = Assembly.LoadFrom(Path.Combine(baseDirectory,i_LibraryName)).GetTypes();
            var typeList = asm.Where(x => x.IsClass && i_Type.IsAssignableFrom(x)).
                Select(t => new { Interface = t.GetInterfaces().FirstOrDefault(i => i.Name == ("I" + t.Name)), Type = t}).ToList();
            //addSingelton();
            //addTransient();
            //addScope();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
            });
        }
    }
}
