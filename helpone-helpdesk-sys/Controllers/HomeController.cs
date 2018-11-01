using helpone_helpdesk_sys.DAL;
using helpone_helpdesk_sys.Models;
using helpone_helpdesk_sys.Models.Enums;
using System.Web;
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
				return View("IndexOp", usuarioLogado);
			} else if (usuarioLogado.TipoAcesso == EnumTipoUsuario.Suporte || usuarioLogado.TipoAcesso == EnumTipoUsuario.Desenvolvimento)
			{
				return View("IndexSup", usuarioLogado);
			}
			return View("IndexOp");
		}
	}
}