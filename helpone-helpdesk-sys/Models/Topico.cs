using System.Collections.Generic;

namespace helpone_helpdesk_sys.Models
{
	public class Topico
	{
		public Topico() => this.ListaSubtopicos = new List<Subtopico>();

		public int Id { get; set; }
		public string Titulo { get; set; }
		public string Descricao { get; set; }

		public virtual ICollection<Subtopico> ListaSubtopicos { get; set; }
	}
}