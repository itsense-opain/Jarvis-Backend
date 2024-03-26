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
    public class TasaAeroportuariaController : ControllerBase
    {
        private readonly ITasaAeroportuariaAplicacion tasaAplicacion;
        private readonly ILogger logger;

        public TasaAeroportuariaController(ITasaAeroportuariaAplicacion tasa, ILogger l)
        {
            tasaAplicacion = tasa;
            logger = l;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IList<TasaAeroportuariaOtd>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IList<TasaAeroportuariaOtd>>> ObtenerTodosAsync()
        {
            try
            {
                var respuesta = await tasaAplicacion.ObtenerTodosAsync();
                logger.Information("Se ejecutó TasaAeroportuariaController.ObtenerTodosAsync");
                return Ok(respuesta);

            }
            catch (Exception err)
            {
                logger.Error(err, "Error en TasaAeroportuariaController.ObtenerTodosAsync");
                return BadRequest();
            }
        }

        [HttpPost]
        public async Task<IActionResult> InsertarAsync([FromBody]TasaAeroportuariaOtd tasaAeroportuariaOtd)
        {
            if (tasaAeroportuariaOtd == null)
            {
                logger.Warning("Entidad para insertar vacia");
                return BadRequest();
            }

            try
            {
                await tasaAplicacion.InsertarAsync(tasaAeroportuariaOtd).ConfigureAwait(false);
                logger.Information("Insertó: {@entidad}" + tasaAeroportuariaOtd);
                return Ok();
            }
            catch (Exception err)
            {
                logger.Error(err, "Error insertando: {@entidad} ", tasaAeroportuariaOtd);
                return BadRequest();
            }
        }

        [HttpPost]
        public async Task<IActionResult> ActualizarAsync([FromBody]TasaAeroportuariaOtd tasaAeroportuariaOtd)
        {
            if (tasaAeroportuariaOtd == null)
            {
                logger.Warning("Entidad para actualizar vacia");
                return BadRequest();
            }

            try
            {
                await tasaAplicacion.ActualizarAsync(tasaAeroportuariaOtd).ConfigureAwait(false);
                logger.Information("Actualizó: {@entidad}", tasaAeroportuariaOtd);
                return Ok();
            }
            catch (Exception err)
            {
                logger.Error(err, "Error actualizando: {@entidad}", tasaAeroportuariaOtd);
                return BadRequest();
            }
        }

        [HttpPost("{id}")]
        public async Task<IActionResult> EliminarAsync(int id)
        {
            if (id < 1)
            {
                logger.Warning("Identificador para eliminar no válido");
                return BadRequest();
            }

            try
            {
                await tasaAplicacion.EliminarAsync(id).ConfigureAwait(false);
                logger.Information("Eliminó id: {@id}", id);
                return Ok();
            }
            catch (Exception err)
            {
                logger.Error(err, "Error eliminando: {@id}", id);
                return BadRequest();
            }
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(TasaAeroportuariaOtd), StatusCodes.Status200OK)]
        public async Task<ActionResult<TasaAeroportuariaOtd>> ObtenerAsync(int id)
        {
            if (id < 1)
            {
                logger.Warning("Identificador para consultar no válido");
                return BadRequest();
            }

            try
            {
                var respuesta = await tasaAplicacion.ObtenerAsync(id).ConfigureAwait(false);
                logger.Information("Consultó: {@entidad}", respuesta);
                return Ok(respuesta);
            }
            catch (Exception err)
            {
                logger.Error(err, "Error consultando id: {@id}", id);
                return BadRequest();
            }
        }

        [HttpGet]
        [ProducesResponseType(typeof(TasaAeroportuariaOtd), StatusCodes.Status200OK)]
        public async Task<ActionResult<TasaAeroportuariaOtd>> ObtenerUltimaAsync()
        {
            try
            {
                var respuesta = await tasaAplicacion.ObtenerUltimaAsync();
                logger.Information("Se ejecutó TasaAeroportuariaController.ObtenerUltimaAsync");
                return Ok(respuesta);

            }
            catch (Exception err)
            {
                logger.Error(err, "Error en TasaAeroportuariaController.ObtenerUltimaAsync");
                return BadRequest();
            }
        }
    }
}