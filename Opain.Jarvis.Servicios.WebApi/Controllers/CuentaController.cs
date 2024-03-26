using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Opain.Jarvis.Dominio.Entidades;
using Opain.Jarvis.Dominio.Entidades;
using Opain.Jarvis.Servicios.WebApi.Helpers;
using Serilog;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Opain.Jarvis.Aplicacion.Interfaces;

namespace Opain.Jarvis.Servicios.WebApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CuentaController : ControllerBase
    {
        private readonly SignInManager<Usuario> signInManager;
        private readonly UserManager<Usuario> userManager;
        private readonly AppSettings appSettings;
        private readonly ILogger logger;
        private readonly IUsuarioAplicacion usuarioApp;

        public CuentaController(UserManager<Usuario> userMan, SignInManager<Usuario> signInMan, IOptions<AppSettings> appSett, ILogger l, IUsuarioAplicacion ua)
        {
            userManager = userMan;
            signInManager = signInMan;
            appSettings = appSett.Value;
            logger = l;
            usuarioApp = ua;
        }

        [HttpGet]
        public async Task<string> ObtenerTokenJwt(string usr, string psw)
        {
            try
            {
                var result = await signInManager.PasswordSignInAsync(usr, psw, false, false);

                if (result.Succeeded)
                {
                    var usuario = userManager.Users.SingleOrDefault(r => r.UserName.Equals(usr));
                    UsuarioOtd usuarioOtd = await usuarioApp.ObtenerPorEmailAsync(usuario.Email).ConfigureAwait(false);

                    logger.Information("ObtenerTokenJwt 1: usr:" + usr+" psw: "+psw);
                    return await GenerarJwtToken(usuarioOtd);
                }
            }
            catch (Exception err)
            {
                logger.Information("ObtenerTokenJwt 2: usr:" + usr + " psw: " + psw);
                //throw new ApplicationException("Intento de ingreso no válido");                
                return "false";
            }            
            //throw new ApplicationException("Intento de ingreso no válido");                
            return "false";
        }

        [HttpPost]
        [Authorize]
        public async Task<object> Registro([FromBody] RegistroOtd model)
        {
            try
            {
                var usuario = new Usuario
                {
                    UserName = model.Usuario,
                    Email = model.Correo,
                    Nombre = "Usuario",
                    Apellido = "Prueba"
                };

                var resultado = await userManager.CreateAsync(usuario, model.Contrasena);

                if (resultado.Succeeded)
                {
                    await signInManager.SignInAsync(usuario, false);
                    logger.Information("Registró: {@model}", model);
                    return null;
                }

            }
            catch (Exception err)
            {
                logger.Error(err, "Error realizando el registro: {@model}", model);
                throw new ApplicationException("No se pudo crear el usuario");
            }

            logger.Warning("Datos de registro no válidos");
            throw new ApplicationException("Datos de registro no válidos");
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<Usuario> ConsultarUsuarioPorId(string id)
        {
            Usuario usuario = await userManager.FindByIdAsync(id).ConfigureAwait(false);
            return usuario;
        }

        private async Task<string> GenerarJwtToken(UsuarioOtd usuario)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, usuario.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, usuario.Id),
                new Claim(ClaimTypes.Role, usuario.RolesUsuario.FirstOrDefault().Rol.Name),
                new Claim("NombreUsuario", usuario.Nombre + " " + usuario.Apellido)
            };

            if (usuario.UsuarioAerolinea.Count > 0 && usuario.RolesUsuario.FirstOrDefault().Rol.Name.Equals("AEROLINEA"))
            {
                claims.Add(new Claim("NombreAerolinea", usuario.UsuarioAerolinea.FirstOrDefault().Aerolinea.Nombre));
                claims.Add(new Claim("IdAerolinea", usuario.UsuarioAerolinea.FirstOrDefault().Aerolinea.Id.ToString()));
            }

            claims.Add(new Claim("ActivarCarga", "1"));

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(appSettings.Secret));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddDays(1);

            var token = new JwtSecurityToken(
                appSettings.Issuer,
                appSettings.Audience,
                claims,
                expires: expires,
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }
}