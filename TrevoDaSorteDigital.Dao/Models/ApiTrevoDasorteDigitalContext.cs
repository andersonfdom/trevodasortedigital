using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace TrevoDaSorteDigital.Dao.Models
{
    public partial class ApiTrevoDasorteDigitalContext : DbContext
    {
        public ApiTrevoDasorteDigitalContext()
        {
        }

        public ApiTrevoDasorteDigitalContext(DbContextOptions<ApiTrevoDasorteDigitalContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Afiliado> Afiliados { get; set; }
        public virtual DbSet<Aposta> Apostas { get; set; }
        public virtual DbSet<Cliente> Clientes { get; set; }
        public virtual DbSet<Estado> Estados { get; set; }
        public virtual DbSet<Loteria> Loterias { get; set; }
        public virtual DbSet<Numerosaposta> Numerosapostas { get; set; }
        public virtual DbSet<Numerostrevo> Numerostrevos { get; set; }
        public virtual DbSet<Registrocep> Registroceps { get; set; }
        public virtual DbSet<Usuario> Usuarios { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseMySql("server=177.136.228.216;database=apitrevodasortedigital;uid=apitrevodasortedigital;pwd=AOdXmwB0uD6r", Microsoft.EntityFrameworkCore.ServerVersion.Parse("10.11.8-mariadb"));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasCharSet("utf8mb3")
                .UseCollation("utf8mb3_general_ci");

            modelBuilder.Entity<Afiliado>(entity =>
            {
                entity.ToTable("afiliados");

                entity.Property(e => e.Id)
                    .HasColumnType("int(11)")
                    .HasColumnName("id");

                entity.Property(e => e.Ativo)
                    .HasColumnType("tinyint(4)")
                    .HasColumnName("ativo");

                entity.Property(e => e.Codafiliado).HasColumnName("codafiliado");

                entity.Property(e => e.Cpf)
                    .HasMaxLength(30)
                    .HasColumnName("cpf");

                entity.Property(e => e.Email)
                    .HasMaxLength(250)
                    .HasColumnName("email");

                entity.Property(e => e.Nome)
                    .HasMaxLength(250)
                    .HasColumnName("nome");

                entity.Property(e => e.Senha).HasColumnName("senha");

                entity.Property(e => e.Serialrecovery).HasColumnName("serialrecovery");

                entity.Property(e => e.Telefone)
                    .HasMaxLength(30)
                    .HasColumnName("telefone");

                entity.Property(e => e.Tokenacesso).HasColumnName("tokenacesso");

                entity.Property(e => e.Ultimoacesso)
                    .HasColumnType("datetime")
                    .HasColumnName("ultimoacesso");

                entity.Property(e => e.Usuariologado)
                    .HasColumnType("tinyint(4)")
                    .HasColumnName("usuariologado");
            });

            modelBuilder.Entity<Aposta>(entity =>
            {
                entity.ToTable("apostas");

                entity.HasCharSet("utf8mb4")
                    .UseCollation("utf8mb4_general_ci");

                entity.HasIndex(e => e.Idcliente, "idcliente");

                entity.HasIndex(e => e.Idloteria, "idloteria");

                entity.Property(e => e.Id)
                    .HasColumnType("int(11)")
                    .HasColumnName("id");

                entity.Property(e => e.Dataaposta)
                    .HasColumnType("datetime")
                    .HasColumnName("dataaposta");

                entity.Property(e => e.Idcliente)
                    .HasColumnType("int(11)")
                    .HasColumnName("idcliente");

                entity.Property(e => e.Idloteria)
                    .HasColumnType("int(11)")
                    .HasColumnName("idloteria");

                entity.Property(e => e.Mesdasorte)
                    .HasMaxLength(200)
                    .HasColumnName("mesdasorte");

                entity.Property(e => e.Timedocoracao)
                    .HasMaxLength(200)
                    .HasColumnName("timedocoracao");

                entity.HasOne(d => d.IdclienteNavigation)
                    .WithMany(p => p.Aposta)
                    .HasForeignKey(d => d.Idcliente)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("apostas_ibfk_2");

                entity.HasOne(d => d.IdloteriaNavigation)
                    .WithMany(p => p.Aposta)
                    .HasForeignKey(d => d.Idloteria)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("apostas_ibfk_1");
            });

            modelBuilder.Entity<Cliente>(entity =>
            {
                entity.ToTable("clientes");

                entity.HasCharSet("utf8mb4")
                    .UseCollation("utf8mb4_general_ci");

                entity.Property(e => e.Id)
                    .HasColumnType("int(11)")
                    .HasColumnName("id");

                entity.Property(e => e.Ativo)
                    .HasColumnType("tinyint(4)")
                    .HasColumnName("ativo")
                    .HasDefaultValueSql("'1'");

                entity.Property(e => e.Bairro)
                    .HasMaxLength(200)
                    .HasColumnName("bairro");

                entity.Property(e => e.Cep)
                    .HasMaxLength(30)
                    .HasColumnName("cep");

                entity.Property(e => e.Cidade)
                    .HasMaxLength(200)
                    .HasColumnName("cidade");

                entity.Property(e => e.Codafiliado).HasColumnName("codafiliado");

                entity.Property(e => e.Complemento)
                    .HasMaxLength(200)
                    .HasColumnName("complemento");

                entity.Property(e => e.Cpf)
                    .HasMaxLength(30)
                    .HasColumnName("cpf");

                entity.Property(e => e.Email)
                    .HasMaxLength(200)
                    .HasColumnName("email");

                entity.Property(e => e.Estado)
                    .HasMaxLength(200)
                    .HasColumnName("estado");

                entity.Property(e => e.Logradouro)
                    .HasMaxLength(200)
                    .HasColumnName("logradouro");

                entity.Property(e => e.Nome)
                    .HasMaxLength(200)
                    .HasColumnName("nome");

                entity.Property(e => e.Senha).HasColumnName("senha");

                entity.Property(e => e.Serialrecovery).HasColumnName("serialrecovery");

                entity.Property(e => e.Telefone)
                    .HasMaxLength(30)
                    .HasColumnName("telefone");

                entity.Property(e => e.Tokenacesso).HasColumnName("tokenacesso");

                entity.Property(e => e.Ultimoacesso)
                    .HasColumnType("datetime")
                    .HasColumnName("ultimoacesso");

                entity.Property(e => e.Usuariologado)
                    .HasColumnType("tinyint(4)")
                    .HasColumnName("usuariologado")
                    .HasDefaultValueSql("'0'");
            });

            modelBuilder.Entity<Estado>(entity =>
            {
                entity.ToTable("estados");

                entity.HasCharSet("utf8mb4")
                    .UseCollation("utf8mb4_general_ci");

                entity.Property(e => e.Id)
                    .HasColumnType("int(11)")
                    .ValueGeneratedNever()
                    .HasColumnName("id");

                entity.Property(e => e.Nome)
                    .HasMaxLength(500)
                    .HasColumnName("nome");

                entity.Property(e => e.Uf)
                    .HasMaxLength(2)
                    .HasColumnName("uf");
            });

            modelBuilder.Entity<Loteria>(entity =>
            {
                entity.ToTable("loterias");

                entity.Property(e => e.Id)
                    .HasColumnType("int(11)")
                    .HasColumnName("id");

                entity.Property(e => e.Descricao)
                    .HasMaxLength(255)
                    .HasColumnName("descricao");

                entity.Property(e => e.Nome)
                    .HasMaxLength(255)
                    .HasColumnName("nome");
            });

            modelBuilder.Entity<Numerosaposta>(entity =>
            {
                entity.ToTable("numerosapostas");

                entity.HasCharSet("utf8mb4")
                    .UseCollation("utf8mb4_general_ci");

                entity.HasIndex(e => e.Idaposta, "idaposta");

                entity.Property(e => e.Id)
                    .HasColumnType("int(11)")
                    .HasColumnName("id");

                entity.Property(e => e.Dezena)
                    .HasColumnType("int(11)")
                    .HasColumnName("dezena");

                entity.Property(e => e.Idaposta)
                    .HasColumnType("int(11)")
                    .HasColumnName("idaposta");

                entity.Property(e => e.Numerodezena1)
                    .HasColumnType("int(11)")
                    .HasColumnName("numerodezena1");

                entity.Property(e => e.Numerodezena2)
                    .HasColumnType("int(11)")
                    .HasColumnName("numerodezena2");

                entity.Property(e => e.Numerodezena3)
                    .HasColumnType("int(11)")
                    .HasColumnName("numerodezena3");

                entity.Property(e => e.Numerodezena4)
                    .HasColumnType("int(11)")
                    .HasColumnName("numerodezena4");

                entity.Property(e => e.Numerodezena5)
                    .HasColumnType("int(11)")
                    .HasColumnName("numerodezena5");

                entity.Property(e => e.Numerodezena6)
                    .HasColumnType("int(11)")
                    .HasColumnName("numerodezena6");

                entity.Property(e => e.Numerodezena7)
                    .HasColumnType("int(11)")
                    .HasColumnName("numerodezena7");

                entity.Property(e => e.Sorteadonumerodezena1)
                    .HasColumnType("tinyint(4)")
                    .HasColumnName("sorteadonumerodezena1")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.Sorteadonumerodezena2)
                    .HasColumnType("tinyint(4)")
                    .HasColumnName("sorteadonumerodezena2")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.Sorteadonumerodezena3)
                    .HasColumnType("tinyint(4)")
                    .HasColumnName("sorteadonumerodezena3")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.Sorteadonumerodezena4)
                    .HasColumnType("tinyint(4)")
                    .HasColumnName("sorteadonumerodezena4")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.Sorteadonumerodezena5)
                    .HasColumnType("tinyint(4)")
                    .HasColumnName("sorteadonumerodezena5")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.Sorteadonumerodezena6)
                    .HasColumnType("tinyint(4)")
                    .HasColumnName("sorteadonumerodezena6")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.Sorteadonumerodezena7)
                    .HasColumnType("tinyint(4)")
                    .HasColumnName("sorteadonumerodezena7")
                    .HasDefaultValueSql("'0'");

                entity.HasOne(d => d.IdapostaNavigation)
                    .WithMany(p => p.Numerosaposta)
                    .HasForeignKey(d => d.Idaposta)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("numerosapostas_ibfk_1");
            });

            modelBuilder.Entity<Numerostrevo>(entity =>
            {
                entity.ToTable("numerostrevos");

                entity.HasCharSet("utf8mb4")
                    .UseCollation("utf8mb4_general_ci");

                entity.HasIndex(e => e.Idaposta, "idaposta");

                entity.Property(e => e.Id)
                    .HasColumnType("int(11)")
                    .HasColumnName("id");

                entity.Property(e => e.Dezena)
                    .HasColumnType("int(11)")
                    .HasColumnName("dezena");

                entity.Property(e => e.Idaposta)
                    .HasColumnType("int(11)")
                    .HasColumnName("idaposta");

                entity.Property(e => e.Numerodezena)
                    .HasColumnType("int(11)")
                    .HasColumnName("numerodezena");

                entity.Property(e => e.Sorteadonumerodezena)
                    .HasColumnType("tinyint(4)")
                    .HasColumnName("sorteadonumerodezena")
                    .HasDefaultValueSql("'0'");

                entity.HasOne(d => d.IdapostaNavigation)
                    .WithMany(p => p.Numerostrevos)
                    .HasForeignKey(d => d.Idaposta)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("numerostrevos_ibfk_1");
            });

            modelBuilder.Entity<Registrocep>(entity =>
            {
                entity.ToTable("registrocep");

                entity.Property(e => e.Id)
                    .HasColumnType("int(11)")
                    .HasColumnName("id");

                entity.Property(e => e.Bairro)
                    .HasMaxLength(250)
                    .HasColumnName("bairro");

                entity.Property(e => e.Cep)
                    .HasMaxLength(30)
                    .HasColumnName("cep");

                entity.Property(e => e.Localidade)
                    .HasMaxLength(250)
                    .HasColumnName("localidade");

                entity.Property(e => e.Logradouro)
                    .HasMaxLength(250)
                    .HasColumnName("logradouro");

                entity.Property(e => e.Uf)
                    .HasMaxLength(250)
                    .HasColumnName("uf");
            });

            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.ToTable("usuarios");

                entity.Property(e => e.Id)
                    .HasColumnType("int(11)")
                    .HasColumnName("id");

                entity.Property(e => e.Email)
                    .HasMaxLength(250)
                    .HasColumnName("email");

                entity.Property(e => e.Senha).HasColumnName("senha");

                entity.Property(e => e.Serialrecovery).HasColumnName("serialrecovery");

                entity.Property(e => e.Tokenacesso).HasColumnName("tokenacesso");

                entity.Property(e => e.Ultimoacesso)
                    .HasColumnType("datetime")
                    .HasColumnName("ultimoacesso");

                entity.Property(e => e.Usuariologado)
                    .HasColumnType("tinyint(4)")
                    .HasColumnName("usuariologado");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
