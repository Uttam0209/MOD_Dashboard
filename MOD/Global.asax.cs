using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Routing;

namespace MOD
{
    public class MvcApplication : System.Web.HttpApplication
    {
        private static string WebPortalUrl = ConfigurationManager.AppSettings["WebPortalUrl"].ToString();
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            AntiForgeryConfig.SuppressIdentityHeuristicChecks = true;
            MvcHandler.DisableMvcResponseHeader = true;

        }
        protected void Application_Error()
        {
            string ipaddress = Request.UserHostAddress;
            Exception ex = Server.GetLastError();
            Server.ClearError();
            // Ravi  Start
            string message = string.Format("Time: {0}", DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt"));
            message += Environment.NewLine;
            message += "-----------------------------------------------------------";
            message += Environment.NewLine;
            message += string.Format("Message: {0}", ex);
            message += Environment.NewLine;
            //message += string.Format("StackTrace: {0}", ex.StackTrace);
            //message += Environment.NewLine;
            //message += string.Format("Source: {0}", ex.Source);
            //message += Environment.NewLine;
            //message += string.Format("TargetSite: {0}", ex.TargetSite.ToString());
            //message += Environment.NewLine;
            message += "-----------------------------------------------------------";
            message += Environment.NewLine;
           // string Basepath = Directory.GetCurrentDirectory();
            // string rootPath = TPResources.env.ContentRootPath;
            String logFileName = "log_" + DateTime.Today.ToString("yyyyMMdd") + "." + "txt";
            //string path = Basepath + "\\bin\\" + logFileName + "";

            string path = Server.MapPath("~/Logs/ " + logFileName + "");
            using (StreamWriter writer = new StreamWriter(path, true))
            {
                writer.WriteLine(message);
                writer.Close();
            }
            // end 
            //Response.Redirect(String.Format("http://localhost:56924/Account/Error"));
           // Response.Redirect(String.Format("https://172.31.2.132:8443/Error"));
            Response.Redirect(String.Format(WebPortalUrl));
            
        }
        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            string[] headers = { "Server", "X-AspNet-Version" };

            if (!Response.HeadersWritten)
            {
                Response.AddOnSendingHeaders((c) =>
                {
                    if (c != null && c.Response != null && c.Response.Headers != null)
                    {
                        foreach (string header in headers)
                        {
                            if (c.Response.Headers[header] != null)
                            {
                                c.Response.Headers.Remove(header);
                            }
                        }
                    }
                });
            }

        }

        protected void Application_EndRequest()
        {
            // removing excessive headers. They don't need to see this.
            Response.Headers.Remove("header_name");
        }
        public class SessionExpire : ActionFilterAttribute
        {
            public override void OnActionExecuting(ActionExecutingContext filterContext)
            {
                HttpContext ctx = HttpContext.Current;
                // check  sessions here
                if (HttpContext.Current.Session["UserID"] == null)
                {
                    //filterContext.Result = new RedirectResult("http://localhost:56924/Account/login");
                    // filterContext.Result = new RedirectResult("http://localhost:51994/");
                    filterContext.Result = new RedirectResult(WebPortalUrl);
                    
                    return;
                }
                base.OnActionExecuting(filterContext);
            }
        }
        public class SessionExpireRefNo : ActionFilterAttribute
        {
            public override void OnActionExecuting(ActionExecutingContext filterContext)
            {
                HttpContext ctx = HttpContext.Current;
                // check  sessions here
                if (HttpContext.Current.Session["UserName"] == null)
                {
                    // filterContext.Result = new RedirectResult("http://localhost:56924/Account/login");
                    //filterContext.Result = new RedirectResult("http://localhost:51994/");
                    filterContext.Result = new RedirectResult(WebPortalUrl);
                    return;
                }
                base.OnActionExecuting(filterContext);
            }
        }
    }
}
