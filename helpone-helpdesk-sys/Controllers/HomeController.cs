using helpone_helpdesk_sys.Models;
using System.Web.Mvc;

namespace helpone_helpdesk_sys.Controllers
{
	public class HomeController : Controller
	{
		public ActionResult Index(Usuario usuarioLogado)
		{
			return View(usuarioLogado);
		}
	}
}