using System.Collections.Generic;

namespace helpone_helpdesk_sys.Models
{
	public class Equipe
	{
		public Equipe() => this.ListaMembros = new List<Usuario>(); //construtor

		public int Id { get; set; }
		public string OMSituada { get; set; }

		public virtual ICollection<Usuario> ListaMembros { get; set; }
	}
}