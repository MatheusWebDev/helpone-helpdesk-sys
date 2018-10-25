using helpone_helpdesk_sys.Models;
using System.Collections.Generic;

namespace helpone_helpdesk_sys.DAL
{
	public class HelpDeskInitializer : System.Data.Entity.DropCreateDatabaseIfModelChanges<HelpDeskContext>
	{
		protected override void Seed(HelpDeskContext context)
		{
			var usuarios = new List<Usuario>
			{

			};
		}
	}
}