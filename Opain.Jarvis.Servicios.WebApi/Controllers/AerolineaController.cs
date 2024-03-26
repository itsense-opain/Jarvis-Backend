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
    public class AerolineaController : ControllerBase
    {
        private readonly IAerolineaAplicacion aerolineaAplicacion;
        private readonly ILogger logger;

        public AerolineaController(IAerolineaAplicacion aerolinea, ILogger l)
        {
            aerolineaAplicacion = aerolinea;
            logger = l;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IList<AerolineaOtd>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IList<AerolineaOtd>>> ObtenerTodosAsync()
        {
            try
            {
                var respuesta = await aerolineaAplicacion.ObtenerTodosAsync();
                logger.Information("Se ejecutó AerolineaController.ObtenerTodosAsync");
                return Ok(respuesta);

            }
            catch (Exception err)
            {
                logger.Error(err, "Error en AerolineaController.ObtenerTodosAsync");
                return BadRequest();
            }
        }

        [HttpPost]
        public async Task<IActionResult> InsertarAsync([FromBody]AerolineaOtd aerolineaOtd)
        {
            if (aerolineaOtd == null)
            {
                logger.Warning("Entidad para insertar vacia");
                return BadRequest();
            }

            try
            {
                await aerolineaAplicacion.InsertarAsync(aerolineaOtd).ConfigureAwait(false);
                logger.Information("Insertó: {@entidad}" + aerolineaOtd);
                return Ok();
            }
            catch (Exception err)
            {
                logger.Error(err, "Error insertando: {@entidad} ", aerolineaOtd);
                return BadRequest();
            }
        }

        [HttpPost]
        public async Task<IActionResult> ActualizarAsync([FromBody]AerolineaOtd aerolineaOtd)
        {
            if (aerolineaOtd == null)
            {
                logger.Warning("Entidad para actualizar vacia");
                return BadRequest();
            }

            try
            {
                await aerolineaAplicacion.ActualizarAsync(aerolineaOtd).ConfigureAwait(false);
                logger.Information("Actualizó: {@entidad}", aerolineaOtd);
                return Ok();
            }
            catch (Exception err)
            {
                logger.Error(err, "Error actualizando: {@entidad}", aerolineaOtd);
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
                await aerolineaAplicacion.EliminarAsync(id).ConfigureAwait(false);
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
        [ProducesResponseType(typeof(AerolineaOtd), StatusCodes.Status200OK)]
        public async Task<ActionResult<AerolineaOtd>> ObtenerAsync(int id)
        {
            if (id < 1)
            {
                logger.Warning("Identificador para consultar no válido");
                return BadRequest();
            }

            try
            {
                var respuesta = await aerolineaAplicacion.ObtenerAsync(id).ConfigureAwait(false);
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
        public async Task<IActionResult> ActualizarTodosAsync([FromBody]List<AerolineaOtd> aerolineasOtd)
        {
            if (aerolineasOtd == null)
            {
                logger.Warning("Entidades para actualizar vacia");
                return BadRequest();
            }

            try
            {
                await aerolineaAplicacion.ActualizarTodosAsync(aerolineasOtd).ConfigureAwait(false);
                logger.Information("Actualizó: {@entidad}", aerolineasOtd);
                return Ok();
            }
            catch (Exception err)
            {
                logger.Error(err, "Error actualizando: {@entidad}", aerolineasOtd);
                return BadRequest();
            }
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(AerolineaOtd), StatusCodes.Status200OK)]
        public async Task<ActionResult<AerolineaOtd>> ObtenerHorarioIdAerolineaAsync(int id)
        {
            if (id < 1)
            {
                logger.Warning("Identificador para consultar no válido");
                return BadRequest();
            }

            try
            {
                var respuesta = await aerolineaAplicacion.ObtenerHorarioIdAerolineaAsync(id).ConfigureAwait(false);
                logger.Information("Consultó: {@entidad}", respuesta);
                return Ok(respuesta);
            }
            catch (Exception err)
            {
                logger.Error(err, "Error consultando id: {@id}", id);
                return BadRequest();
            }
        }
    }
}