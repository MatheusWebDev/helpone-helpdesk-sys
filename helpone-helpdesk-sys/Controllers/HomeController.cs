using helpone_helpdesk_sys.DAL;
using helpone_helpdesk_sys.Models;
using helpone_helpdesk_sys.Models.Enums;
using System.Net;
using System.Web.Mvc;
using System.Linq;

namespace helpone_helpdesk_sys.Controllers
{
	public class HomeController : Controller
	{
		private HelpDeskContext db = new HelpDeskContext();

		public ActionResult Index(Usuario usuarioLogado)
		{
			if (Session["userLogged"] == null && Session["userLoggedId"] == null)
			{
				Session["userLogged"] = usuarioLogado.Login;
				Session["userLoggedId"] = usuarioLogado.Id;
				Session["userLoggedDal"] = usuarioLogado.Daltonismo;
			}

			Usuario userLogged = db.Usuarios.Find(Session["userLoggedId"]);

			ViewBag.Topicos = db.Topicos.AsEnumerable();
			ViewBag.Subtopicos = db.Subtopicos.AsEnumerable();

			if (userLogged.TipoAcesso == EnumTipoUsuario.Operador ||
				 userLogged.TipoAcesso == EnumTipoUsuario.Suporte ||
				 userLogged.TipoAcesso == EnumTipoUsuario.Desenvolvimento)
			{
				return View();
			}
			else
			{
				return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);
			}
		}

		[ChildActionOnly]
		public PartialViewResult UserLogged()
		{
			return PartialView("_UserLogged");
		}

		[ChildActionOnly]
		public PartialViewResult SectionHero()
		{
			Usuario usuarioLogado = db.Usuarios.Find(Session["userLoggedId"]);

			if (usuarioLogado.TipoAcesso == EnumTipoUsuario.Operador)
			{
				return PartialView("_SectionHeroOpe");
			}
			else if (usuarioLogado.TipoAcesso == EnumTipoUsuario.Suporte || usuarioLogado.TipoAcesso == EnumTipoUsuario.Desenvolvimento)
			{
				return PartialView("_SectionHeroSup");
			}
			return PartialView("_NaoAutorizado");
		}

		[ChildActionOnly]
		public PartialViewResult Breadcrumb()
		{
			return PartialView("_Breadcrumb");
		}
	}
}