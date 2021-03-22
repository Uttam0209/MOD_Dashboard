using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Routing;

namespace MOD
{
    public class MvcApplication : System.Web.HttpApplication
    {
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
            Response.Redirect(String.Format("http://localhost:56924/Account/Error"));

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
                    filterContext.Result = new RedirectResult("http://localhost:56924/Account/login");
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
                    filterContext.Result = new RedirectResult("http://localhost:56924/Account/login");
                    return;
                }
                base.OnActionExecuting(filterContext);
            }
        }
    }
}
