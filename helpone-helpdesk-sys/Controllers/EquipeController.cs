using System;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using helpone_helpdesk_sys.DAL;
using helpone_helpdesk_sys.Models;
using helpone_helpdesk_sys.Models.Enums;

namespace helpone_helpdesk_sys.Controllers
{
	public class EquipeController : Controller
    {
        private HelpDeskContext db = new HelpDeskContext();

        // GET: Equipe
        public ActionResult Index()
        {
            return View(db.Equipes.ToList());
        }

        // GET: Equipe/Detalhes/5
        public ActionResult Detalhes(int? id, string search)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
				Equipe equipe = db.Equipes.Find(id);
				var usuarios = db.Usuarios.Where(a => a.Id > 0);
				//PESQUISA
				ViewBag.pesquisa = false;
				ViewBag.TextoPesquisado = "";
				ViewBag.QtdEncontrado = 0;

				if (!String.IsNullOrEmpty(search))
				{
					ViewBag.pesquisa = true;
					ViewBag.TextoPesquisado = search;
					usuarios = usuarios.Where(a => a.Login.Contains(search) || a.TipoAcesso == EnumTipoUsuario.Operador);
					ViewBag.QtdEncontrado = usuarios.Count();
					ViewBag.Usuarios = usuarios.ToList();
				}

			if (equipe == null)
            {
                return HttpNotFound();
            }
            return View(equipe);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
