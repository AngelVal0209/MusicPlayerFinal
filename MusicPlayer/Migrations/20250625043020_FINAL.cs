using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MusicPlayer.Migrations
{
    /// <inheritdoc />
    public partial class FINAL : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Cancion",
                columns: table => new
                {
                    CancionID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Titulo = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Artista = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    RutaArchivo = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    DuracionSegundos = table.Column<int>(type: "int", nullable: true),
                    FechaAgregado = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cancion", x => x.CancionID);
                });

            migrationBuilder.CreateTable(
                name: "PlaylistAleatoria",
                columns: table => new
                {
                    PlaylistID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlaylistAleatoria", x => x.PlaylistID);
                });

            migrationBuilder.CreateTable(
                name: "Usuario",
                columns: table => new
                {
                    UsuarioID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NombreUsuario = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CorreoElectronico = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Contrasena = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    FechaNacimiento = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Genero = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FechaRegistro = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuario", x => x.UsuarioID);
                });

            migrationBuilder.CreateTable(
                name: "PlaylistAleatoriaCancion",
                columns: table => new
                {
                    PlaylistID = table.Column<int>(type: "int", nullable: false),
                    CancionID = table.Column<int>(type: "int", nullable: false),
                    FechaAgregado = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlaylistAleatoriaCancion", x => new { x.PlaylistID, x.CancionID });
                    table.ForeignKey(
                        name: "FK_PlaylistAleatoriaCancion_Cancion_CancionID",
                        column: x => x.CancionID,
                        principalTable: "Cancion",
                        principalColumn: "CancionID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PlaylistAleatoriaCancion_PlaylistAleatoria_PlaylistID",
                        column: x => x.PlaylistID,
                        principalTable: "PlaylistAleatoria",
                        principalColumn: "PlaylistID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Favorito",
                columns: table => new
                {
                    UsuarioID = table.Column<int>(type: "int", nullable: false),
                    CancionID = table.Column<int>(type: "int", nullable: false),
                    FechaAgregado = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Favorito", x => new { x.UsuarioID, x.CancionID });
                    table.ForeignKey(
                        name: "FK_Favorito_Cancion_CancionID",
                        column: x => x.CancionID,
                        principalTable: "Cancion",
                        principalColumn: "CancionID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Favorito_Usuario_UsuarioID",
                        column: x => x.UsuarioID,
                        principalTable: "Usuario",
                        principalColumn: "UsuarioID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HistorialReproduccion",
                columns: table => new
                {
                    HistorialID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UsuarioID = table.Column<int>(type: "int", nullable: false),
                    CancionID = table.Column<int>(type: "int", nullable: false),
                    FechaReproduccion = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HistorialReproduccion", x => x.HistorialID);
                    table.ForeignKey(
                        name: "FK_HistorialReproduccion_Cancion_CancionID",
                        column: x => x.CancionID,
                        principalTable: "Cancion",
                        principalColumn: "CancionID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_HistorialReproduccion_Usuario_UsuarioID",
                        column: x => x.UsuarioID,
                        principalTable: "Usuario",
                        principalColumn: "UsuarioID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Playlist",
                columns: table => new
                {
                    PlaylistID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UsuarioID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Playlist", x => x.PlaylistID);
                    table.ForeignKey(
                        name: "FK_Playlist_Usuario_UsuarioID",
                        column: x => x.UsuarioID,
                        principalTable: "Usuario",
                        principalColumn: "UsuarioID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PlaylistCancion",
                columns: table => new
                {
                    PlaylistID = table.Column<int>(type: "int", nullable: false),
                    CancionID = table.Column<int>(type: "int", nullable: false),
                    FechaAgregado = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlaylistCancion", x => new { x.PlaylistID, x.CancionID });
                    table.ForeignKey(
                        name: "FK_PlaylistCancion_Cancion_CancionID",
                        column: x => x.CancionID,
                        principalTable: "Cancion",
                        principalColumn: "CancionID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PlaylistCancion_Playlist_PlaylistID",
                        column: x => x.PlaylistID,
                        principalTable: "Playlist",
                        principalColumn: "PlaylistID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Favorito_CancionID",
                table: "Favorito",
                column: "CancionID");

            migrationBuilder.CreateIndex(
                name: "IX_HistorialReproduccion_CancionID",
                table: "HistorialReproduccion",
                column: "CancionID");

            migrationBuilder.CreateIndex(
                name: "IX_HistorialReproduccion_UsuarioID",
                table: "HistorialReproduccion",
                column: "UsuarioID");

            migrationBuilder.CreateIndex(
                name: "IX_Playlist_UsuarioID",
                table: "Playlist",
                column: "UsuarioID");

            migrationBuilder.CreateIndex(
                name: "IX_PlaylistAleatoriaCancion_CancionID",
                table: "PlaylistAleatoriaCancion",
                column: "CancionID");

            migrationBuilder.CreateIndex(
                name: "IX_PlaylistCancion_CancionID",
                table: "PlaylistCancion",
                column: "CancionID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Favorito");

            migrationBuilder.DropTable(
                name: "HistorialReproduccion");

            migrationBuilder.DropTable(
                name: "PlaylistAleatoriaCancion");

            migrationBuilder.DropTable(
                name: "PlaylistCancion");

            migrationBuilder.DropTable(
                name: "PlaylistAleatoria");

            migrationBuilder.DropTable(
                name: "Cancion");

            migrationBuilder.DropTable(
                name: "Playlist");

            migrationBuilder.DropTable(
                name: "Usuario");
        }
    }
}
