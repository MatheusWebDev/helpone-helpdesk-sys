using System.Web;
using System.Web.Mvc;

namespace helpone_helpdesk_sys
{
	public class FilterConfig
	{
		public static void RegisterGlobalFilters(GlobalFilterCollection filters)
		{
			filters.Add(new HandleErrorAttribute());
		}
	}
}
