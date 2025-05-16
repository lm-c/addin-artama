using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace AddinArtama {
  [DbConfigurationType(typeof(MySql.Data.EntityFramework.MySqlEFConfiguration))]
  internal class ContextoDados : DbContext {
    public ContextoDados() : base(ConexaoMySql.ConnectionString()) {
    }

    public DbSet<usuarios> usuarios { get; set; }
    public DbSet<perfis> perfis { get; set; }
    public DbSet<usuario_alocados> usuario_alocados { get; set; }
    public DbSet<materiais> materiais { get; set; }
    public DbSet<materia_primas> materia_primas { get; set; }
    public DbSet<templates> templates { get; set; }
    public DbSet<configuracao_api> configuracao_api { get; set; }
    public DbSet<produto_erp> produto_erp { get; set; }

    protected override void OnModelCreating(DbModelBuilder modelBuilder) {
      modelBuilder.Conventions
          .Remove<PluralizingTableNameConvention>();
    }
  }
}
