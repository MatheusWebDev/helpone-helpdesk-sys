using helpone_helpdesk_sys.DAL;
using helpone_helpdesk_sys.Models;
using helpone_helpdesk_sys.Models.Enums;
using System.Web.Mvc;

namespace helpone_helpdesk_sys.Controllers
{
	public class HomeController : Controller
	{
		private HelpDeskContext db = new HelpDeskContext();

		public ActionResult Index(Usuario usuarioLogado)
		{

			if (usuarioLogado.TipoAcesso == EnumTipoUsuario.Operador)
			{
				if (Session["userLogged"] != null && Session["userLoggedId"] != null)
				{
					Session["userLogged"] = null;
					Session["userLoggedId"] = null;
				}
				return View("IndexOpe", usuarioLogado);
			} else if (usuarioLogado.TipoAcesso == EnumTipoUsuario.Suporte || usuarioLogado.TipoAcesso == EnumTipoUsuario.Desenvolvimento)
			{
				if (Session["userLogged"] != null && Session["userLoggedId"] != null)
				{
					Session["userLogged"] = null;
					Session["userLoggedId"] = null;
				}
				return View("IndexSup", usuarioLogado);
			}
			return View("IndexOpe");
		}

		[ChildActionOnly]
		public PartialViewResult UserLogged(Usuario usuarioLogado)
		{
			if (Session["userLogged"] == null && Session["userLoggedId"] == null)
			{
				Session["userLogged"] = usuarioLogado.Login;
				Session["userLoggedId"] = usuarioLogado.Id;
			}

			return PartialView("_UserLogged");
		}
	}
}