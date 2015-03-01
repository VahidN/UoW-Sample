using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure.Interception;
using System.Globalization;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using EF_Sample07.DataLayer.Context;
using EF_Sample07.IoCConfig;
using ElmahEFLogger.CustomElmahLogger;

namespace EF_Sample07.MvcAppSample
{
    public class MvcApplication : HttpApplication
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Home", action = "Index", id = UrlParameter.Optional } // Parameter defaults
            );

        }

        protected void Application_Start()
        {
            DbInterception.Add(new ElmahEfInterceptor());
            initDatabases();

            AreaRegistration.RegisterAllAreas();

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);
            initStructureMap();
        }

        private static void initStructureMap()
        {
            //Set current Controller factory as StructureMapControllerFactory
            ControllerBuilder.Current.SetControllerFactory(new StructureMapControllerFactory());
        }

        protected void Application_EndRequest(object sender, EventArgs e)
        {
            SmObjectFactory.HttpContextDisposeAndClearAll();
        }


        private static void initDatabases()
        {
            // defined in web.config
            string[] connectionStringNames =
            {
                "Sample07Context",
                "Database2012"
            };

            foreach (var connectionStringName in connectionStringNames)
            {
                Database.SetInitializer(
                    new MigrateDatabaseToLatestVersion<Sample07Context, Configuration>(connectionStringName));

                using (var ctx = new Sample07Context(connectionStringName))
                {
                    ctx.Database.Initialize(force: true);
                }
            }
        }
    }

    public class StructureMapControllerFactory : DefaultControllerFactory
    {
        protected override IController GetControllerInstance(RequestContext requestContext, Type controllerType)
        {
            if (controllerType == null && requestContext.HttpContext.Request.Url != null)
                throw new InvalidOperationException(string.Format("Page not found: {0}",
                    requestContext.HttpContext.Request.Url.AbsoluteUri.ToString(CultureInfo.InvariantCulture)));
            return SmObjectFactory.Container.GetInstance(controllerType) as Controller;
        }
    }
}