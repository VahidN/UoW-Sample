using System;
using System.Threading;
using System.Web;
using EF_Sample07.DataLayer.Context;
using EF_Sample07.ServiceLayer;
using EF_Sample07.ServiceLayer.Contracts;
using StructureMap;
using StructureMap.Web;
using StructureMap.Web.Pipeline;

namespace EF_Sample07.IoCConfig
{
    public static class SmObjectFactory
    {
        private static readonly Lazy<Container> _containerBuilder =
            new Lazy<Container>(defaultContainer, LazyThreadSafetyMode.ExecutionAndPublication);

        public static IContainer Container
        {
            get { return _containerBuilder.Value; }
        }

        public static void HttpContextDisposeAndClearAll()
        {
            HttpContextLifecycle.DisposeAndClearAll();
        }

        private static Container defaultContainer()
        {
            return new Container(ioc =>
            {
                // session manager setup
                ioc.For<ISessionProvider>().Use<DefaultWebSessionProvider>();
                ioc.For<HttpSessionStateBase>()
                   .Use(ctx => new HttpSessionStateWrapper(HttpContext.Current.Session));

                ioc.For<IUnitOfWork>()
                   .HybridHttpOrThreadLocalScoped()
                   .Use<Sample07Context>()
                    // Remove these 2 lines if you want to use a connection string named Sample07Context, defined in the web.config file.
                   .Ctor<string>("connectionString")
                   .Is(ctx => getCurrentConnectionString(ctx));

                ioc.For<ICategoryService>().Use<EfCategoryService>();
                ioc.For<IProductService>().Use<EfProductService>();

                ioc.Policies.SetAllProperties(properties =>
                {
                    properties.OfType<IUnitOfWork>();
                    properties.OfType<ICategoryService>();
                    properties.OfType<IProductService>();
                    properties.OfType<ISessionProvider>();
                });
            });
        }

        private static string getCurrentConnectionString(IContext ctx)
        {
            if (HttpContext.Current != null)
            {
                // this is a web application
                var sessionProvider = ctx.GetInstance<ISessionProvider>();
                var connectionString = sessionProvider.Get<string>("CurrentConnectionString");
                if (string.IsNullOrWhiteSpace(connectionString))
                {
                    // It's a default connectionString.
                    connectionString = "Database2012";
                    // this session value should be set during the login phase
                    sessionProvider.Store("CurrentConnectionStringName", connectionString);
                }

                return connectionString;
            }
            else
            {
                // this is a desktop application, so you can store this value in a global static variable.
                return "Database2012";
            }
        }
    }
}