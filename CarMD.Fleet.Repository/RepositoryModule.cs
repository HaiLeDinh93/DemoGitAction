using System;
using Autofac;
using Environment = CarMD.Fleet.Common.Environment;
using CarMD.Fleet.Repository.Repository;
using CarMD.Fleet.Repository.IRepository;

namespace CarMD.Fleet.Repository
{
    public class RepositoryModule : Module
    {
        private readonly Environment _environment;

        public RepositoryModule(Environment environment)
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
            builder.RegisterType<UserRepository>().As<IUserRepository>().InstancePerLifetimeScope();
            builder.RegisterType<KioskRepository>().As<IKioskRepository>().InstancePerLifetimeScope();
            builder.RegisterType<LoggingErrorRepository>().As<ILoggingErrorRepository>().InstancePerLifetimeScope();
            builder.RegisterType<LoggingTimeRepository>().As<ILoggingTimeRepository>().InstancePerLifetimeScope();
            builder.RegisterType<FeedBackRepository>().As<IFeedBackRepository>().InstancePerLifetimeScope();
            builder.RegisterType<VimeoSettingRepository>().As<IVimeoSettingRepository>().InstancePerLifetimeScope();
            builder.RegisterType<DiagnosticReportRepository>().As<IDiagnosticReportRepository>().InstancePerLifetimeScope();
            builder.RegisterType<ConsumerRepository>().As<IConsumerRepository>().InstancePerLifetimeScope();
        }

        private void RegisterForWeb(ContainerBuilder builder)
        {
            throw new NotImplementedException();
        }

        private void RegisterForStandalone(ContainerBuilder builder)
        {
            throw new NotImplementedException();
        }

        private void RegisterForWcf(ContainerBuilder builder)
        {
            throw new NotImplementedException();
        }
    }
}