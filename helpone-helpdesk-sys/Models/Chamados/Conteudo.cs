using System;
using System.Collections.Generic;

namespace helpone_helpdesk_sys.Models.Chamados
{
	public class Conteudo
	{
		public int ID { get; set; } // talves retirar
		public string ConteudoChamado { get; set; }
		public List<string> Anexos { get; set; }
		public String DataCriacao { get; set; }
		public int ChamadoID { get; set; }
		public int? UsuarioID { get; set; }

		public virtual Chamado Chamado { get; set; }
		public virtual Usuario Usuario { get; set; }
	}
}