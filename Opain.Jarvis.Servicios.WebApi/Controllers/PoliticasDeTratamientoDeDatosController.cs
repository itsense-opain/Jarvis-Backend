using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Opain.Jarvis.Aplicacion.Interfaces;
using Opain.Jarvis.Dominio.Entidades;
using Serilog;

namespace Opain.Jarvis.Servicios.WebApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class PoliticasDeTratamientoDeDatosController : ControllerBase
    {
        private readonly IPoliticasDeTratamientoDeDatosAplicacion politicasDeTratamientoDeDatosAplicacion;
        private readonly ILogger logger;

        public PoliticasDeTratamientoDeDatosController(IPoliticasDeTratamientoDeDatosAplicacion politicasDeTratamientoDeDatos, ILogger l)
        {
            politicasDeTratamientoDeDatosAplicacion = politicasDeTratamientoDeDatos;
            logger = l;
        }

        [HttpGet]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        public async Task<ActionResult<bool>> ObtenerTodosAsync(string NombreUsuario)
        {
            IList<PoliticasDeTratamientoDeDatosOtd> politicasDeTratamientoDeDatos = new List<PoliticasDeTratamientoDeDatosOtd>();
            bool Respuesta = false;
            try
            {

                if (!string.IsNullOrEmpty(NombreUsuario))
                {
                    try
                    {
                        Respuesta = await politicasDeTratamientoDeDatosAplicacion.ObtenerTodosAsync(NombreUsuario).ConfigureAwait(false);
                        logger.Information("Acepto Politica: {@cantidad} registros", Respuesta);
                    }
                    catch (Exception err)
                    {
                        logger.Error(err, "Error al consultar la pilitica con el usuario: {@fi}", NombreUsuario);
                    }
                }

                
                return Ok(Respuesta);
            }
            catch (Exception err)
            { 
                logger.Error(err, "Error consultando todas los Politicas De Tratamiento De Datos: {@fi}", NombreUsuario);
                return BadRequest();
            }
        }

        [HttpPost]
        public async Task<IActionResult> InsertarAsync([FromBody]PoliticasDeTratamientoDeDatosOtd politicasDeTratamientoDeDatosOtd)
        {
            if (politicasDeTratamientoDeDatosOtd == null)
            {
                logger.Warning("Entidad para insertar vacia");
                return BadRequest();
            }

            try
            {
                await politicasDeTratamientoDeDatosAplicacion.InsertarAsync(politicasDeTratamientoDeDatosOtd).ConfigureAwait(false);
                logger.Information("Insertó: {@entidad}" + politicasDeTratamientoDeDatosOtd);
                return Ok();
            }
            catch (Exception err)
            {
                logger.Error(err, "Error insertando: {@entidad} ", politicasDeTratamientoDeDatosOtd);
                return BadRequest();
            }
        }
    }
}