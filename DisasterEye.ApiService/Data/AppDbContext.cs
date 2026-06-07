using DisasterEye.ApiService.Models;
using Microsoft.EntityFrameworkCore;

namespace DisasterEye.ApiService.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Tecnologia> Tecnologias { get; set; }
    public DbSet<CategoriaImpacto> CategoriasImpacto { get; set; }
    public DbSet<Usuario> Usuarios { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // ── CategoriaImpacto ────────────────────────────────────
        modelBuilder.Entity<CategoriaImpacto>(entity =>
        {
            entity.ToTable("CATEGORIASIMPACTO");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id)
                  .HasColumnName("ID")
                  .UseIdentityColumn(); // Oracle GENERATED AS IDENTITY
        });

        // ── Tecnologia ──────────────────────────────────────────
        modelBuilder.Entity<Tecnologia>(entity =>
        {
            entity.ToTable("TECNOLOGIAS");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id)
                  .HasColumnName("ID")
                  .UseIdentityColumn();

            entity.HasOne(e => e.CategoriaImpacto)
                  .WithMany(c => c.Tecnologias)
                  .HasForeignKey(e => e.CategoriaImpactoId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        // ── Usuario ─────────────────────────────────────────────
        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.ToTable("USUARIOS");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id)
                  .HasColumnName("ID")
                  .UseIdentityColumn();
            entity.HasIndex(e => e.Email).IsUnique();
        });

        // ── Seed: Categorias ────────────────────────────────────
        modelBuilder.Entity<CategoriaImpacto>().HasData(
            new CategoriaImpacto { Id = 1, Nome = "Incêndios Florestais", Descricao = "Monitoramento de incêndios via satélite", TipoDesastre = "Wildfires" },
            new CategoriaImpacto { Id = 2, Nome = "Inundações",           Descricao = "Detecção de cheias e alagamentos",        TipoDesastre = "Floods" },
            new CategoriaImpacto { Id = 3, Nome = "Terremotos",           Descricao = "Análise sísmica via sensores espaciais",   TipoDesastre = "Earthquakes" },
            new CategoriaImpacto { Id = 4, Nome = "Vulcões",              Descricao = "Monitoramento de atividade vulcânica",     TipoDesastre = "Volcanoes" },
            new CategoriaImpacto { Id = 5, Nome = "Tempestades Severas",  Descricao = "Rastreamento de furacões e tempestades",   TipoDesastre = "Severe Storms" }
        );

        // ── Seed: Admin padrão (senha: Admin@123) ───────────────
        modelBuilder.Entity<Usuario>().HasData(new Usuario
        {
            Id = 1,
            Nome = "Administrador",
            Email = "admin@disastereye.com",
            SenhaHash = BCrypt.Net.BCrypt.HashPassword("Admin@123"),
            Perfil = "Administrador",
            DataCadastro = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc),
            Ativo = 1
        });

        // ── Seed: Tecnologias de exemplo ────────────────────────
        modelBuilder.Entity<Tecnologia>().HasData(
            new Tecnologia
            {
                Id = 1, Nome = "EONET Fire Tracker",
                Descricao = "Rastreamento de incêndios usando a API EONET da NASA com coordenadas de satélite em tempo real.",
                OrigemEspacial = "NASA EONET", Latitude = -15.78, Longitude = -47.93,
                Status = "Ativo", CategoriaImpactoId = 1,
                DataCadastro = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            },
            new Tecnologia
            {
                Id = 2, Nome = "Sentinel Flood Monitor",
                Descricao = "Monitoramento de inundações por imagens SAR do satélite Sentinel-1 da ESA.",
                OrigemEspacial = "ESA Sentinel-1", Latitude = -23.55, Longitude = -46.63,
                Status = "Monitorando", CategoriaImpactoId = 2,
                DataCadastro = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            },
            new Tecnologia
            {
                Id = 3, Nome = "GPS Seismic Array",
                Descricao = "Rede de sensores sísmicos derivados de tecnologia GPS espacial para detecção de terremotos na Amazônia.",
                OrigemEspacial = "GPS/GNSS Satellites", Latitude = -3.10, Longitude = -60.02,
                Status = "Alerta", CategoriaImpactoId = 3,
                DataCadastro = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            },
            new Tecnologia
            {
                Id = 4, Nome = "GOES-16 Storm Eye",
                Descricao = "Rastreamento de tempestades severas no Atlântico Sul via satélite geoestacionário GOES-16 da NOAA.",
                OrigemEspacial = "NOAA GOES-16", Latitude = -10.00, Longitude = -35.00,
                Status = "Monitorando", CategoriaImpactoId = 5,
                DataCadastro = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            },
            new Tecnologia
            {
                Id = 5, Nome = "Volcano SAR Monitor",
                Descricao = "Monitoramento de deformações vulcânicas via interferometria SAR dos satélites Sentinel-2.",
                OrigemEspacial = "ESA Sentinel-2", Latitude = -22.44, Longitude = -45.85,
                Status = "Ativo", CategoriaImpactoId = 4,
                DataCadastro = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            }
        );
    }
}
