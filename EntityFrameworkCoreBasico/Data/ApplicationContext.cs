using EntityFrameworkCoreBasico.Domain;
using Microsoft.EntityFrameworkCore;

namespace EntityFrameworkCoreBasico.Data
{
    internal class ApplicationContext : DbContext
    {
        public DbSet<Pedido> Pedidos { get; set; }
        public DbSet<Produto> Produtos { get; set; }
        public DbSet<Cliente> Clientes { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Data source=(localdb)\\mssqllocaldb;Initial Catalog=CursoEFCore;Integrated Security=true");
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Cliente>(builder =>
            {
                builder.ToTable("Clientes");
                builder.HasKey(p => p.Id);
                builder.Property(p => p.Nome).HasColumnType("VARCHAR(80)").IsRequired();
                builder.Property(p => p.Telefone).HasColumnType("CHAR(11)");
                builder.Property(p => p.CEP).HasColumnType("CHAR(8)").IsRequired();
                builder.Property(p => p.Estado).HasColumnType("CHAR(2)").IsRequired();
                builder.Property(p => p.Cidade).HasMaxLength(60).IsRequired();

                //Criação de Index na tabela
                builder.HasIndex(i => i.Telefone).HasName("idx_cliente_telefone");
            });

            modelBuilder.Entity<Produto>(builder =>
            {
                builder.ToTable("Produtos");
                builder.HasKey(p => p.Id);
                builder.Property(p => p.CodigoBarras).HasColumnType("VARCHAR(14)").IsRequired();
                builder.Property(p => p.Descricao).HasColumnType("VARCHAR(60)");
                builder.Property(p => p.Valor).IsRequired();
                builder.Property(p => p.TipoProduto).HasConversion<string>();
            });

            modelBuilder.Entity<Pedido>(builder =>
            {
                builder.ToTable("Pedidos");
                builder.HasKey(p => p.Id);
                //HasDefaultValueSql("GETDATE()") insere os dados já com data preenchida  
                builder.Property(p => p.IniciadoEm).HasDefaultValueSql("GETDATE()").ValueGeneratedOnAdd();
                builder.Property(p => p.Status).HasConversion<string>();
                builder.Property(p => p.TipoFrete).HasConversion<int>();
                builder.Property(p => p.Observacao).HasColumnType("VARCHAR(512)");

                //Aqui no HasMany é o relacionamento
                //OnDelete deleta todos os itens desse pedido quando o pedido é deletado
                builder.HasMany(p => p.Itens)
                    .WithOne(p => p.Pedido)
                    .OnDelete(DeleteBehavior.Cascade); 
            });
        }
    }
}
