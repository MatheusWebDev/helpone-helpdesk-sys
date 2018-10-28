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
			var chamados = db.Chamados.Include(c => c.Conteudos).Include(c => c.Subtopico).Include(c => c.Subtopico.Topico);
			//var conteudos = db.Conteudos;
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

		// GET: Chamado/Create [FORMULARIO]
		public ActionResult Create()
		{
			ViewBag.TopicoID = new SelectList(db.Topicos, "Id", "Titulo");
			ViewBag.SubtopicoID = new SelectList(db.Subtopicos, "Id", "Titulo");
			return View();
		}

		// POST: Chamado/Create [AÇÃO SALVA]
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Create([Bind(Include = "Titulo,TopicoID,SubtopicoID")] Chamado chamado, String conteudoForm)
		{
			if (ModelState.IsValid)
			{
				chamado.Status = EnumStatus.AguardandoResposta;
				chamado.DataCriacao = String.Format("{0:dd/MM/yyyy - HH:mm}", DateTime.Now);
				chamado.UsuarioID = 1; // implementar depois -> pegar ID do usuario logado

				if (chamado.SubtopicoID == 1) // implementar mais corretamente depois -> procura lista de subtopicos (problemas p/ suporte) se o SubtopicoID cadastrado no formulário está na lista de problemas p/ suporte && mesmo para comparação a baixo, porem para equipe de desenvolvimento || algo assim "db.Subtopicos.ToList().ForEach(subt => subt.Id == chamado.SubtopicoID )"
				{
					chamado.EquipeAtendimento = EnumTipoEquipe.Suporte;
				}
				else if (chamado.SubtopicoID == 3)
				{
					chamado.EquipeAtendimento = EnumTipoEquipe.Desenvolvimento;
				}

				// Cria o conteudo para salvar na lista de conteudos do chamado
				Conteudo conteudoSalvar = new Conteudo();
				conteudoSalvar.ConteudoChamado = conteudoForm;
				conteudoSalvar.UsuarioID = 1; // implementar depois -> pegar ID do usuario logado
				conteudoSalvar.DataCriacao = String.Format("{0:dd/MM/yyyy - HH:mm}", DateTime.Now);
				conteudoSalvar.Chamado = chamado;
				db.Conteudos.Add(conteudoSalvar);

				// salva o conteudo no chamado
				chamado.Conteudos.Add(conteudoSalvar);
				var teste = chamado.Conteudos;
				db.Chamados.Add(chamado);
				db.SaveChanges();
				return RedirectToAction("Index");
			}

			ViewBag.TopicoID = new SelectList(db.Topicos, "Id", "Titulo");
			ViewBag.SubtopicoID = new SelectList(db.Subtopicos, "Id", "Titulo");
			return View(chamado);
		}

		// GET: Chamado/Edit/5 [FORMULARIO]
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
			ViewBag.TopicoID = new SelectList(db.Topicos, "Id", "Titulo");
			ViewBag.SubtopicoID = new SelectList(db.Subtopicos, "Id", "Titulo");
			return View(chamado);
		}

		// POST: Chamado/Edit/5 [AÇÃO SALVA]
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Edit([Bind(Include = "Titulo,TopicoID,SubtopicoID")] Chamado chamado, String conteudoForm)
		{
			if (ModelState.IsValid)
			{
				chamado.DataCriacao = String.Format("{0:dd/MM/yyyy - HH:mm}", DateTime.Now);

				if (chamado.SubtopicoID == 1) // implementar mais corretamente depois -> procura lista de subtopicos (problemas p/ suporte) se o SubtopicoID cadastrado no formulário está na lista de problemas p/ suporte && mesmo para comparação a baixo, porem para equipe de desenvolvimento || algo assim "db.Subtopicos.ToList().ForEach(subt => subt.Id == chamado.SubtopicoID )"
				{
					chamado.EquipeAtendimento = EnumTipoEquipe.Suporte;
				}
				else if (chamado.SubtopicoID == 3)
				{
					chamado.EquipeAtendimento = EnumTipoEquipe.Desenvolvimento;
				}

				// Edita o conteudo para salvar na lista de conteudos do chamado
				Conteudo conteudoSalvar = chamado.Conteudos.First();
				conteudoSalvar.ConteudoChamado = conteudoForm;
				conteudoSalvar.DataCriacao = String.Format("{0:dd/MM/yyyy - HH:mm}", DateTime.Now);
				db.Entry(conteudoSalvar).State = EntityState.Modified;

				db.Entry(chamado).State = EntityState.Modified;
				db.SaveChanges();
				return RedirectToAction("Index");
			}
			ViewBag.TopicoID = new SelectList(db.Topicos, "Id", "Titulo");
			ViewBag.SubtopicoID = new SelectList(db.Subtopicos, "Id", "Titulo");
			return View(chamado);
		}

		// GET: Chamado/Cancelar/5
		public ActionResult Cancelar(int? id)
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

		// POST: Chamado/Cancelar/5
		[HttpPost, ActionName("Cancelar")]
		[ValidateAntiForgeryToken]
		public ActionResult CancelarConfirmed(int id)
		{
			Chamado chamado = db.Chamados.Find(id);

			chamado.Status = EnumStatus.Cancelado;
			chamado.DataFim = String.Format("{0:dd/MM/yyyy - HH:mm}", DateTime.Now);

			db.Entry(chamado).State = EntityState.Modified;
			db.SaveChanges();
			return RedirectToAction("Index");
		}
	}
}
