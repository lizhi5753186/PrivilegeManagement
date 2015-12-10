using System.Reflection;
using System.Web.Compilation;
using System.Web.Http;
using System.Web.Http.Dispatcher;
using System.Web.Mvc;
using Autofac;
using Autofac.Integration.Mvc;
using Autofac.Integration.WebApi;
using LH.Application.ServiceImp;
using LH.Domain.Repositories;
using LH.Repository;
using LH.Web.API.DI;

namespace LH.Web.API
{
    public static class Bootstrapper
    {
        public static void InitAutofac()
        {
            var builder = new ContainerBuilder();

            builder.RegisterControllers(Assembly.GetExecutingAssembly());
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());

            // Repository注册
            // builder.RegisterModule(new ConfigurationSettingsReader("autofac"));
            builder.RegisterAssemblyTypes(typeof(BaseRepository<>).Assembly)
                .Where(type => type.Name.EndsWith("Repository"))
              .AsImplementedInterfaces();
            builder.RegisterAssemblyTypes(typeof(BaseService).Assembly)
                .Where(type => type.Name.EndsWith("Service"))
                .AsImplementedInterfaces();
           
            // Set the dependency resolver to be Autofac.
            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));

            GlobalConfiguration.Configuration.Services.Replace(typeof(IHttpControllerActivator), new LHControllerActivator(container));
        }
    }
}