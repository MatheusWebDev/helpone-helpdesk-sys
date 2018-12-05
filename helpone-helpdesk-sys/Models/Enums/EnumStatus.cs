using System.ComponentModel;

namespace helpone_helpdesk_sys.Models.Enums
{
	public enum EnumStatus
	{
		[Description("Aguardando Resposta")]
		AguardandoResposta = 1,
		[Description("Chamado Respondido")]
		ChamadoRespondido = 2,
		[Description("Cancelado")]
		Cancelado = 3,
		[Description("Finalizado sem Feedback")]
		FinalizadoSemFeedback = 4,
		[Description("Finalizado c/ Feedback +")]
		FinalizadoFeedbackPositivo = 5,
		[Description("Finalizado c/ Feedback -")]
		FinalizadoFeedbackNegativo = 6
	}
}