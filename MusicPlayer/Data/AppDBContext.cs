using Microsoft.EntityFrameworkCore;
using MusicPlayer.Models;

namespace MusicPlayer.Data
{
    public class AppDBContext : DbContext
    {
        public AppDBContext(DbContextOptions<AppDBContext> options) : base(options) { 
        

        }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Cancion> Canciones { get; set; }
        public DbSet<Playlist> Playlists { get; set; }
        public DbSet<PlaylistCancion> PlaylistCanciones { get; set; }
        public DbSet<Favorito> Favoritos { get; set; }
        public DbSet<HistorialReproduccion> HistorialReproducciones { get; set; }
        public DbSet<PlaylistAleatoria> PlaylistAleatorias { get; set; }
        public DbSet<PlaylistAleatoriaCancion> PlaylistAleatoriaCanciones { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // 1. TABLA USUARIO
            modelBuilder.Entity<Usuario>().ToTable("Usuario");
            modelBuilder.Entity<Usuario>(tb =>
            {
                tb.HasKey(col => col.UsuarioID);
                tb.Property(col => col.UsuarioID)
                  .UseIdentityColumn()
                  .ValueGeneratedOnAdd();

                tb.Property(col => col.NombreUsuario).IsRequired().HasMaxLength(50);
                tb.Property(col => col.CorreoElectronico).IsRequired().HasMaxLength(100);
                tb.Property(col => col.Contrasena).IsRequired().HasMaxLength(255);
                tb.Property(col => col.FechaRegistro).IsRequired();
            });

            // 2. TABLA CANCION
            modelBuilder.Entity<Cancion>().ToTable("Cancion");
            modelBuilder.Entity<Cancion>(tb =>
            {
                tb.HasKey(col => col.CancionID);
                tb.Property(col => col.CancionID)
                  .UseIdentityColumn()
                  .ValueGeneratedOnAdd();

                tb.Property(col => col.Titulo).IsRequired().HasMaxLength(100);
                tb.Property(col => col.Artista).IsRequired().HasMaxLength(100);
                tb.Property(col => col.RutaArchivo).IsRequired().HasMaxLength(255);
                tb.Property(col => col.FechaAgregado).IsRequired();

              
            });

         

            //4. TABLA PLAYLIST
            modelBuilder.Entity<Playlist>().ToTable("Playlist");
            modelBuilder.Entity<Playlist>(tb =>
            {
                tb.HasKey(col => col.PlaylistID);
                tb.Property(col => col.PlaylistID)
                  .UseIdentityColumn()
                  .ValueGeneratedOnAdd();

                tb.Property(col => col.Nombre).IsRequired().HasMaxLength(100);
                tb.Property(col => col.FechaCreacion).IsRequired();

                tb.HasOne(p => p.Usuario)
                  .WithMany(u => u.Playlists)
                  .HasForeignKey(p => p.UsuarioID)
                  .OnDelete(DeleteBehavior.Cascade);
            });

            //5. TABLA PLAYLISTCANCION
            modelBuilder.Entity<PlaylistCancion>().ToTable("PlaylistCancion");
            modelBuilder.Entity<PlaylistCancion>(tb =>
            {
                tb.HasKey(pc => new { pc.PlaylistID, pc.CancionID });

                tb.HasOne(pc => pc.Playlist)
                  .WithMany(p => p.PlaylistCanciones)
                  .HasForeignKey(pc => pc.PlaylistID)
                  .OnDelete(DeleteBehavior.Cascade);

                tb.HasOne(pc => pc.Cancion)
                  .WithMany(c => c.PlaylistCanciones)
                  .HasForeignKey(pc => pc.CancionID)
                  .OnDelete(DeleteBehavior.Cascade);
            });

            //6. TABLAFAVORITO
            modelBuilder.Entity<Favorito>().ToTable("Favorito");
            modelBuilder.Entity<Favorito>(tb =>
            {
                tb.HasKey(f => new { f.UsuarioID, f.CancionID });

                tb.HasOne(f => f.Usuario)
                  .WithMany(u => u.Favoritos)
                  .HasForeignKey(f => f.UsuarioID)
                  .OnDelete(DeleteBehavior.Cascade);

                tb.HasOne(f => f.Cancion)
                  .WithMany(c => c.Favoritos)
                  .HasForeignKey(f => f.CancionID)
                  .OnDelete(DeleteBehavior.Cascade);
            });

            //7. TABLA HISTORAILDEREPRODUCCION
            modelBuilder.Entity<HistorialReproduccion>().ToTable("HistorialReproduccion");
            modelBuilder.Entity<HistorialReproduccion>(tb =>
            {
                tb.HasKey(h => h.HistorialID);
                tb.Property(col => col.HistorialID)
                  .UseIdentityColumn()
                  .ValueGeneratedOnAdd();

                tb.Property(col => col.FechaReproduccion).IsRequired();

                tb.HasOne(h => h.Usuario)
                  .WithMany(u => u.HistorialReproduccion)
                  .HasForeignKey(h => h.UsuarioID)
                  .OnDelete(DeleteBehavior.Cascade);

                tb.HasOne(h => h.Cancion)
                  .WithMany(c => c.HistorialReproduccion)
                  .HasForeignKey(h => h.CancionID)
                  .OnDelete(DeleteBehavior.Cascade);
            });

            //8. TABLA PLAYLIST ALEATORIA
            modelBuilder.Entity<PlaylistAleatoria>().ToTable("PlaylistAleatoria");
            modelBuilder.Entity<PlaylistAleatoria>(tb =>
            {
                tb.HasKey(col => col.PlaylistID);
                tb.Property(col => col.PlaylistID)
                  .UseIdentityColumn()
                  .ValueGeneratedOnAdd();

                tb.Property(col => col.Nombre).IsRequired().HasMaxLength(100);
                tb.Property(col => col.FechaCreacion).IsRequired();
            });

            //9. TABLA CANCIONENPLAYLISTALEATORIA
            modelBuilder.Entity<PlaylistAleatoriaCancion>().ToTable("PlaylistAleatoriaCancion");
            modelBuilder.Entity<PlaylistAleatoriaCancion>(tb =>
            {
                tb.HasKey(pac => new { pac.PlaylistID, pac.CancionID });

                tb.HasOne(pac => pac.PlaylistAleatoria)
                  .WithMany(p => p.PlaylistAleatoriaCanciones)
                  .HasForeignKey(pac => pac.PlaylistID)
                  .OnDelete(DeleteBehavior.Cascade);

                tb.HasOne(pac => pac.Cancion)
                  .WithMany(c => c.PlaylistAleatoriaCanciones)
                  .HasForeignKey(pac => pac.CancionID)
                  .OnDelete(DeleteBehavior.Cascade);
            });
        }


    }
}
