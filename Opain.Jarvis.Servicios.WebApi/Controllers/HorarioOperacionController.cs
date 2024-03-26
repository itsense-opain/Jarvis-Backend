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
    public class HorarioOperacionController : ControllerBase
    {
        private readonly IHorarioOperacionAplicacion horarioAplicacion;
        private readonly ILogger logger;

        public HorarioOperacionController(IHorarioOperacionAplicacion horario, ILogger l)
        {
            horarioAplicacion = horario;
            logger = l;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IList<HorarioOperacionOtd>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IList<HorarioOperacionOtd>>> ObtenerTodosAsync()
        {
            try
            {
                var respuesta = await horarioAplicacion.ObtenerTodosAsync();
                logger.Information("Se ejecutó HorarioOperacionController.ObtenerTodosAsync");
                return Ok(respuesta);

            }
            catch (Exception err)
            {
                logger.Error(err, "Error en HorarioOperacionController.ObtenerTodosAsync");
                return BadRequest();
            }
        }

        [HttpPost]
        public async Task<IActionResult> InsertarAsync([FromBody]HorarioOperacionOtd horarioOperacionOtd)
        {
            if (horarioOperacionOtd == null)
            {
                logger.Warning("Entidad para insertar vacia");
                return BadRequest();
            }

            try
            {
                await horarioAplicacion.InsertarAsync(horarioOperacionOtd).ConfigureAwait(false);
                logger.Information("Insertó: {@entidad}" + horarioOperacionOtd);
                return Ok();
            }
            catch (Exception err)
            {
                logger.Error(err, "Error insertando: {@entidad} ", horarioOperacionOtd);
                return BadRequest();
            }
        }

        [HttpPost]
        public async Task<IActionResult> ActualizarAsync([FromBody]HorarioOperacionOtd horarioOperacionOtd)
        {
            if (horarioOperacionOtd == null)
            {
                logger.Warning("Entidad para actualizar vacia");
                return BadRequest();
            }

            try
            {
                await horarioAplicacion.ActualizarAsync(horarioOperacionOtd).ConfigureAwait(false);
                logger.Information("Actualizó: {@entidad}", horarioOperacionOtd);
                return Ok();
            }
            catch (Exception err)
            {
                logger.Error(err, "Error actualizando: {@entidad}", horarioOperacionOtd);
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
                await horarioAplicacion.EliminarAsync(id).ConfigureAwait(false);
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
        [ProducesResponseType(typeof(HorarioOperacionOtd), StatusCodes.Status200OK)]
        public async Task<ActionResult<HorarioOperacionOtd>> ObtenerAsync(int id)
        {
            if (id < 1)
            {
                logger.Warning("Identificador para consultar no válido");
                return BadRequest();
            }

            try
            {
                var respuesta = await horarioAplicacion.ObtenerAsync(id).ConfigureAwait(false);
                logger.Information("Consultó: {@entidad}", respuesta);
                return Ok(respuesta);
            }
            catch (Exception err)
            {
                logger.Error(err, "Error consultando id: {@id}", id);
                return BadRequest();
            }
        }

        [HttpPost]
        public async Task<IActionResult> ActualizarTodosAsync([FromBody]List<HorarioOperacionOtd> horarioOperacionOtd)
        {
            if (horarioOperacionOtd == null)
            {
                logger.Warning("Entidades para actualizar vacia");
                return BadRequest();
            }

            try
            {
                await horarioAplicacion.ActualizarTodosAsync(horarioOperacionOtd).ConfigureAwait(false);
                logger.Information("Actualizó: {@entidad}", horarioOperacionOtd);
                return Ok();
            }
            catch (Exception err)
            {
                logger.Error(err, "Error actualizando: {@entidad}", horarioOperacionOtd);
                return BadRequest();
            }
        }
    }
}