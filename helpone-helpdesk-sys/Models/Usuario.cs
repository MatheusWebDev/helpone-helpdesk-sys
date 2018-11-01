using helpone_helpdesk_sys.Models.Enums;

namespace helpone_helpdesk_sys.Models
{
	public class Usuario
	{
		public int Id { get; set; }
		public string Login { get; set; }
		public string Senha { get; set; }
		public bool Daltonismo { get; set; }
		public EnumTipoUsuario TipoAcesso { get; set; }
		public int? EquipeId { get; set; }

		public virtual Equipe Equipe { get; set; }
	}
}