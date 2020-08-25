using Autofac;
using CarMD.Fleet.Adapter;
using CarMD.Fleet.Common;
using CarMD.Fleet.Core.Utility;
using CarMD.Fleet.Repository;
using CarMD.Fleet.Repository.EntityFramework;
using CarMD.Fleet.Service;
using log4net;
using log4net.Config;
using log4net.Repository;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.IO;
using System.Reflection;

namespace CarMD.Shell.Api
{
    public class Startup
    {
        public Startup(IWebHostEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            this.Configuration = builder.Build();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            AppSettingUtility.Settings = Configuration.GetSection("AppSettings");
            services.AddHttpContextAccessor();

            services.AddCors(options =>
                {
                    options.AddPolicy("AllowSpecificOrigins",
                    builder =>
                    {
                        builder.AllowAnyOrigin()
                               .AllowAnyMethod()
                               .AllowAnyHeader(); ;
                    });
                });

            services.AddControllers().AddNewtonsoftJson();
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterModule(new ServiceModule(Environment.WebApi));
            builder.RegisterModule(new RepositoryModule(Environment.WebApi));
            builder.RegisterModule(new AdapterModule(Environment.WebApi));

            var connectionString = Configuration.GetConnectionString("MbkDbConstr");
            // Register Entity Framework
            var dbContextOptionsBuilder = new DbContextOptionsBuilder<CarMDShellContext>().UseSqlServer(connectionString).UseLazyLoadingProxies(false); ;

            builder.RegisterType<CarMDShellContext>()
                .WithParameter("options", dbContextOptionsBuilder.Options)
                .InstancePerLifetimeScope();

            RegisterLogChain(builder);
        }

        private void RegisterLogChain(ContainerBuilder builder)
        {
            var log4netConfig = new System.Xml.XmlDocument();
            log4netConfig.Load(File.OpenRead(@"log4net.config"));
            var repo = LogManager.CreateRepository(Assembly.GetEntryAssembly(), typeof(log4net.Repository.Hierarchy.Hierarchy));
            XmlConfigurator.Configure(repo, log4netConfig["log4net"]);

            ILoggerRepository repository = LogManager.GetRepository(Assembly.GetEntryAssembly());
            XmlConfigurator.Configure(repository, new FileInfo("log4net.config"));

            //builder.RegisterType
            builder.Register(c => LogManager.GetLogger(typeof(object))).As<ILog>().InstancePerLifetimeScope();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            var pathBase = Configuration.GetValue<string>("PathBase");
            if (!string.IsNullOrEmpty(pathBase))
            {
                app.UsePathBase(pathBase);
            }

            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseCors("AllowSpecificOrigins");

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
