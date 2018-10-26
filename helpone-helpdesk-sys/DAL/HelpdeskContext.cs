using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using helpone_helpdesk_sys.Models;
using helpone_helpdesk_sys.Models.Chamados;

namespace helpone_helpdesk_sys.DAL
{
	public class HelpDeskContext : DbContext
	{
		public HelpDeskContext() : base("HelpDeskContext")
		{
			Database.SetInitializer<HelpDeskContext>(new HelpDeskInitializer());
		}

		public DbSet<Topico> Topicos { get; set; }
		public DbSet<Subtopico> Subtopicos { get; set; }
		public DbSet<Chamado> Chamados { get; set; }
		public DbSet<Conteudo> Conteudos { get; set; }
		public DbSet<Feedback> Feedbacks { get; set; }
		public DbSet<Artigo> Artigos { get; set; }
		public DbSet<Usuario> Usuarios { get; set; }
		public DbSet<Equipe> Equipes { get; set; }

		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
		}
	}
}