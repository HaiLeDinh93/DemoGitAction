using Autofac;
using CarMD.Fleet.Adapter.Metafuse;
using System;
using Environment = CarMD.Fleet.Common.Environment;

namespace CarMD.Fleet.Adapter
{
    public class AdapterModule : Module
    {
        private readonly Environment _environment;

        public AdapterModule(Environment environment)
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
            // register adapters here
            builder.RegisterType<MetafuseAdapter>().As<IMetafuseAdapter>().InstancePerLifetimeScope();
            builder.RegisterType<MetafuseServiceInvoker>().As<IWebServiceInvoker>().InstancePerLifetimeScope();
        }

        private void RegisterForWcf(ContainerBuilder builder)
        {
            throw new NotImplementedException();
        }

        private void RegisterForWeb(ContainerBuilder builder)
        {
            // register adapters here
            builder.RegisterType<MetafuseAdapter>().As<IMetafuseAdapter>().InstancePerLifetimeScope();
            builder.RegisterType<MetafuseServiceInvoker>().As<IWebServiceInvoker>().InstancePerLifetimeScope();
        }

        private void RegisterForStandalone(ContainerBuilder builder)
        {
            throw new NotImplementedException();
        }
    }
}
