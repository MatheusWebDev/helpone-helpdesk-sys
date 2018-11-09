using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using helpone_helpdesk_sys.Models.Enums;

namespace helpone_helpdesk_sys.Models.Chamados
{
	public class Chamado
	{
		public Chamado() => this.Conteudos = new List<Conteudo>(); //construtor

		public int Id { get; set; }
		public string Titulo { get; set; }
		public EnumStatus Status { get; set; }
		public EnumTipoEquipe EquipeAtendimento { get; set; }
		[DataType(DataType.DateTime)]
		[Column(TypeName = "datetime2")]
		[DisplayFormat(DataFormatString = "{0:dd/MM/yyyy - HH:mm}", ApplyFormatInEditMode = true)]
		public DateTime DataCriacao { get; set; }
		[DataType(DataType.DateTime)]
		[Column(TypeName = "datetime2")]
		[DisplayFormat(DataFormatString = "{0:dd/MM/yyyy - HH:mm}", ApplyFormatInEditMode = true)]
		public DateTime DataFim { get; set; }
		public int SubtopicoID { get; set; }
		public int UsuarioID { get; set; }
		public int FeedbackID { get; set; }

		public virtual Usuario Usuario { get; set; }
		public virtual Feedback Feedback { get; set; }
		public virtual Subtopico Subtopico { get; set; }
		public virtual ICollection<Conteudo> Conteudos { get; set; }
	}
}