using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace SupportSync
{
    public class MvcApplication : System.Web.HttpApplication
    {
        public static NextPage.SupportSync.Zapier zap = new NextPage.SupportSync.Zapier();
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            zap.LoadCustomers();
        }
    }
}
