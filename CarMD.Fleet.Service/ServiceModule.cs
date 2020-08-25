using Autofac;
using Environment = CarMD.Fleet.Common.Environment;
using System;
using CarMD.Fleet.Service.Service;
using CarMD.Fleet.Service.IService;

namespace CarMD.Fleet.Service
{
    public class ServiceModule : Module
    {
        private readonly Environment _environment;

        public ServiceModule(Environment environment)
        {
            _environment = environment;
        }

        protected override void Load(ContainerBuilder builder)
        {
            switch (_environment)
            {
                case Environment.WebApi:
                    RegisterForWebApi(builder);
                    break;
                case Environment.Wcf:
                    RegisterForWcf(builder);
                    break;
                case Environment.Web:
                    RegisterForWeb(builder);
                    break;
                case Environment.Standalone:
                    RegisterForStandalone(builder);
                    break;
            }
        }

        private void RegisterForWebApi(ContainerBuilder builder)
        {
            builder.RegisterType<ApiService>().As<IApiService>().InstancePerLifetimeScope();
            builder.RegisterType<UserService>().As<IUserService>().InstancePerLifetimeScope();
            builder.RegisterType<KioskService>().As<IKioskService>().InstancePerLifetimeScope();
            builder.RegisterType<VehicleService>().As<IVehicleService>().InstancePerLifetimeScope();
            builder.RegisterType<VimeoSettingService>().As<IVimeoSettingService>().InstancePerLifetimeScope();
        }

        private void RegisterForWcf(ContainerBuilder builder)
        {
            throw new NotImplementedException();
        }

        private void RegisterForWeb(ContainerBuilder builder)
        {
            throw new NotImplementedException();
        }

        private void RegisterForStandalone(ContainerBuilder builder)
        {
            throw new NotImplementedException();
        }
    }
}

