using System.Reflection;
using System.Web.Mvc;
using Autofac;
using Autofac.Configuration;
using Autofac.Integration.Mvc;
using LH.Domain.Repositories;

namespace LH.Web.API
{
    public static class AutofacExt
    {
        public static void InitAutofac()
        {
            var builder = new ContainerBuilder();

            builder.RegisterControllers(Assembly.GetExecutingAssembly());

            // Repository注册
            // builder.RegisterModule(new ConfigurationSettingsReader("autofac"));

            builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly())
              .Where(type => type.Name.EndsWith("Repository"))
              .AsImplementedInterfaces();
            builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly())
                .Where(type => type.Name.EndsWith("Service"))
                .AsImplementedInterfaces();
           
            // Set the dependency resolver to be Autofac.
            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }
    }
}