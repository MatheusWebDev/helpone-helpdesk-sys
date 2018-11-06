using System.Web.Mvc;
using System.Web.Routing;

namespace helpone_helpdesk_sys
{
	public class RouteConfig
	{
		public static void RegisterRoutes(RouteCollection routes)
		{
			routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
			//routes.LowercaseUrls = true;
			routes.MapRoute(
				 name: "Login",
				 url: "{controller}/{action}/{id}",
				 defaults: new { controller = "Usuario", action = "Login", id = UrlParameter.Optional }
			);

			routes.MapRoute(
				 name: "Default",
				 url: "{controller}/{action}/{id}",
				 defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
			);
		}
	}
}
