using helpone_helpdesk_sys.DAL;
using helpone_helpdesk_sys.Models;
using System;
using System.Web;
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

			if(usuario.TipoAcesso == Models.Enums.EnumTipoUsuario.Suporte || usuario.TipoAcesso == Models.Enums.EnumTipoUsuario.Desenvolvimento)
			{
				return RedirectToAction("Index", "Home", usuario);
			} else if (usuario.TipoAcesso == Models.Enums.EnumTipoUsuario.Operador)
			{
				return RedirectToAction("Index", "Home", usuario);
			}
			throw new HttpException(401, "Unauthorized access");
		}

	}
}
