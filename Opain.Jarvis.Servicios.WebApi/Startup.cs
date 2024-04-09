using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Opain.Jarvis.Aplicacion.Interfaces;
using Opain.Jarvis.Aplicacion.Principal;
using Opain.Jarvis.Dominio.Entidades;
using Opain.Jarvis.Infraestructura.Datos;
using Opain.Jarvis.Servicios.WebApi.Helpers;

using Serilog;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.Net.Http.Formatting;
using Microsoft.Net.Http.Headers;
using Microsoft.AspNetCore.Mvc.Formatters;
using Opain.Jarvis.Infraestructura.Datos.Core;

namespace Opain.Jarvis.Servicios.WebApi
{
    public class Startup
    {
        private readonly string politicaCors = "PoliticaJarvis";
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;            
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            var appSettingsSection = Configuration.GetSection("Config");
            services.Configure<AppSettings>(appSettingsSection);

            var appSettings = appSettingsSection.Get<AppSettings>();

            var originCors = appSettings.OriginCors;
            var key = Encoding.ASCII.GetBytes(appSettings.Secret);
            var Issuer = appSettings.Issuer;
            var Audience = appSettings.Audience;

            services.AddAutoMapper(x => x.AddProfile(new PerfilMapeos()));

            services.AddCors(options => options
            .AddPolicy(politicaCors, builder => builder.WithOrigins(originCors)
            .AllowAnyHeader()
            .AllowAnyMethod()
            ));

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
            .AddJsonOptions(Options =>
            {
                Options.SerializerSettings.ContractResolver = new Newtonsoft.Json.Serialization.DefaultContractResolver();
            });

            services.AddMvc(options =>
            {             
                    options.InputFormatters.Insert(0, new BinaryInputFormatter());                
            });

            services.AddSingleton<IConfiguration>(Configuration);

            /////Serilog

