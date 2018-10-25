using System.Collections.Generic;
using helpone_helpdesk_sys.Models.Chamados;

namespace helpone_helpdesk_sys.Models
{
	public class Subtopico
	{
		public Subtopico() => this.ListaChamados = new List<Chamado>();

		public int Id { get; set; }
		public string TituloSubtopico { get; set; }
		public string Descricao { get; set; }
		public int TopicoID { get; set; }

		public virtual Topico Topico { get; set; }
		public virtual ICollection<Chamado> ListaChamados { get; set; }
	}
}