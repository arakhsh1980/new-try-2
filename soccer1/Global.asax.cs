using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;
using System.Web.SessionState;
using soccer1.App_Start;
using System.Data.Entity;
using soccer1.Models.main_blocks;


namespace soccer1
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            StartingServer.StartAll();
            /*
            Database.SetInitializer<playerInfoDBContext>(new DropCreateDatabaseIfModelChanges<playerInfoDBContext>());
            Database.SetInitializer<Log3DBContext>(new DropCreateDatabaseIfModelChanges<Log3DBContext>());
            Database.SetInitializer<PawnInfoDBContext>(new DropCreateDatabaseIfModelChanges<PawnInfoDBContext>());
            Database.SetInitializer<ElixirInfoDBContext>(new DropCreateDatabaseIfModelChanges<ElixirInfoDBContext>());
            Database.SetInitializer<FormationInfoDBContext>(new DropCreateDatabaseIfModelChanges<FormationInfoDBContext>());
            */
            //AppDomain.CurrentDomain.SetData("DataDirectory", AppDomain.CurrentDomain.);
        }
    }
}
