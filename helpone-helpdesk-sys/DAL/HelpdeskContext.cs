using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using helpone_helpdesk_sys.Models;
using helpone_helpdesk_sys.Models.Chamados;
using helpone_helpdesk_sys.Models.Enums;

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


		public class HelpDeskInitializer : System.Data.Entity.DropCreateDatabaseIfModelChanges<HelpDeskContext>
		{
			protected override void Seed(HelpDeskContext context)
			{
				var equipes = new List<Equipe>
			{
				new Equipe { Id=1, OMSituada="Local do Teste", TipoEquipe=EnumTipoEquipe.Suporte },
				new Equipe { Id=2, OMSituada="Algum Lugar", TipoEquipe=EnumTipoEquipe.Desenvolvimento }
			};
				equipes.ForEach(eq => context.Equipes.Add(eq));
				context.SaveChanges();

				var usuarios = new List<Usuario>
			{
				new Usuario {Id=1, Login="user1", Senha="123", Daltonismo=true, TipoAcesso=EnumTipoUsuario.Operador, EquipeId=1},
				new Usuario {Id=2, Login="user2", Senha="123", Daltonismo=false, TipoAcesso=EnumTipoUsuario.Operador, EquipeId=2},
				new Usuario {Id=3, Login="user3", Senha="123", Daltonismo=false, TipoAcesso=EnumTipoUsuario.Operador, EquipeId=2},
				new Usuario {Id=4, Login="user4", Senha="123", Daltonismo=false, TipoAcesso=EnumTipoUsuario.Operador, EquipeId=1},
				new Usuario {Id=5, Login="user5", Senha="123", Daltonismo=true, TipoAcesso=EnumTipoUsuario.Operador, EquipeId=1}
			};
				usuarios.ForEach(user => context.Usuarios.Add(user));
				context.SaveChanges();

				var topicos = new List<Topico>
			{
				new Topico {Id=1, Titulo="Topico1", Descricao="alguma descricao"},
				new Topico {Id=2, Titulo="Topico2", Descricao="alguma descricao"}
			};
				topicos.ForEach(top => context.Topicos.Add(top));
				context.SaveChanges();

				var subtopicos = new List<Subtopico>
			{
				new Subtopico {Id=1, Titulo="Subtopico1", Descricao="alguma descricao", TopicoID=1},
				new Subtopico {Id=2, Titulo="Subtopico2", Descricao="alguma descricao", TopicoID=1},
				new Subtopico {Id=3, Titulo="Subtopico3", Descricao="alguma descricao", TopicoID=1},
				new Subtopico {Id=4, Titulo="Subtopico4", Descricao="alguma descricao", TopicoID=2},
				new Subtopico {Id=5, Titulo="Subtopico5", Descricao="alguma descricao", TopicoID=2},
				new Subtopico {Id=6, Titulo="Subtopico6", Descricao="alguma descricao", TopicoID=2}
			};
				subtopicos.ForEach(subt => context.Subtopicos.Add(subt));
				context.SaveChanges();

				//var chamados = new List<Chamado>
				//{
				//	new Chamado {Id=1, Titulo="Nome do chamado", }
				//	new Chamado {Id=1, Titulo="Nome do chamado", }
				//	new Chamado {Id=1, Titulo="Nome do chamado", }
				//};
			}
		}
	}
}