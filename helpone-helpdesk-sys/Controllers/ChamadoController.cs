using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using helpone_helpdesk_sys.DAL;
using helpone_helpdesk_sys.Models;
using helpone_helpdesk_sys.Models.Chamados;
using helpone_helpdesk_sys.Models.Enums;

namespace helpone_helpdesk_sys.Controllers
{
	public class ChamadoController : Controller
	{
		private HelpDeskContext db = new HelpDeskContext();

		public void ToggleActiveClass()
		{
			string show = "";
			string[] active = new string[] { "", "", "", "", "", "" };
			switch (ViewBag.StatusFilterParm)
			{
				case "aguardando":
					active[0] = "active";
					show = "show";
					break;
				case "cancelados":
					active[1] = "active";
					show = "show";
					break;
				case "respondidos":
					active[2] = "active";
					show = "show";
					break;
				case "finalizados":
					active[3] = "active";
					show = "show";
					break;
				case "finalizados-feedback-pos":
					active[4] = "active";
					show = "show";
					break;
				default:
					active[0] = "";
					show = "";
					break;
			}
		}

		// GET: Chamado [Lista de Chamados]
		public ActionResult Index(string sortOrder, string filter)
		{
			ViewBag.IdSortParm = String.IsNullOrEmpty(sortOrder) ? "id_desc" : "";
			ViewBag.TituloSortParm = sortOrder == "Titulo" ? "titulo_desc" : "Titulo";
			ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";
			if(!String.IsNullOrEmpty(sortOrder))
			{
				ViewBag.QuerySort = sortOrder;
			}

			if (!String.IsNullOrEmpty(filter))
			{
				ViewBag.QueryFilter = filter;
			}

			ViewBag.AllFilterParm = String.IsNullOrEmpty(filter) ? "todos" : "";
			ViewBag.ActiveClassAllFilter = String.IsNullOrEmpty(ViewBag.AllFilterParm) ? "active" : "";
			ViewBag.StatusFilterParm = "";
			var chamados = from c in db.Chamados
								select c;
			switch (sortOrder)
			{
				case "id_desc":
					chamados = chamados.OrderByDescending(c => c.Id);
					break;
				case "Titulo":
					chamados = chamados.OrderBy(c => c.Titulo);
					break;
				case "titulo_desc":
					chamados = chamados.OrderByDescending(c => c.Titulo);
					break;
				case "Date":
					chamados = chamados.OrderBy(c => c.DataCriacao);
					break;
				case "date_desc":
					chamados = chamados.OrderByDescending(c => c.DataCriacao);
					break;
				default:
					chamados = chamados.OrderBy(c => c.Id);
					break;
			}
			Usuario userLogged = db.Usuarios.Find(Session["userLoggedId"]); // TODO: fazer comparação em todos 'CASE' do SWITCH para exibir apenas os chamados daquele usuário (Se ele for operador))
			switch (filter)
			{
				case "todos":
					chamados = chamados.Where(c => c.Id > 0 && c.UsuarioID == userLogged.Id);
					ViewBag.StatusFilterParm = "todos";
					break;
				case "aguardando":
					chamados = chamados.Where(c => c.Status == EnumStatus.AguardandoResposta);
					ViewBag.StatusFilterParm = "aguardando";
					break;
				case "cancelados":
					chamados = chamados.Where(c => c.Status == EnumStatus.Cancelado);
					ViewBag.StatusFilterParm = "cancelados";
					break;
				case "respondidos":
					chamados = chamados.Where(c => c.Status == EnumStatus.ChamadoRespondido);
					ViewBag.StatusFilterParm = "respondidos";
					break;
				case "finalizados-feedback-pos":
					chamados = chamados.Where(c => c.Status == EnumStatus.FinalizadoFeedbackPositivo);
					ViewBag.StatusFilterParm = "finalizados-feedback-pos";
					break;
				case "finalizados":
					chamados = chamados.Where(c => c.Status == EnumStatus.FinalizadoFeedbackNegativo ||
															 c.Status == EnumStatus.FinalizadoFeedbackPositivo ||
															 c.Status == EnumStatus.FinalizadoSemFeedback);
					ViewBag.StatusFilterParm = "finalizados";
					break;
				default:
					if (userLogged.TipoAcesso == EnumTipoUsuario.Suporte)
					{
						chamados = chamados.Where(c => c.Status == EnumStatus.AguardandoResposta &&
																 c.EquipeAtendimento == EnumTipoEquipe.Suporte);
					}
					else if (userLogged.TipoAcesso == EnumTipoUsuario.Desenvolvimento)
					{
						chamados = chamados.Where(c => c.Status == EnumStatus.AguardandoResposta &&
																 c.EquipeAtendimento == EnumTipoEquipe.Desenvolvimento);
					}
					else
					{
						chamados = chamados.Where(c => (c.Status == EnumStatus.AguardandoResposta || c.Status == EnumStatus.ChamadoRespondido) &&
																 c.UsuarioID == userLogged.Id);
					}
					break;
			}

			if (userLogged.TipoAcesso == EnumTipoUsuario.Suporte || userLogged.TipoAcesso == EnumTipoUsuario.Desenvolvimento)
			{
				ViewBag.TipoAcaoUsuario = "Atender";
			}
			else
			{
				ViewBag.TipoAcaoUsuario = "Acompanhar";
			}
			return View(chamados.ToList());
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
				Usuario usuario = db.Usuarios.Find(Session["userLoggedId"]);
				chamado.UsuarioID = usuario.Id;

				chamado.Status = EnumStatus.AguardandoResposta;
				chamado.DataCriacao = DateTime.Now;

				var listaSubtSuporte = db.Subtopicos.Where(st => st.TopicoID == 1).ToList();
				var listaSubtDev = db.Subtopicos.Where(st => st.TopicoID == 2).ToList();

				if (listaSubtSuporte.Any(st => st.Id == chamado.SubtopicoID))
				{
					chamado.EquipeAtendimento = EnumTipoEquipe.Suporte;
				}
				else if (listaSubtDev.Any(st => st.Id == chamado.SubtopicoID))
				{
					chamado.EquipeAtendimento = EnumTipoEquipe.Desenvolvimento;
				}
				else
				{
					return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
				}

				// Cria uma instância de 'Conteudo' para salvar na lista de 'Conteudos' do chamado
				Conteudo conteudoSalvar = new Conteudo();
				conteudoSalvar.ConteudoChamado = conteudoForm;
				conteudoSalvar.UsuarioID = usuario.Id;
				conteudoSalvar.DataCriacao = DateTime.Now;
				conteudoSalvar.ChamadoID = chamado.Id; // salva o chamado no 'conteudo'
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
					chamadoEditar.DataCriacao = DateTime.Now;
					// SE mudar o Subtopico
					if (!chamadoEditar.SubtopicoID.Equals(chamadoForm.SubtopicoID))
					{
						chamadoEditar.SubtopicoID = chamadoForm.SubtopicoID;

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
					// SE mudar o Titulo
					if (!chamadoEditar.Titulo.Equals(chamadoForm.Titulo))
					{
						chamadoEditar.Titulo = chamadoForm.Titulo;
					}
					// Edita o 'Conteudo' deste chamadoEditar
					Conteudo conteudoEditar = chamadoEditar.Conteudos.First();
					conteudoEditar.ConteudoChamado = conteudoForm;
					conteudoEditar.DataCriacao = DateTime.Now;

					db.Entry(conteudoEditar).State = EntityState.Modified;
					db.Entry(chamadoEditar).State = EntityState.Modified;
				} else {
					return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
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
			chamado.DataFim = DateTime.Now;

			db.Entry(chamado).State = EntityState.Modified;
			db.SaveChanges();
			return RedirectToAction("Index");
		}

		// POST: Chamado/Finalizar/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Finalizar([Bind(Include = "SolucaoFoiUtil,NivelSatisfacao,Mensagem")] Feedback feedback, int id)
		{
			// Se usuário NÃO informar NADA => salva chamado como 'SemFeedback'
			if (feedback.NivelSatisfacao == 0 && (feedback.Mensagem == null || feedback.Mensagem == ""))
			{
				Chamado chamado = db.Chamados.Find(id);
				chamado.Status = EnumStatus.FinalizadoSemFeedback;
				chamado.DataFim = DateTime.Now;

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
				feedback.UsuarioID = chamado.UsuarioID;
				feedback.Chamado = chamado;
				feedback.ChamadoID = chamado.Id;
				feedback.DataCriacao = DateTime.Now;
				chamado.Feedback = feedback;
				db.Feedbacks.Add(feedback);
				db.Entry(chamado).State = EntityState.Modified;
			}

			db.SaveChanges();
			return RedirectToAction("Index");
		}

		// GET: Chamado/Atender/5 [VISUALIZAR/ANÁLISE + FORMULÁRIO]
		public ActionResult Atender(int? id)
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

		// POST: Chamado/Atender/5 [AÇÃO SALVA RESPOSTA DO ATENDIMENTO e ACOMPANHAMENTO]
		[HttpPost, ActionName("Responder")]
		[ValidateAntiForgeryToken]
		public ActionResult Atender(String conteudoForm, int id)
		{
			if (ModelState.IsValid)
			{
				List<Chamado> acharChamadoOld = db.Chamados.Where(c => c.Id == id).Include(c => c.Conteudos).ToList();
				Usuario usuario = db.Usuarios.Find(Session["userLoggedId"]);
				if (acharChamadoOld != null || acharChamadoOld.Count < 2)
				{
					Chamado chamadoAtender = acharChamadoOld.First();

					chamadoAtender.Status = chamadoAtender.Status == EnumStatus.AguardandoResposta ? EnumStatus.ChamadoRespondido : EnumStatus.AguardandoResposta;

					// Cria uma instância de 'Conteudo' para salvar na lista de 'Conteudos' do chamado
					Conteudo conteudoAtendedimentoSalvar = new Conteudo();
					conteudoAtendedimentoSalvar.ConteudoChamado = conteudoForm;
					conteudoAtendedimentoSalvar.UsuarioID = usuario.Id;
					conteudoAtendedimentoSalvar.DataCriacao = DateTime.Now;
					conteudoAtendedimentoSalvar.ChamadoID = chamadoAtender.Id; // salva o chamado no 'conteudo'
					db.Conteudos.Add(conteudoAtendedimentoSalvar);

					db.Entry(chamadoAtender).State = EntityState.Modified;
				}
				else
				{
					return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
				}

				db.SaveChanges();
				if (usuario.TipoAcesso == EnumTipoUsuario.Desenvolvimento || usuario.TipoAcesso == EnumTipoUsuario.Suporte)
				{
					return RedirectToAction("Atender", new { id });
				} else
				{
					return RedirectToAction("Acompanhar", new { id });
				}
			}

			return View();
		}
	}
}
