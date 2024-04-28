using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace CodeOrderAPI.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Filme",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Title = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    Episode = table.Column<int>(type: "integer", nullable: false),
                    OpeningCrawl = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    Director = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    Producer = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    ReleaseDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Filme", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Nave",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    Model = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    Manufacturer = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    CostInCredits = table.Column<decimal>(type: "numeric", nullable: false),
                    Length = table.Column<decimal>(type: "numeric", nullable: false),
                    MaxSpeed = table.Column<decimal>(type: "numeric", nullable: false),
                    Crew = table.Column<int>(type: "integer", nullable: false),
                    Passengers = table.Column<int>(type: "integer", nullable: false),
                    CargoCapacity = table.Column<decimal>(type: "numeric", nullable: false),
                    HyperdriveRating = table.Column<decimal>(type: "numeric", nullable: false),
                    Mglt = table.Column<int>(type: "integer", nullable: false),
                    Consumables = table.Column<TimeSpan>(type: "interval", maxLength: 500, nullable: false),
                    Class = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Nave", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Planeta",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    RotationPeriod = table.Column<TimeSpan>(type: "interval", maxLength: 500, nullable: false),
                    OrbitalPeriod = table.Column<TimeSpan>(type: "interval", maxLength: 500, nullable: false),
                    Diameter = table.Column<decimal>(type: "numeric", maxLength: 500, nullable: false),
                    Climate = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    Gravity = table.Column<decimal>(type: "numeric", maxLength: 500, nullable: false),
                    Terrain = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    SurfaceWater = table.Column<int>(type: "integer", maxLength: 500, nullable: false),
                    Population = table.Column<long>(type: "bigint", maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Planeta", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Veiculo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    Model = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    Manufacturer = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    CostInCredits = table.Column<decimal>(type: "numeric", nullable: false),
                    Length = table.Column<decimal>(type: "numeric", nullable: false),
                    MaxSpeed = table.Column<decimal>(type: "numeric", nullable: false),
                    Crew = table.Column<int>(type: "integer", nullable: false),
                    Passengers = table.Column<int>(type: "integer", nullable: false),
                    CargoCapacity = table.Column<decimal>(type: "numeric", nullable: false),
                    Consumables = table.Column<TimeSpan>(type: "interval", nullable: false),
                    Class = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Veiculo", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FilmesNaves",
                columns: table => new
                {
                    MoviesId = table.Column<int>(type: "integer", nullable: false),
                    StarshipsId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FilmesNaves", x => new { x.MoviesId, x.StarshipsId });
                    table.ForeignKey(
                        name: "FK_FilmesNaves_Filme_MoviesId",
                        column: x => x.MoviesId,
                        principalTable: "Filme",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FilmesNaves_Nave_StarshipsId",
                        column: x => x.StarshipsId,
                        principalTable: "Nave",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FilmesPlanets",
                columns: table => new
                {
                    MoviesId = table.Column<int>(type: "integer", nullable: false),
                    PlanetsId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FilmesPlanets", x => new { x.MoviesId, x.PlanetsId });
                    table.ForeignKey(
                        name: "FK_FilmesPlanets_Filme_MoviesId",
                        column: x => x.MoviesId,
                        principalTable: "Filme",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FilmesPlanets_Planeta_PlanetsId",
                        column: x => x.PlanetsId,
                        principalTable: "Planeta",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Personagem",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    Height = table.Column<decimal>(type: "numeric", nullable: false),
                    Weight = table.Column<decimal>(type: "numeric", nullable: false),
                    HairColor = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    SkinColor = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    EyeColor = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    BirthYear = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Gender = table.Column<int>(type: "integer", nullable: false),
                    PlanetId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Personagem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Personagem_Planeta_PlanetId",
                        column: x => x.PlanetId,
                        principalTable: "Planeta",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FilmesVeiculos",
                columns: table => new
                {
                    MoviesId = table.Column<int>(type: "integer", nullable: false),
                    VeichlesId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FilmesVeiculos", x => new { x.MoviesId, x.VeichlesId });
                    table.ForeignKey(
                        name: "FK_FilmesVeiculos_Filme_MoviesId",
                        column: x => x.MoviesId,
                        principalTable: "Filme",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FilmesVeiculos_Veiculo_VeichlesId",
                        column: x => x.VeichlesId,
                        principalTable: "Veiculo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FilmesPersonagens",
                columns: table => new
                {
                    CharactersId = table.Column<int>(type: "integer", nullable: false),
                    MoviesId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FilmesPersonagens", x => new { x.CharactersId, x.MoviesId });
                    table.ForeignKey(
                        name: "FK_FilmesPersonagens_Filme_MoviesId",
                        column: x => x.MoviesId,
                        principalTable: "Filme",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FilmesPersonagens_Personagem_CharactersId",
                        column: x => x.CharactersId,
                        principalTable: "Personagem",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FilmesNaves_StarshipsId",
                table: "FilmesNaves",
                column: "StarshipsId");

            migrationBuilder.CreateIndex(
                name: "IX_FilmesPersonagens_MoviesId",
                table: "FilmesPersonagens",
                column: "MoviesId");

            migrationBuilder.CreateIndex(
                name: "IX_FilmesPlanets_PlanetsId",
                table: "FilmesPlanets",
                column: "PlanetsId");

            migrationBuilder.CreateIndex(
                name: "IX_FilmesVeiculos_VeichlesId",
                table: "FilmesVeiculos",
                column: "VeichlesId");

            migrationBuilder.CreateIndex(
                name: "IX_Personagem_PlanetId",
                table: "Personagem",
                column: "PlanetId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FilmesNaves");

            migrationBuilder.DropTable(
                name: "FilmesPersonagens");

            migrationBuilder.DropTable(
                name: "FilmesPlanets");

            migrationBuilder.DropTable(
                name: "FilmesVeiculos");

            migrationBuilder.DropTable(
                name: "Nave");

            migrationBuilder.DropTable(
                name: "Personagem");

            migrationBuilder.DropTable(
                name: "Filme");

            migrationBuilder.DropTable(
                name: "Veiculo");

            migrationBuilder.DropTable(
                name: "Planeta");
        }
    }
}
