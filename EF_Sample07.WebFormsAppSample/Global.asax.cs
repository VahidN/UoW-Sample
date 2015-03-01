using System;
using System.Data.Entity;
using System.Web;
using EF_Sample07.DataLayer.Context;
using EF_Sample07.IoCConfig;

namespace EF_Sample07.WebFormsAppSample
{
    public class Global : HttpApplication
    {
        void Application_Start(object sender, EventArgs e)
        {
            initDatabases();
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

        void Application_EndRequest(object sender, EventArgs e)
        {
            SmObjectFactory.HttpContextDisposeAndClearAll();
        }

        void Application_End(object sender, EventArgs e)
        {
            //  Code that runs on application shutdown

        }

        void Application_Error(object sender, EventArgs e)
        {
            // Code that runs when an unhandled error occurs

        }

        void Session_Start(object sender, EventArgs e)
        {
            // Code that runs when a new session is started

        }

        void Session_End(object sender, EventArgs e)
        {
            // Code that runs when a session ends. 
            // Note: The Session_End event is raised only when the sessionstate mode
            // is set to InProc in the Web.config file. If session mode is set to StateServer 
            // or SQLServer, the event is not raised.

        }
    }
}
