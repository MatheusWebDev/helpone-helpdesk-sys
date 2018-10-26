using System.Web.Mvc;

namespace helpone_helpdesk_sys.Controllers
{
	public class UsuarioController : Controller
	{
		//private HelpDeskContext db = new HelpDeskContext();

		//GET: Fomulário Login
		public ActionResult Login()
		{
			return View();
		}

		//POST: Logar usuário
		public ActionResult Logar()
		{
			return RedirectToAction("Index", "Home");
		}

	}
}
