using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using helpone_helpdesk_sys.DAL;
using helpone_helpdesk_sys.Models.Chamados;
using helpone_helpdesk_sys.Models.Enums;

namespace helpone_helpdesk_sys.Controllers
{
	public class ChamadoController : Controller
	{
		private HelpDeskContext db = new HelpDeskContext();

		// GET: Chamado
		public ActionResult Index()
		{
			var chamados = db.Chamados.Include(c => c.Subtopico).Include(c => c.Subtopico.Topico);
			return View(chamados.ToList());
		}

		// GET: Chamado/Details/5
		public ActionResult Details(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			Chamado chamado = db.Chamados.Find(id);
			if (chamado == null)
			{
				return HttpNotFound();
			}
			return View(chamado);
		}

		// GET: Chamado/Create
		public ActionResult Create()
		{
			ViewBag.TopicoID = new SelectList(db.Topicos, "Id", "Titulo");
			ViewBag.SubtopicoID = new SelectList(db.Subtopicos, "Id", "Titulo");
			return View();
		}

		// POST: Chamado/Create
		// To protect from overposting attacks, please enable the specific properties you want to bind to, for
		// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Create([Bind(Include = "Titulo,TopicoID,SubtopicoID")] Chamado chamado)
		{
			if (ModelState.IsValid)
			{
				chamado.Status = EnumStatus.AguardandoResposta;
				chamado.DataCriacao = String.Format("{0:dd/MM/yyyy - HH:mm}", DateTime.Now);
				chamado.UsuarioID = 1; // implementar depois -> pegar ID do usuario logado
				if (chamado.SubtopicoID == 1) // implementar mais corretamente depois -> procura lista de subtopicos (problemas p/ suporte) se o SubtopicoID cadastrado no formulário está na lista de problemas p/ suporte && mesmo para comparação a baixo, porem para equipe de desenvolvimento || algo assim "db.Subtopicos.ToList().ForEach(subt => subt.Id == chamado.SubtopicoID )"
				{
					chamado.EquipeAtendimento = EnumTipoEquipe.Suporte;
				} else if (chamado.SubtopicoID == 3)
				{
					chamado.EquipeAtendimento = EnumTipoEquipe.Desenvolvimento;
				}
				db.Chamados.Add(chamado);
				db.SaveChanges();
				return RedirectToAction("Index");
			}

			ViewBag.SubtopicoID = new SelectList(db.Subtopicos, "Id", "Titulo", chamado.SubtopicoID);
			ViewBag.UsuarioID = new SelectList(db.Usuarios, "Id", "Login", chamado.UsuarioID);
			return View(chamado);
		}

		// GET: Chamado/Edit/5
		public ActionResult Edit(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			Chamado chamado = db.Chamados.Find(id);
			if (chamado == null)
			{
				return HttpNotFound();
			}
			ViewBag.SubtopicoID = new SelectList(db.Subtopicos, "Id", "Titulo", chamado.SubtopicoID);
			ViewBag.UsuarioID = new SelectList(db.Usuarios, "Id", "Login", chamado.UsuarioID);
			return View(chamado);
		}

		// POST: Chamado/Edit/5
		// To protect from overposting attacks, please enable the specific properties you want to bind to, for
		// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Edit([Bind(Include = "Id,Titulo,Status,EquipeAtendimento,DataCriacao,DataFim,UsuarioID,SubtopicoID")] Chamado chamado)
		{
			if (ModelState.IsValid)
			{
				db.Entry(chamado).State = EntityState.Modified;
				db.SaveChanges();
				return RedirectToAction("Index");
			}
			ViewBag.SubtopicoID = new SelectList(db.Subtopicos, "Id", "Titulo", chamado.SubtopicoID);
			ViewBag.UsuarioID = new SelectList(db.Usuarios, "Id", "Login", chamado.UsuarioID);
			return View(chamado);
		}

		// GET: Chamado/Delete/5
		public ActionResult Delete(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			Chamado chamado = db.Chamados.Find(id);
			if (chamado == null)
			{
				return HttpNotFound();
			}
			return View(chamado);
		}

		// POST: Chamado/Delete/5
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public ActionResult DeleteConfirmed(int id)
		{
			Chamado chamado = db.Chamados.Find(id);
			db.Chamados.Remove(chamado);
			db.SaveChanges();
			return RedirectToAction("Index");
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
