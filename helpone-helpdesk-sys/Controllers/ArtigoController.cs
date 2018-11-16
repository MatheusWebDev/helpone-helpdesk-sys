using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using helpone_helpdesk_sys.DAL;
using helpone_helpdesk_sys.Models;

namespace helpone_helpdesk_sys.Controllers
{
	public class ArtigoController : Controller
	{
		private HelpDeskContext db = new HelpDeskContext();

		// GET: Artigo
		public ActionResult Index()
		{
			var artigos = db.Artigos.Include(a => a.Subtopico);
			return View(artigos.ToList());
		}

		// GET: Artigo/Detalhes/5
		public ActionResult Detalhes(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			Artigo artigo = db.Artigos.Find(id);
			if (artigo == null)
			{
				return HttpNotFound();
			}
			return View(artigo);
		}

		// POST: Artigo/Votar/5
		public ActionResult Votar(String voto,int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			Artigo artigo = db.Artigos.Find(id);
			if (artigo == null)
			{
				return HttpNotFound();
			}

			if (ModelState.IsValid)
			{
				if (voto == "true")
				{
					artigo.QtdLike++;
				}
				if (voto == "false") {
					artigo.QtdUnlike++;
				}

				db.Entry(artigo).State = EntityState.Modified;
				db.SaveChanges();
				return View("Detalhes", artigo);
			}
			return View("Detalhes", artigo);
		}


		// GET: Artigo/Create
		public ActionResult Create()
		{
			ViewBag.SubtopicoID = new SelectList(db.Subtopicos, "Id", "Titulo");
			ViewBag.UsuarioID = new SelectList(db.Usuarios, "Id", "Login");
			return View();
		}

		// POST: Artigo/Create
		// To protect from overposting attacks, please enable the specific properties you want to bind to, for
		// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Create([Bind(Include = "Id,TituloArtigo,ConteudoArtigo,Status,DataCriacao,DataFim,QtdLike,QtdUnlike,SubtopicoID,UsuarioID")] Artigo artigo)
		{
			if (ModelState.IsValid)
			{
				db.Artigos.Add(artigo);
				db.SaveChanges();
				return RedirectToAction("Index");
			}

			ViewBag.SubtopicoID = new SelectList(db.Subtopicos, "Id", "Titulo", artigo.SubtopicoID);
			ViewBag.UsuarioID = new SelectList(db.Usuarios, "Id", "Login", artigo.UsuarioID);
			return View(artigo);
		}

		// GET: Artigo/Edit/5
		public ActionResult Editar(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			Artigo artigo = db.Artigos.Find(id);
			if (artigo == null)
			{
				return HttpNotFound();
			}
			ViewBag.SubtopicoID = new SelectList(db.Subtopicos, "Id", "Titulo", artigo.SubtopicoID);
			ViewBag.UsuarioID = new SelectList(db.Usuarios, "Id", "Login", artigo.UsuarioID);
			return View(artigo);
		}

		// POST: Artigo/Edit/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Editar([Bind(Include = "Id,Titulo,ConteudoArtigo,SubtopicoID,QtdLike, QtdUnlike")] Artigo artigo)
		{
			if (ModelState.IsValid)
			{
				artigo.DataCriacao = DateTime.Now;

				db.Entry(artigo).State = EntityState.Modified;
				db.SaveChanges();
				return RedirectToAction("Index");
			}
			ViewBag.SubtopicoID = new SelectList(db.Subtopicos, "Id", "Titulo", artigo.SubtopicoID);
			return View(artigo);
		}


		// POST: Chamado/Apagar/5
		[HttpPost, ActionName("Apagar")]
		[ValidateAntiForgeryToken]
		public ActionResult ApagarConfirmed(int id)
		{
			Artigo artigo = db.Artigos.Find(id);

			artigo.DataFim = DateTime.Now;

			db.Entry(artigo).State = EntityState.Modified;
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
