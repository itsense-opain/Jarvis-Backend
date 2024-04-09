using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Opain.Jarvis.Infraestructura.Datos.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Accesos",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Grupo = table.Column<string>(maxLength: 45, nullable: false),
                    Rol = table.Column<string>(maxLength: 45, nullable: false),
                    NombreUsuario = table.Column<string>(maxLength: 150, nullable: false),
                    Fecha = table.Column<DateTime>(nullable: false),
                    Hora = table.Column<string>(maxLength: 5, nullable: false),
                    IdAeroLineas = table.Column<int>(maxLength: 11, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Accesos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Aerolineas",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Nombre = table.Column<string>(maxLength: 150, nullable: false),
                    IdEstado = table.Column<int>(nullable: false),
                    PDFPasajeros = table.Column<int>(nullable: false),
                    Codigo = table.Column<string>(nullable: true),
                    Sigla = table.Column<string>(nullable: true),
                    CantidadUsuarios = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Aerolineas", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Aeropuertos",
                columns: table => new
                {
                    CodigoIATA = table.Column<string>(maxLength: 3, nullable: false),
                    CobroCausal64VuelosDom = table.Column<bool>(nullable: false),
                    Ciudad = table.Column<string>(maxLength: 45, nullable: true),
                    Pais = table.Column<string>(maxLength: 45, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Aeropuertos", x => x.CodigoIATA);
                });

            migrationBuilder.CreateTable(
                name: "Cargues",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    FechaHora = table.Column<DateTime>(nullable: false),
                    Usuario = table.Column<string>(maxLength: 50, nullable: false),
                    Tipo = table.Column<int>(nullable: false),
                    Archivo = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cargues", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "HorariosOperaciones",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Dia = table.Column<string>(maxLength: 1, nullable: false),
                    HoraInicio = table.Column<string>(maxLength: 5, nullable: false),
                    HoraFin = table.Column<string>(maxLength: 5, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HorariosOperaciones", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Paises",
                columns: table => new
                {
                    Id = table.Column<string>(maxLength: 2, nullable: false),
                    Nombre = table.Column<string>(maxLength: 100, nullable: false),
                    IdEstado = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Paises", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PoliticasDeTratamientoDeDatos",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    NombreUsuario = table.Column<string>(maxLength: 150, nullable: true),
                    AceptarPoliticas = table.Column<bool>(type: "BIT", nullable: false),
                    Fecha = table.Column<DateTime>(nullable: false),
                    Hora = table.Column<string>(maxLength: 5, nullable: true),
                    Email = table.Column<string>(maxLength: 256, nullable: true),
                    PhoneNumber = table.Column<string>(maxLength: 45, nullable: true),
                    NumeroDocumento = table.Column<string>(maxLength: 20, nullable: true),
                    Cargo = table.Column<string>(maxLength: 50, nullable: true),
                    Aerolinea = table.Column<string>(maxLength: 150, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PoliticasDeTratamientoDeDatos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<string>(maxLength: 150, nullable: false),
                    Name = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TasasAeroportuarias",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ValorCOP = table.Column<decimal>(nullable: false),
                    ValorUSD = table.Column<decimal>(nullable: false),
                    Fecha = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TasasAeroportuarias", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "U_Catalogo",
                columns: table => new
                {
                    Id = table.Column<int>(maxLength: 11, nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Nombre = table.Column<string>(maxLength: 255, nullable: true),
                    Descripcion = table.Column<string>(maxLength: 255, nullable: true),
                    Activo = table.Column<bool>(nullable: false),
                    FechaCreacion = table.Column<DateTime>(nullable: false),
                    Usuario = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_U_Catalogo", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Usuario",
                columns: table => new
                {
                    Id = table.Column<string>(maxLength: 150, nullable: false),
                    UserName = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(maxLength: 256, nullable: true),
                    Email = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<int>(type: "int", nullable: false),
                    PasswordHash = table.Column<string>(nullable: true),
                    SecurityStamp = table.Column<string>(nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true),
                    PhoneNumberConfirmed = table.Column<int>(type: "int", nullable: false),
                    TwoFactorEnabled = table.Column<int>(type: "int", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(nullable: true),
                    LockoutEnabled = table.Column<int>(type: "int", nullable: false),
                    AccessFailedCount = table.Column<int>(nullable: false),
                    Nombre = table.Column<string>(nullable: true),
                    Apellido = table.Column<string>(nullable: true),
                    Cargo = table.Column<string>(nullable: true),
                    Telefono = table.Column<string>(nullable: true),
                    TipoDocumento = table.Column<string>(nullable: true),
                    NumeroDocumento = table.Column<string>(nullable: true),
                    Activo = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuario", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ValidacionesManuales",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    IdVuelo = table.Column<int>(maxLength: 10, nullable: false),
                    CantPasajeros_Old = table.Column<int>(maxLength: 3, nullable: false),
                    CantPasajeros_New = table.Column<int>(maxLength: 3, nullable: false),
                    CausalPasajeros = table.Column<string>(maxLength: 3, nullable: true),
                    CantTransitos_Old = table.Column<int>(maxLength: 3, nullable: false),
                    CantTransitos_New = table.Column<int>(maxLength: 3, nullable: false),
                    CausalTransitos = table.Column<string>(maxLength: 3, nullable: true),
                    CantInfantes_Old = table.Column<int>(maxLength: 3, nullable: false),
                    CantInfantes_New = table.Column<int>(maxLength: 3, nullable: false),
                    CausalInfantes = table.Column<string>(maxLength: 3, nullable: true),
                    CantTTL_Old = table.Column<int>(maxLength: 3, nullable: false),
                    CantTTL_New = table.Column<int>(maxLength: 3, nullable: false),
                    CausalTTL = table.Column<string>(maxLength: 3, nullable: true),
                    CantTTC_Old = table.Column<int>(maxLength: 3, nullable: false),
                    CantTTC_New = table.Column<int>(maxLength: 3, nullable: false),
                    CausalTTC = table.Column<string>(maxLength: 3, nullable: true),
                    CantEX_Old = table.Column<int>(maxLength: 3, nullable: false),
                    CantEX_New = table.Column<int>(maxLength: 3, nullable: false),
                    CausalEX = table.Column<int>(maxLength: 3, nullable: false),
                    CantTRIP_Old = table.Column<int>(maxLength: 3, nullable: false),
                    CantTRIP_New = table.Column<int>(maxLength: 3, nullable: false),
                    CausalTRIP = table.Column<string>(maxLength: 3, nullable: true),
                    CantPagoCOP_Old = table.Column<int>(maxLength: 3, nullable: false),
                    CantPagoCOP_New = table.Column<int>(maxLength: 3, nullable: false),
                    CausalPagoCOP = table.Column<string>(maxLength: 3, nullable: true),
                    CantPagoUSD_Old = table.Column<int>(maxLength: 3, nullable: false),
                    CantPagoUSD_New = table.Column<int>(maxLength: 3, nullable: false),
                    CausalPagoUSD = table.Column<string>(maxLength: 3, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ValidacionesManuales", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "HorariosAerolineas",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    IdAerolinea = table.Column<int>(nullable: false),
                    Fecha = table.Column<DateTime>(nullable: false),
                    HoraInicio = table.Column<string>(maxLength: 5, nullable: false),
                    HoraFin = table.Column<string>(maxLength: 5, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HorariosAerolineas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HorariosAerolineas_Aerolineas_IdAerolinea",
                        column: x => x.IdAerolinea,
                        principalTable: "Aerolineas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Tripulantes",
                columns: table => new
                {
                    IdTripulantes = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    NombreTripulante = table.Column<string>(maxLength: 150, nullable: true),
                    LicenciaTripulante = table.Column<string>(maxLength: 45, nullable: true),
                    FuncionTripulante = table.Column<string>(maxLength: 45, nullable: true),
                    codigoAreolinea = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tripulantes", x => x.IdTripulantes);
                    table.ForeignKey(
                        name: "FK_Tripulantes_Aerolineas_codigoAreolinea",
                        column: x => x.codigoAreolinea,
                        principalTable: "Aerolineas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Ciudades",
                columns: table => new
                {
                    Id = table.Column<string>(maxLength: 3, nullable: false),
                    Nombre = table.Column<string>(maxLength: 150, nullable: false),
                    IdPais = table.Column<string>(nullable: false),
                    IdEstado = table.Column<int>(nullable: false),
                    Codigo = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ciudades", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Ciudades_Paises_IdPais",
                        column: x => x.IdPais,
                        principalTable: "Paises",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ClaimsRoles",
                columns: table => new
                {
                    Id = table.Column<int>(maxLength: 150, nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    RoleId = table.Column<string>(nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClaimsRoles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClaimsRoles_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "U_Item",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    IdCatalogo = table.Column<int>(nullable: false),
                    Nombre = table.Column<string>(maxLength: 255, nullable: true),
                    Descripcion = table.Column<string>(maxLength: 255, nullable: true),
                    Activo = table.Column<bool>(nullable: false),
                    FechaCreacion = table.Column<DateTime>(nullable: false),
                    Usuario = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_U_Item", x => x.Id);
                    table.ForeignKey(
                        name: "FK_U_Item_U_Catalogo_IdCatalogo",
                        column: x => x.IdCatalogo,
                        principalTable: "U_Catalogo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ClaimsUsuario",
                columns: table => new
                {
                    Id = table.Column<int>(maxLength: 150, nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<string>(nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClaimsUsuario", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClaimsUsuario_Usuario_UserId",
                        column: x => x.UserId,
                        principalTable: "Usuario",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LoginsUsuario",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(maxLength: 150, nullable: false),
                    ProviderKey = table.Column<string>(maxLength: 150, nullable: false),
                    ProviderDisplayName = table.Column<string>(nullable: true),
                    UserId = table.Column<string>(maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LoginsUsuario", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_LoginsUsuario_Usuario_UserId",
                        column: x => x.UserId,
                        principalTable: "Usuario",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RolesUsuarios",
                columns: table => new
                {
                    UserId = table.Column<string>(maxLength: 150, nullable: false),
                    RoleId = table.Column<string>(maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RolesUsuarios", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_RolesUsuarios_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RolesUsuarios_Usuario_UserId",
                        column: x => x.UserId,
                        principalTable: "Usuario",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TokensUsuario",
                columns: table => new
                {
                    UserId = table.Column<string>(maxLength: 150, nullable: false),
                    LoginProvider = table.Column<string>(maxLength: 150, nullable: false),
                    Name = table.Column<string>(maxLength: 150, nullable: false),
                    Value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TokensUsuario", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_TokensUsuario_Usuario_UserId",
                        column: x => x.UserId,
                        principalTable: "Usuario",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UsuariosAerolineas",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    IdAerolinea = table.Column<int>(nullable: false),
                    IdUsuario = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsuariosAerolineas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UsuariosAerolineas_Aerolineas_IdAerolinea",
                        column: x => x.IdAerolinea,
                        principalTable: "Aerolineas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UsuariosAerolineas_Usuario_IdUsuario",
                        column: x => x.IdUsuario,
                        principalTable: "Usuario",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Vuelos",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    NumeroVuelo = table.Column<string>(maxLength: 10, nullable: false),
                    IdAerolinea = table.Column<int>(nullable: false),
                    IdOrigen = table.Column<string>(nullable: false),
                    IdDestino = table.Column<string>(nullable: false),
                    IdEstado = table.Column<int>(nullable: false),
                    TipoVuelo = table.Column<string>(nullable: false),
                    IdVuelo = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vuelos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Vuelos_Aerolineas_IdAerolinea",
                        column: x => x.IdAerolinea,
                        principalTable: "Aerolineas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Vuelos_Ciudades_IdDestino",
                        column: x => x.IdDestino,
                        principalTable: "Ciudades",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Vuelos_Ciudades_IdOrigen",
                        column: x => x.IdOrigen,
                        principalTable: "Ciudades",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "Causales",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Codigo = table.Column<string>(maxLength: 5, nullable: false),
                    Descripcion = table.Column<string>(maxLength: 250, nullable: false),
                    Tipo = table.Column<int>(nullable: false),
                    Estado = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Causales", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Causales_U_Item_Estado",
                        column: x => x.Estado,
                        principalTable: "U_Item",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Causales_U_Item_Tipo",
                        column: x => x.Tipo,
                        principalTable: "U_Item",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "OperacionesVuelos",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    MatriculaVuelo = table.Column<string>(maxLength: 15, nullable: false),
                    FechaVuelo = table.Column<DateTime>(nullable: false),
                    HoraVuelo = table.Column<string>(maxLength: 5, nullable: false),
                    TotalEmbarcados = table.Column<int>(nullable: false),
                    INF = table.Column<int>(nullable: false),
                    TTL = table.Column<int>(nullable: false),
                    TTC = table.Column<int>(nullable: false),
                    EX = table.Column<int>(nullable: false),
                    TRIP = table.Column<int>(nullable: false),
                    PAX = table.Column<int>(nullable: false),
                    PagoCOP = table.Column<int>(nullable: false),
                    PagoUSD = table.Column<int>(nullable: false),
                    TipoVuelo = table.Column<string>(maxLength: 5, nullable: false),
                    NumeroVuelo = table.Column<string>(maxLength: 15, nullable: false),
                    Destino = table.Column<string>(maxLength: 5, nullable: false),
                    IdAerolinea = table.Column<int>(nullable: false),
                    EstadoProceso = table.Column<int>(nullable: false),
                    NumeroVueloLlegada = table.Column<string>(maxLength: 15, nullable: false),
                    OrigenDes = table.Column<string>(maxLength: 45, nullable: false),
                    Origen = table.Column<string>(maxLength: 45, nullable: false),
                    FechaLlegada = table.Column<DateTime>(nullable: false),
                    HoraLlegada = table.Column<string>(maxLength: 5, nullable: false),
                    TotalEmbarcadosAdd = table.Column<int>(nullable: true),
                    TotalEmbarcados_LIQ = table.Column<int>(nullable: true),
                    FechaCreacion = table.Column<DateTime>(nullable: false),
                    CargueId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OperacionesVuelos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OperacionesVuelos_Cargues_CargueId",
                        column: x => x.CargueId,
                        principalTable: "Cargues",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OperacionesVuelos_U_Item_EstadoProceso",
                        column: x => x.EstadoProceso,
                        principalTable: "U_Item",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OperacionesVuelos_Aerolineas_IdAerolinea",
                        column: x => x.IdAerolinea,
                        principalTable: "Aerolineas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Tickets",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    NumeroTicket = table.Column<string>(nullable: false),
                    TipoConsulta = table.Column<int>(nullable: false),
                    Asunto = table.Column<string>(nullable: false),
                    FechaVuelo = table.Column<DateTime>(nullable: false),
                    FechaCreacion = table.Column<DateTime>(nullable: false),
                    Mensaje = table.Column<string>(nullable: false),
                    Adjunto = table.Column<string>(nullable: true),
                    Estado = table.Column<int>(nullable: false),
                    IdAerolinea = table.Column<int>(nullable: false),
                    IdUsuario = table.Column<string>(nullable: false),
                    Seguimiento = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tickets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tickets_U_Item_Estado",
                        column: x => x.Estado,
                        principalTable: "U_Item",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Tickets_Aerolineas_IdAerolinea",
                        column: x => x.IdAerolinea,
                        principalTable: "Aerolineas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Tickets_Usuario_IdUsuario",
                        column: x => x.IdUsuario,
                        principalTable: "Usuario",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Tickets_U_Item_TipoConsulta",
                        column: x => x.TipoConsulta,
                        principalTable: "U_Item",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "NovedadesProcesos",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    IdOperacionVuelo = table.Column<int>(nullable: false),
                    TipoNovedad = table.Column<int>(nullable: false),
                    IdCausal = table.Column<int>(nullable: false),
                    Descripcion = table.Column<string>(nullable: false),
                    FechaHora = table.Column<DateTime>(nullable: false),
                    IdRegistro = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NovedadesProcesos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NovedadesProcesos_Causales_IdCausal",
                        column: x => x.IdCausal,
                        principalTable: "Causales",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_NovedadesProcesos_OperacionesVuelos_IdOperacionVuelo",
                        column: x => x.IdOperacionVuelo,
                        principalTable: "OperacionesVuelos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "OperacionesVueloErrores",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    IdVuelo = table.Column<int>(nullable: false),
                    TipoError = table.Column<string>(maxLength: 45, nullable: false),
                    Error = table.Column<string>(maxLength: 500, nullable: false),
                    Fecha = table.Column<DateTime>(nullable: false),
                    Valores = table.Column<string>(maxLength: 100, nullable: true),
                    ValoresNuevos = table.Column<int>(maxLength: 10, nullable: false),
                    TipoValidacion = table.Column<int>(nullable: false),
                    TipoValidacion2 = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OperacionesVueloErrores", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OperacionesVueloErrores_OperacionesVuelos_IdVuelo",
                        column: x => x.IdVuelo,
                        principalTable: "OperacionesVuelos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OperacionVueloSeguimiento",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    IdOperacionVuelo = table.Column<int>(nullable: false),
                    Observacion = table.Column<string>(maxLength: 200, nullable: true),
                    Estado = table.Column<int>(nullable: false),
                    IdUsuario = table.Column<string>(nullable: false),
                    FechaCreacion = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OperacionVueloSeguimiento", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OperacionVueloSeguimiento_U_Item_Estado",
                        column: x => x.Estado,
                        principalTable: "U_Item",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OperacionVueloSeguimiento_OperacionesVuelos_IdOperacionVuelo",
                        column: x => x.IdOperacionVuelo,
                        principalTable: "OperacionesVuelos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_OperacionVueloSeguimiento_Usuario_IdUsuario",
                        column: x => x.IdUsuario,
                        principalTable: "Usuario",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Pasajeros",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    IdOperacionVuelo = table.Column<int>(nullable: false),
                    NombrePasajero = table.Column<string>(maxLength: 250, nullable: false),
                    IdCategoriaPasajero = table.Column<string>(nullable: false),
                    FechaVuelo = table.Column<DateTime>(nullable: false),
                    NumeroVuelo = table.Column<string>(nullable: false),
                    MatriculaVuelo = table.Column<string>(maxLength: 15, nullable: false),
                    Realiza_viaje = table.Column<string>(maxLength: 45, nullable: true),
                    Motivo_exencion = table.Column<string>(maxLength: 45, nullable: true),
                    IdCargue = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pasajeros", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Pasajeros_OperacionesVuelos_IdOperacionVuelo",
                        column: x => x.IdOperacionVuelo,
                        principalTable: "OperacionesVuelos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PasajerosTransito",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    IdOperacionVuelo = table.Column<int>(nullable: false),
                    NombrePasajero = table.Column<string>(maxLength: 250, nullable: false),
                    IdVueloLlegada = table.Column<int>(nullable: false),
                    IdVueloSalida = table.Column<int>(nullable: false),
                    Categoria = table.Column<string>(nullable: false),
                    FechaLlegada = table.Column<DateTime>(nullable: false),
                    HoraLlegada = table.Column<string>(maxLength: 5, nullable: false),
                    FechaSalida = table.Column<DateTime>(nullable: false),
                    HoraSalida = table.Column<string>(maxLength: 5, nullable: false),
                    FechaHoraCargue = table.Column<DateTime>(nullable: false),
                    Firmado = table.Column<int>(nullable: false),
                    FechaHoraFirma = table.Column<DateTime>(nullable: false),
                    NumeroVueloSalida = table.Column<string>(maxLength: 15, nullable: false),
                    Destino = table.Column<string>(maxLength: 5, nullable: false),
                    AerolineaSalida = table.Column<string>(maxLength: 50, nullable: false),
                    NumeroVueloLlegada = table.Column<string>(maxLength: 15, nullable: false),
                    Origen = table.Column<string>(maxLength: 5, nullable: false),
                    AerolineaLlegada = table.Column<string>(maxLength: 50, nullable: false),
                    Observaciones = table.Column<string>(maxLength: 100, nullable: true),
                    TasaCobrada = table.Column<bool>(nullable: false),
                    IdCargue = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PasajerosTransito", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PasajerosTransito_OperacionesVuelos_IdOperacionVuelo",
                        column: x => x.IdOperacionVuelo,
                        principalTable: "OperacionesVuelos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RutaArchivos",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    NombreArchivo = table.Column<string>(maxLength: 256, nullable: false),
                    IdOperacionVuelo = table.Column<int>(nullable: false),
                    OperacionesVuelosId = table.Column<int>(nullable: true),
                    TipoArchivo = table.Column<string>(maxLength: 45, nullable: false),
                    FechaCreacion = table.Column<DateTime>(nullable: false),
                    FechaActualizacion = table.Column<DateTime>(nullable: false),
                    IdUsuario = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RutaArchivos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RutaArchivos_Usuario_IdUsuario",
                        column: x => x.IdUsuario,
                        principalTable: "Usuario",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RutaArchivos_OperacionesVuelos_OperacionesVuelosId",
                        column: x => x.OperacionesVuelosId,
                        principalTable: "OperacionesVuelos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RespuestasTickets",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    IdTicket = table.Column<int>(nullable: false),
                    Mensaje = table.Column<string>(nullable: false),
                    Adjunto = table.Column<string>(nullable: true),
                    FechaCreacion = table.Column<DateTime>(nullable: false),
                    IdUsuario = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RespuestasTickets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RespuestasTickets_Tickets_IdTicket",
                        column: x => x.IdTicket,
                        principalTable: "Tickets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RespuestasTickets_Usuario_IdUsuario",
                        column: x => x.IdUsuario,
                        principalTable: "Usuario",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Causales_Estado",
                table: "Causales",
                column: "Estado");

            migrationBuilder.CreateIndex(
                name: "IX_Causales_Tipo",
                table: "Causales",
                column: "Tipo");

            migrationBuilder.CreateIndex(
                name: "IX_Ciudades_IdPais",
                table: "Ciudades",
                column: "IdPais");

            migrationBuilder.CreateIndex(
                name: "IX_ClaimsRoles_RoleId",
                table: "ClaimsRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_ClaimsUsuario_UserId",
                table: "ClaimsUsuario",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_HorariosAerolineas_IdAerolinea",
                table: "HorariosAerolineas",
                column: "IdAerolinea");

            migrationBuilder.CreateIndex(
                name: "IX_LoginsUsuario_UserId",
                table: "LoginsUsuario",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_NovedadesProcesos_IdCausal",
                table: "NovedadesProcesos",
                column: "IdCausal");

            migrationBuilder.CreateIndex(
                name: "IX_NovedadesProcesos_IdOperacionVuelo",
                table: "NovedadesProcesos",
                column: "IdOperacionVuelo");

            migrationBuilder.CreateIndex(
                name: "IX_OperacionesVueloErrores_IdVuelo",
                table: "OperacionesVueloErrores",
                column: "IdVuelo");

            migrationBuilder.CreateIndex(
                name: "IX_OperacionesVuelos_CargueId",
                table: "OperacionesVuelos",
                column: "CargueId");

            migrationBuilder.CreateIndex(
                name: "IX_OperacionesVuelos_EstadoProceso",
                table: "OperacionesVuelos",
                column: "EstadoProceso");

            migrationBuilder.CreateIndex(
                name: "IX_OperacionesVuelos_IdAerolinea",
                table: "OperacionesVuelos",
                column: "IdAerolinea");

            migrationBuilder.CreateIndex(
                name: "IX_OperacionVueloSeguimiento_Estado",
                table: "OperacionVueloSeguimiento",
                column: "Estado");

            migrationBuilder.CreateIndex(
                name: "IX_OperacionVueloSeguimiento_IdOperacionVuelo",
                table: "OperacionVueloSeguimiento",
                column: "IdOperacionVuelo");

            migrationBuilder.CreateIndex(
                name: "IX_OperacionVueloSeguimiento_IdUsuario",
                table: "OperacionVueloSeguimiento",
                column: "IdUsuario");

            migrationBuilder.CreateIndex(
                name: "IX_Pasajeros_IdOperacionVuelo",
                table: "Pasajeros",
                column: "IdOperacionVuelo");

            migrationBuilder.CreateIndex(
                name: "IX_PasajerosTransito_IdOperacionVuelo",
                table: "PasajerosTransito",
                column: "IdOperacionVuelo");

            migrationBuilder.CreateIndex(
                name: "IX_RespuestasTickets_IdTicket",
                table: "RespuestasTickets",
                column: "IdTicket");

            migrationBuilder.CreateIndex(
                name: "IX_RespuestasTickets_IdUsuario",
                table: "RespuestasTickets",
                column: "IdUsuario");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "Roles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_RolesUsuarios_RoleId",
                table: "RolesUsuarios",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_RutaArchivos_IdUsuario",
                table: "RutaArchivos",
                column: "IdUsuario");

            migrationBuilder.CreateIndex(
                name: "IX_RutaArchivos_OperacionesVuelosId",
                table: "RutaArchivos",
                column: "OperacionesVuelosId");

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_Estado",
                table: "Tickets",
                column: "Estado");

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_IdAerolinea",
                table: "Tickets",
                column: "IdAerolinea");

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_IdUsuario",
                table: "Tickets",
                column: "IdUsuario");

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_TipoConsulta",
                table: "Tickets",
                column: "TipoConsulta");

            migrationBuilder.CreateIndex(
                name: "IX_Tripulantes_codigoAreolinea",
                table: "Tripulantes",
                column: "codigoAreolinea");

            migrationBuilder.CreateIndex(
                name: "IX_U_Item_IdCatalogo",
                table: "U_Item",
                column: "IdCatalogo");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "Usuario",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "Usuario",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_UsuariosAerolineas_IdAerolinea",
                table: "UsuariosAerolineas",
                column: "IdAerolinea");

            migrationBuilder.CreateIndex(
                name: "IX_UsuariosAerolineas_IdUsuario",
                table: "UsuariosAerolineas",
                column: "IdUsuario");

            migrationBuilder.CreateIndex(
                name: "IX_Vuelos_IdAerolinea",
                table: "Vuelos",
                column: "IdAerolinea");

            migrationBuilder.CreateIndex(
                name: "IX_Vuelos_IdDestino",
                table: "Vuelos",
                column: "IdDestino");

            migrationBuilder.CreateIndex(
                name: "IX_Vuelos_IdOrigen",
                table: "Vuelos",
                column: "IdOrigen");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Accesos");

            migrationBuilder.DropTable(
                name: "Aeropuertos");

            migrationBuilder.DropTable(
                name: "ClaimsRoles");

            migrationBuilder.DropTable(
                name: "ClaimsUsuario");

            migrationBuilder.DropTable(
                name: "HorariosAerolineas");

            migrationBuilder.DropTable(
                name: "HorariosOperaciones");

            migrationBuilder.DropTable(
                name: "LoginsUsuario");

            migrationBuilder.DropTable(
                name: "NovedadesProcesos");

            migrationBuilder.DropTable(
                name: "OperacionesVueloErrores");

            migrationBuilder.DropTable(
                name: "OperacionVueloSeguimiento");

            migrationBuilder.DropTable(
                name: "Pasajeros");

            migrationBuilder.DropTable(
                name: "PasajerosTransito");

            migrationBuilder.DropTable(
                name: "PoliticasDeTratamientoDeDatos");

            migrationBuilder.DropTable(
                name: "RespuestasTickets");

            migrationBuilder.DropTable(
                name: "RolesUsuarios");

            migrationBuilder.DropTable(
                name: "RutaArchivos");

            migrationBuilder.DropTable(
                name: "TasasAeroportuarias");

            migrationBuilder.DropTable(
                name: "TokensUsuario");

            migrationBuilder.DropTable(
                name: "Tripulantes");

            migrationBuilder.DropTable(
                name: "UsuariosAerolineas");

            migrationBuilder.DropTable(
                name: "ValidacionesManuales");

            migrationBuilder.DropTable(
                name: "Vuelos");

            migrationBuilder.DropTable(
                name: "Causales");

            migrationBuilder.DropTable(
                name: "Tickets");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "OperacionesVuelos");

            migrationBuilder.DropTable(
                name: "Ciudades");

            migrationBuilder.DropTable(
                name: "Usuario");

            migrationBuilder.DropTable(
                name: "Cargues");

            migrationBuilder.DropTable(
                name: "U_Item");

            migrationBuilder.DropTable(
                name: "Aerolineas");

            migrationBuilder.DropTable(
                name: "Paises");

            migrationBuilder.DropTable(
                name: "U_Catalogo");
        }
    }
}
