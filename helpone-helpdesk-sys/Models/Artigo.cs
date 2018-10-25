using System;
using System.Collections.Generic;
using helpone_helpdesk_sys.Models.Enums;
using helpone_helpdesk_sys.Models.Chamados;

namespace helpone_helpdesk_sys.Models
	public class Artigo
	{
		public Artigo() => this.ListaChamados = new List<Chamado>();

		public int Id { get; set; }
		public string TituloArtigo { get; set; }
		public string ConteudoArtigo { get; set; }
		public EnumStatus Status { get; set; }
		public DateTime DataCriacao { get; set; }
		public DateTime DataFim { get; set; }
		public int QtdLike { get; set; }
		public int QtdUnlike { get; set; }
		//public int QtdVisualizacao { get; set; }
		public int SubtopicoID { get; set; }
		public int UsuarioID { get; set; }

		public virtual Subtopico Subtopico { get; set; }
		public virtual Usuario Usuario { get; set; }
		public virtual ICollection<Chamado> ListaChamados { get; set; }
	}
}