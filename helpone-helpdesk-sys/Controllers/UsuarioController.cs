using helpone_helpdesk_sys.DAL;
using helpone_helpdesk_sys.Models;
using System.Web.Mvc;

namespace helpone_helpdesk_sys.Controllers
{
	public class UsuarioController : Controller
	{
		private HelpDeskContext db = new HelpDeskContext();

		//GET: Fomulário Login
		public ActionResult Login()
		{
			return View();
		}

		//POST: Logar usuário
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Logar(int id)
		{
			Usuario usuario = db.Usuarios.Find(id);
			if (Session["userLogged"] != null && Session["userLoggedId"] != null)
			{
				Session["userLogged"] = null;
				Session["userLoggedId"] = null;
			}
			return RedirectToAction("Index", "Home", usuario);
		}

	}
}
