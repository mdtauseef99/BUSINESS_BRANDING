using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using BizzBranding.Filters;
using System.Web.SessionState;

namespace BizzBranding
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start(object sender,EventArgs e)
        {
            Application["NoOfVisitors"] = 0;
            AreaRegistration.RegisterAllAreas();
           
            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            AuthConfig.RegisterAuth();
        }

        void Session_Start(object sender, EventArgs e)
        {
            // Code that runs when a new session is started
            Application.Lock();
            //Session.Timeout = 1200;
            Application["NoOfVisitors"] = (int)Application["NoOfVisitors"] + 1;
            Application.UnLock();
        }
    }
}