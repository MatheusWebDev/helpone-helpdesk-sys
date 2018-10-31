using System;
using System.Collections.Generic;
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
			var chamados = db.Chamados.ToList();
			return View(chamados);
		}

		// GET: Chamado/Acompanhar/5
		public ActionResult Acompanhar(int? id)
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

		// GET: Chamado/Abrir [FORMULARIO]
		public ActionResult Abrir()
		{
			ViewBag.TopicoID = new SelectList(db.Topicos, "Id", "Titulo");
			ViewBag.SubtopicoID = new SelectList(db.Subtopicos, "Id", "Titulo");
			return View();
		}

		// POST: Chamado/Abrir [AÇÃO SALVA]
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Abrir([Bind(Include = "Titulo,TopicoID,SubtopicoID")] Chamado chamado, String conteudoForm)
		{
			if (ModelState.IsValid)
			{
				chamado.Status = EnumStatus.AguardandoResposta;
				chamado.DataCriacao = String.Format("{0:dd/MM/yyyy - HH:mm}", DateTime.Now);
				chamado.UsuarioID = 1; // TODO: implementar depois -> pegar ID do usuario logado

				var listaSubtSuporte = db.Subtopicos.Where(st => st.TopicoID == 1).ToList();
				var listaSubtDev = db.Subtopicos.Where(st => st.TopicoID == 2).ToList();

				if (listaSubtSuporte.Any(st => st.Id == chamado.SubtopicoID))
				{
					chamado.EquipeAtendimento = EnumTipoEquipe.Suporte;
				}
				else if (listaSubtDev.Any(st => st.Id == chamado.SubtopicoID))
				{
					chamado.EquipeAtendimento = EnumTipoEquipe.Desenvolvimento;
				} else {
					return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
				}

				// Cria uma instância de 'Conteudo' para salvar na lista de 'Conteudos' do chamado
				Conteudo conteudoSalvar = new Conteudo();
				conteudoSalvar.ConteudoChamado = conteudoForm;
				conteudoSalvar.UsuarioID = 1; // TODO: implementar depois -> pegar ID do usuario logado
				conteudoSalvar.DataCriacao = String.Format("{0:dd/MM/yyyy - HH:mm}", DateTime.Now);
				conteudoSalvar.Chamado = chamado; // salva o chamado no 'conteudo'
				db.Conteudos.Add(conteudoSalvar);

				chamado.Conteudos.Add(conteudoSalvar); // salva o conteudo no chamado
				db.Chamados.Add(chamado);
				db.SaveChanges();
				return RedirectToAction("Index");
			}

			ViewBag.TopicoID = new SelectList(db.Topicos, "Id", "Titulo");
			ViewBag.SubtopicoID = new SelectList(db.Subtopicos, "Id", "Titulo");
			return View(chamado);
		}

		// GET: Chamado/Editar/5 [FORMULARIO]
		public ActionResult Editar(int? id)
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

		// POST: Chamado/Editar/5 [AÇÃO SALVA]
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Editar([Bind(Include = "Titulo,TopicoID,SubtopicoID")] Chamado chamadoForm, String conteudoForm, int id)
		{
			if (ModelState.IsValid)
			{
				List<Chamado> acharChamadoOld = db.Chamados.Where(c => c.Id == id).Include(c => c.Conteudos).ToList();
				if (acharChamadoOld != null || acharChamadoOld.Count < 2)
				{
					Chamado chamadoEditar = acharChamadoOld.First();
					chamadoEditar.DataCriacao = String.Format("{0:dd/MM/yyyy - HH:mm}", DateTime.Now);
					// SE mudar o Subtopico
					if (!chamadoEditar.SubtopicoID.Equals(chamadoForm.SubtopicoID))
					{
						var listaSubtSuporte = db.Subtopicos.Where(st => st.TopicoID == 1).ToList();
						var listaSubtDev = db.Subtopicos.Where(st => st.TopicoID == 2).ToList();
						if (listaSubtSuporte.Any(st => st.Id == chamadoForm.SubtopicoID))
						{
							chamadoEditar.EquipeAtendimento = EnumTipoEquipe.Suporte;
						}
						else if (listaSubtDev.Any(st => st.Id == chamadoForm.SubtopicoID))
						{
							chamadoEditar.EquipeAtendimento = EnumTipoEquipe.Desenvolvimento;
						}
						else
						{
							chamadoEditar.EquipeAtendimento = EnumTipoEquipe.Suporte;
						}
					}

					// Edita o 'Conteudo' deste chamadoEditar
					Conteudo conteudoEditar = chamadoEditar.Conteudos.First();
					conteudoEditar.ConteudoChamado = conteudoForm;
					conteudoEditar.DataCriacao = String.Format("{0:dd/MM/yyyy - HH:mm}", DateTime.Now);
					db.Entry(conteudoEditar).State = EntityState.Modified;

					db.Entry(chamadoEditar).State = EntityState.Modified;
				}

				db.SaveChanges();
				return RedirectToAction("Index");
			}
			ViewBag.TopicoID = new SelectList(db.Topicos, "Id", "Titulo");
			ViewBag.SubtopicoID = new SelectList(db.Subtopicos, "Id", "Titulo");
			return View(chamadoForm);
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

		// POST: Chamado/Finalizar/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Finalizar([Bind(Include = "SolucaoFoiUtil,NivelSatisfacao,Mensagem")] Feedback feedback, int id)
		{
			// Se usuário NÃO informar NADA => salva chamado comoo 'SemFeedback'
			if (feedback.NivelSatisfacao == 0 && (feedback.Mensagem == null || feedback.Mensagem == ""))
			{
				Chamado chamado = db.Chamados.Find(id);
				chamado.Status = EnumStatus.FinalizadoSemFeedback;
				chamado.DataFim = String.Format("{0:dd/MM/yyyy - HH:mm}", DateTime.Now);

				db.Entry(chamado).State = EntityState.Modified;
			}
			else // Se usuário informar ALGO => lógica para salvar o 'Feedback' && salvar o chamado com o Feedback
			{
				Chamado chamado = db.Chamados.Find(id);
				if (feedback.SolucaoFoiUtil)
				{
					chamado.Status = EnumStatus.FinalizadoFeedbackPositivo;
				}
				else
				{
					chamado.Status = EnumStatus.FinalizadoFeedbackNegativo;
				}
				feedback.Chamado = chamado;
				feedback.ChamadoID = chamado.Id;
				feedback.DataCriacao = String.Format("{0:dd/MM/yyyy - HH:mm}", DateTime.Now);
				feedback.UsuarioID = 1; // implementar depois
				chamado.Feedback = feedback;
				db.Feedbacks.Add(feedback);
				db.Entry(chamado).State = EntityState.Modified;
			}

			db.SaveChanges();
			return RedirectToAction("Index");
		}
	}
}
