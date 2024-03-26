using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Opain.Jarvis.Aplicacion.Interfaces;
using Opain.Jarvis.Dominio.Entidades;
using Serilog;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authorization;

namespace Opain.Jarvis.Servicios.WebApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class UsuariosController : ControllerBase
    {
        private readonly IUsuarioAplicacion usuarios;
        private readonly ILogger logger;
        public IConfiguration Configuration { get; }
        private readonly Store.OperacionesVuelo Store_OperacionesVuelo;

        public UsuariosController(IUsuarioAplicacion u, ILogger l, Store.OperacionesVuelo store_OperacionesVuelo)
        {
            usuarios = u;
            logger = l.ForContext<UsuariosController>();
            this.Store_OperacionesVuelo = store_OperacionesVuelo;
        }

        [HttpGet]
        [ProducesResponseType(typeof(UsuarioOtd), StatusCodes.Status200OK)]
        [AllowAnonymous]
        public async Task<ActionResult<UsuarioOtd>> ObtenerPorEmailAsync(string email)
        {
            try
            {
                UsuarioOtd u = await usuarios.ObtenerPorEmailAsync(email).ConfigureAwait(false);
                return u;
            }
            catch (Exception err)
            {
                logger.Error(err, "Error metodo ObtenerPorEmailAsync");
                return BadRequest();
            }
        }

        [HttpGet]
        [ProducesResponseType(typeof(NotificacionODT), StatusCodes.Status200OK)]
        public async Task<ActionResult<NotificacionODT>> NotifyNotRead(string idUsr)
        {
            try
            {
                //var respuesta = await novedadAplicacion.ObtenerTodosPorOperacionAsync(id);
                UsuarioOtd u = await usuarios.ObtenerPorAliasAsync(idUsr).ConfigureAwait(false);

                string rolUsr = u.RolesUsuario[0].Rol.Name;
                string aerolineaUsr = "0";

                if (rolUsr.Equals("AEROLINEA"))
                {
                    aerolineaUsr = u.UsuarioAerolinea[0].IdAerolinea.ToString();
                }


                var respuesta = this.Store_OperacionesVuelo.TraerNotificaciones(rolUsr, aerolineaUsr, u.Id);

                logger.Information("Se ejecutó Store_OperacionesVuelo.TraerNotificaciones");
                return Ok(respuesta);

            }
            catch (Exception err)
            {
                logger.Error(err, "Error en .Store_OperacionesVuelo.TraerNotificaciones");
                return BadRequest();
            }
        }

        [HttpGet]
        [ProducesResponseType(typeof(AerolineaOtd), StatusCodes.Status200OK)]
        public async Task<ActionResult<AerolineaOtd>> TraerAero(string alias)
        {
            try
            {

                var respuesta = this.Store_OperacionesVuelo.TraerAero(alias);

                logger.Information("Se ejecutó Store_OperacionesVuelo.TraerAero");
                return Ok(respuesta);

            }
            catch (Exception err)
            {
                logger.Error(err, "Error en .Store_OperacionesVuelo.TraerAero");
                return BadRequest();
            }
        }

        [HttpGet]
        [ProducesResponseType(typeof(UsuarioOtd), StatusCodes.Status200OK)]
        [AllowAnonymous]
        public async Task<ActionResult<bool>> emailExiste(string email)
        {
            try
            {
                UsuarioOtd u = await usuarios.ObtenerPorEmailAsync(email).ConfigureAwait(false);

                if (u != null)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            }
            catch (Exception err)
            {
                logger.Error(err, "Error metodo emailExiste");
                return false;
            }
        }

        [HttpGet]
        [ProducesResponseType(typeof(UsuarioOtd), StatusCodes.Status200OK)]
        public async Task<ActionResult<bool>> NotifyUpd(int notify)
        {
            try
            {
                //UsuarioOtd u = await usuarios.ObtenerPorEmailAsync(email).ConfigureAwait(false);
                var respuesta = this.Store_OperacionesVuelo.NotificacionesUPD(notify);

                if (respuesta != null)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            }
            catch (Exception err)
            {
                logger.Error(err, "Error metodo emailExiste");
                return false;
            }
        }

        [HttpGet]
        [ProducesResponseType(typeof(UsuarioOtd), StatusCodes.Status200OK)]
        public async Task<ActionResult<UsuarioOtd>> ObtenerPorAliasAsync(string alias)
        {

            try
            {
                UsuarioOtd u = await usuarios.ObtenerPorAliasAsync(alias).ConfigureAwait(false);
                return u;
            }
            catch (Exception err)
            {
                logger.Error(err, "Error metodo ObtenerPorAliasAsync");
                return BadRequest();
            }
        }

        [HttpGet]
        [ProducesResponseType(typeof(UsuarioOtd), StatusCodes.Status200OK)]
        public async Task<ActionResult<bool>> ObtenerPorusuarioyclave(string alias, string clave)
        {

            try
            {
                bool u = await usuarios.ObtenerPorusuarioyclave(alias, clave).ConfigureAwait(false);
                return u;
            }
            catch (Exception err)
            {
                logger.Error(err, "Error metodo ObtenerPorusuarioyclave");
                return BadRequest();
            }
        }



        [HttpGet]
        [ProducesResponseType(typeof(IList<UsuarioOtd>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IList<UsuarioOtd>>> ObtenerTodosAsync()
        {
            try
            {
                IList<UsuarioOtd> us = await usuarios.ObtenerAsync().ConfigureAwait(false);
                return Ok(us);
            }
            catch (Exception err)
            {
                logger.Error(err, "Error metodo ObtenerAsync");
                return BadRequest();
            }

        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(UsuarioOtd), StatusCodes.Status200OK)]
        public async Task<ActionResult<UsuarioOtd>> ObtenerAsync(string id)
        {
            try
            {
                UsuarioOtd us = await usuarios.ObtenerAsync(id).ConfigureAwait(false);
                return Ok(us);
            }
            catch (Exception err)
            {
                logger.Error(err, "Error metodo ObtenerAsync");
                return BadRequest();
            }

        }

        [HttpPost]
        public async Task<IActionResult> ActualizarAsync([FromBody]UsuarioOtd usuarioOtd)
        {
            if (usuarioOtd == null)
            {
                logger.Warning("Entidad para actualizar vacia");
                return BadRequest();
            }

            try
            {
                await usuarios.ActualizarAsync(usuarioOtd).ConfigureAwait(false);
                logger.Information("Actualizó: {@entidad}" + usuarioOtd);
                return Ok();
            }
            catch (Exception err)
            {
                logger.Error(err, "Error actualizando: {@entidad} ", usuarioOtd);
                return BadRequest();
            }
        }

        [HttpPost]
        public async Task<IActionResult> InsertarAsync([FromBody]UsuarioOtd usuarioOtd)
        {
            if (usuarioOtd == null)
            {
                logger.Warning("Entidad para insertar vacia");
                return BadRequest();
            }

            try
            {
                await usuarios.InsertarAsync(usuarioOtd).ConfigureAwait(false);
                logger.Information("Insertó: {@entidad}" + usuarioOtd);
                return Ok();
            }
            catch (Exception err)
            {
                logger.Error(err, "Error insertando: {@entidad} ", usuarioOtd);
                return BadRequest();
            }
        }


        //[HttpGet]
        //public async Task<ActionResult<string>> ValidarUsuarioPass(UsuarioOtd usuarioOtd)
        //{
        //    try
        //    {
        //        string resultadoUser = "";
        //        UsuarioOtd us = await usuarios.ObtenerAsync(id).ConfigureAwait(false);
        //        return Ok(resultadoUser);
        //    }
        //    catch (Exception err)
        //    {
        //        logger.Error(err, "Error metodo ValidarUsuarioPass");
        //        return BadRequest();
        //    }

        //}

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<bool>> ActualizarclaveUsuario(string usuarioNombre, string clave)
        {

            try
            {
                bool u = await usuarios.ActualizarclaveUsuario(usuarioNombre, clave).ConfigureAwait(false);
                return u;
            }
            catch (Exception err)
            {
                logger.Error(err, "Error metodo ActualizarclaveUsuario");
                return BadRequest();
            }
        }

        [HttpPost("{id}")]
        public async Task<IActionResult> EliminarAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                logger.Warning("Identificador para eliminar no válido");
                return BadRequest();
            }

            try
            {
                await usuarios.EliminarAsync(id).ConfigureAwait(false);
                logger.Information("Eliminó id: {@id}", id);
                return Ok();
            }
            catch (Exception err)
            {
                logger.Error(err, "Error eliminando: {@id}", id);
                return BadRequest();
            }
        }
    }
}