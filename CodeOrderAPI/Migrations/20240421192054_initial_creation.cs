using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace CodeOrderAPI.Migrations
{
    /// <inheritdoc />
    public partial class initial_creation : Migration
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
                    Consumables = table.Column<TimeSpan>(type: "interval", nullable: false),
                    Class = table.Column<string>(type: "text", nullable: false)
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
                    Nome = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    RotationPeriod = table.Column<TimeSpan>(type: "interval", maxLength: 500, nullable: false),
                    OrbitalPeriod = table.Column<TimeSpan>(type: "interval", maxLength: 500, nullable: false),
                    Diameter = table.Column<decimal>(type: "numeric", nullable: false),
                    Climate = table.Column<string>(type: "text", nullable: false),
                    Gravity = table.Column<decimal>(type: "numeric", nullable: false),
                    Terrain = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    SurfaceWater = table.Column<int>(type: "integer", nullable: false),
                    Population = table.Column<long>(type: "bigint", nullable: false)
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
                name: "FilmeNave",
                columns: table => new
                {
                    MoviesId = table.Column<int>(type: "integer", nullable: false),
                    StarshipsId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FilmeNave", x => new { x.MoviesId, x.StarshipsId });
                    table.ForeignKey(
                        name: "FK_FilmeNave_Filme_MoviesId",
                        column: x => x.MoviesId,
                        principalTable: "Filme",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FilmeNave_Nave_StarshipsId",
                        column: x => x.StarshipsId,
                        principalTable: "Nave",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FilmePlaneta",
                columns: table => new
                {
                    MoviesId = table.Column<int>(type: "integer", nullable: false),
                    PlanetasId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FilmePlaneta", x => new { x.MoviesId, x.PlanetasId });
                    table.ForeignKey(
                        name: "FK_FilmePlaneta_Filme_MoviesId",
                        column: x => x.MoviesId,
                        principalTable: "Filme",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FilmePlaneta_Planeta_PlanetasId",
                        column: x => x.PlanetasId,
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
                    Nome = table.Column<string>(type: "text", nullable: false),
                    Height = table.Column<decimal>(type: "numeric", nullable: false),
                    Weight = table.Column<decimal>(type: "numeric", nullable: false),
                    HairColor = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    SkinColor = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    EyeColor = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    BirthYear = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Gender = table.Column<int>(type: "integer", nullable: false),
                    PlanetaId = table.Column<int>(type: "integer", nullable: false),
                    FilmeId = table.Column<int>(type: "integer", nullable: true),
                    PlanetaId1 = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Personagem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Personagem_Filme_FilmeId",
                        column: x => x.FilmeId,
                        principalTable: "Filme",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Personagem_Planeta_PlanetaId",
                        column: x => x.PlanetaId,
                        principalTable: "Planeta",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Personagem_Planeta_PlanetaId1",
                        column: x => x.PlanetaId1,
                        principalTable: "Planeta",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "FilmeVeiculo",
                columns: table => new
                {
                    MoviesId = table.Column<int>(type: "integer", nullable: false),
                    VeiculosId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FilmeVeiculo", x => new { x.MoviesId, x.VeiculosId });
                    table.ForeignKey(
                        name: "FK_FilmeVeiculo_Filme_MoviesId",
                        column: x => x.MoviesId,
                        principalTable: "Filme",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FilmeVeiculo_Veiculo_VeiculosId",
                        column: x => x.VeiculosId,
                        principalTable: "Veiculo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FilmeNave_StarshipsId",
                table: "FilmeNave",
                column: "StarshipsId");

            migrationBuilder.CreateIndex(
                name: "IX_FilmePlaneta_PlanetasId",
                table: "FilmePlaneta",
                column: "PlanetasId");

            migrationBuilder.CreateIndex(
                name: "IX_FilmeVeiculo_VeiculosId",
                table: "FilmeVeiculo",
                column: "VeiculosId");

            migrationBuilder.CreateIndex(
                name: "IX_Personagem_FilmeId",
                table: "Personagem",
                column: "FilmeId");

            migrationBuilder.CreateIndex(
                name: "IX_Personagem_PlanetaId",
                table: "Personagem",
                column: "PlanetaId");

            migrationBuilder.CreateIndex(
                name: "IX_Personagem_PlanetaId1",
                table: "Personagem",
                column: "PlanetaId1");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FilmeNave");

            migrationBuilder.DropTable(
                name: "FilmePlaneta");

            migrationBuilder.DropTable(
                name: "FilmeVeiculo");

            migrationBuilder.DropTable(
                name: "Personagem");

            migrationBuilder.DropTable(
                name: "Nave");

            migrationBuilder.DropTable(
                name: "Veiculo");

            migrationBuilder.DropTable(
                name: "Filme");

            migrationBuilder.DropTable(
                name: "Planeta");
        }
    }
}