            Log.Logger = new LoggerConfiguration()
                .AuditTo.Logger(cfg => cfg.MinimumLevel.Information().Enrich.FromLogContext().WriteTo.RollingFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logs/AUD.log"),  outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level}] [{SourceContext}] {Message:lj}{NewLine}"))
                .WriteTo.Logger(cfg => cfg.MinimumLevel.Warning().Enrich.FromLogContext().WriteTo.RollingFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logs/LOG.log"),  outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level}] [{SourceContext}] {Message:lj}:{Exception}{NewLine}"))
                .CreateLogger();

            services.AddSingleton(Log.Logger);

            ////Serilog


            services.AddDbContext<ContextoOpain>(options => options.UseSqlServer(Configuration.GetConnectionString("ConexionJarvisBD")));
            services.AddIdentity<Usuario, Rol>()
                .AddEntityFrameworkStores<ContextoOpain>()
                .AddDefaultTokenProviders();

            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
            services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

                })
                .AddJwtBearer(cfg =>
                {
                    cfg.Events = new JwtBearerEvents
                    {
                        OnAuthenticationFailed = context =>
                        {
                            if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                            {
                                context.Response.Headers.Add("Token-Expired", "true");
                            }
                            return Task.CompletedTask;
                        }
                    };
                    cfg.RequireHttpsMetadata = false;
                    cfg.SaveToken = false;
                    cfg.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidIssuer = Issuer,
                        ValidAudience = Audience,
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ClockSkew = TimeSpan.Zero // remove delay of token when expire
                    };
                });

            services.AddScoped<RoleManager<Rol>>();
            services.AddScoped<UserManager<Usuario>>();
            services.AddScoped<SignInManager<Usuario>>();

            services.AddScoped<Store.Helper.Ejecutor>();
            services.AddScoped<Store.OperacionesVuelo>();
            services.AddScoped<Store.Trace>();
            services.AddScoped<Store.Metodos.OperacionesVuelo>();
            services.AddScoped<Store.Metodos.Pasajeros>();

            services.AddScoped<IOperacionVueloAplicacion, OperacionVueloAplicacion>();
            services.AddScoped<IOperacionesVueloRepositorio, OperacionesVueloRepositorio>();
            services.AddScoped<IVueloRepositorio, VueloRepositorio>();
            services.AddScoped<IArchivoRepositorio, ArchivoRepositorio>();
            services.AddScoped<IArchivoAplicacion, ArchivoAplicacion>();
            services.AddScoped<IPasajeroRepositorio, PasajeroRepositorio>();
            services.AddScoped<IPasajeroAplicacion, PasajeroAplicacion>();
            services.AddScoped<IPasajeroTransitoRepositorio, PasajeroTransitoRepositorio>();
            services.AddScoped<IPasajeroTransitoAplicacion, PasajeroTransitoAplicacion>();
            services.AddScoped<IUsuarioAplicacion, UsuarioAplicacion>();
            services.AddScoped<IUsuariosRepositorio, UsuariosRepositorio>();
            services.AddScoped<IAerolineaAplicacion, AerolineaAplicacion>();
            services.AddScoped<IAerolineaRepositorio, AerolineaRepositorio>();
            services.AddScoped<IHorarioOperacionAplicacion, HorarioOperacionAplicacion>();
            services.AddScoped<IHorarioOperacionRepositorio, HorarioOperacionRepositorio>();
            services.AddScoped<ITasaAeroportuariaAplicacion, TasaAeroportuariaAplicacion>();
            services.AddScoped<ITasaAeroportuariaRepositorio, TasaAeroportuariaRepositorio>();
            services.AddScoped<IHorarioAerolineaAplicacion, HorarioAerolineaAplicacion>();
            services.AddScoped<IHorarioAerolineaRepositorio, HorarioAerolineaRepositorio>();
            services.AddScoped<ITicketAplicacion, TicketAplicacion>();
            services.AddScoped<ITicketRepositorio, TicketRepositorio>();
            services.AddScoped<IRespuestaTicketAplicacion, RespuestaTicketAplicacion>();
            services.AddScoped<IRespuestaTicketRepositorio, RespuestaTicketRepositorio>();
            services.AddScoped<IAccesoAplicacion, AccesoAplicacion>();
            services.AddScoped<IPoliticasDeTratamientoDeDatosAplicacion, PoliticasDeTratamientoDeDatosAplicacion>();
            services.AddScoped<IPoliticasDeTratamientoDeDatosRepositorio, PoliticasDeTratamientoDeDatosRepositorio>();
            services.AddScoped<IAccesoRepositorio, AccesoRepositorio>();
            services.AddScoped<ICargueAplicacion, CargueAplicacion>();
            services.AddScoped<ICargueRepositorio,CargueRepositorio>();
            services.AddScoped<ICausalRepositorio, CausalRepositorio>();
            services.AddScoped<ICausalAplicacion, CausalAplicacion>();
            services.AddScoped<INovedadRepositorio, NovedadRepositorio>();
            services.AddScoped<INovedadAplicacion, NovedadAplicacion>();
            services.AddScoped<IConsecutivoCargueRepositorio, ConsecutivoCargueRepositorio>();
            services.AddScoped<IConsecutivoCargueAplicacion, ConsecutivoCargueAplicacion>();

            services.AddScoped<InicializarDatos>();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info
                {
                    Version = "v1",
                    Title = "OPAIN Servicios API JARVIS",
                    Description = "Servicios web API del sistema JARVIS - OPAIN",
                    Contact = new Contact
                    {
                        Name = "Componente Serviex",
                        Url = "https://www.opain.co/"
                    },
                    License = new License
                    {
                        Name = "Use bajo licencia",
                        Url = "https://www.opain.co/"
                    }
                });

                var archivoXml = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var rutaXml = Path.Combine(AppContext.BaseDirectory, archivoXml);
                c.IncludeXmlComments(rutaXml);

                c.AddSecurityDefinition("Authorization", new ApiKeyScheme
                {
                    Description = "Authorization by API key.",
                    In = "header",
                    Type = "apiKey",
                    Name = "Authorization"
                });

                c.AddSecurityRequirement(new Dictionary<string, IEnumerable<string>>
                {
                    { "Authorization", new string[0] }
                });
            });
        }



        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public async void Configure(IApplicationBuilder app, IHostingEnvironment env, InicializarDatos iniciar)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("../swagger/v1/swagger.json", "OPAIN JARVIS Servicios Web API V1");
            });

            app.UseCors(politicaCors);

            app.UseAuthentication();

            app.UseMvc();

            //await iniciar.Acceso();
        }
    }

    public class BinaryInputFormatter : InputFormatter
    {
        const string binaryContentType = "application/octet-stream";
        const int bufferLength = 16384;

        public BinaryInputFormatter()
        {
            SupportedMediaTypes.Add(MediaTypeHeaderValue.Parse(binaryContentType));
        }

        public async override Task<InputFormatterResult> ReadRequestBodyAsync(InputFormatterContext context)
        {
            using (MemoryStream ms = new MemoryStream(bufferLength))
            {
                await context.HttpContext.Request.Body.CopyToAsync(ms);
                object result = ms.ToArray();
                return await InputFormatterResult.SuccessAsync(result);
            }
        }

        protected override bool CanReadType(Type type)
        {
            if (type == typeof(byte[]))
                return true;
            else
                return false;
        }
    }
}
