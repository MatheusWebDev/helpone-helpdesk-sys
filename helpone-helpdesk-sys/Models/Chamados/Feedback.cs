﻿using System;
using System.ComponentModel.DataAnnotations;

namespace helpone_helpdesk_sys.Models.Chamados
{
	public class Feedback
	{
		public int Id { get; set; }
		public bool SolucaoFoiUtil { get; set; }
		public int NivelSatisfacao { get; set; }
		public string Mensagem { get; set; }
		public String DataCriacao { get; set; }
		public int ChamadoID { get; set; }
		public int? UsuarioID { get; set; }

		[Required]
		public virtual Chamado Chamado { get; set; }
		public virtual Usuario Usuario { get; set; }
	}
}