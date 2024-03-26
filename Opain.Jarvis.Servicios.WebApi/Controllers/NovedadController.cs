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
    public class NovedadController : ControllerBase
    {
        private readonly INovedadAplicacion novedadAplicacion;
        private readonly ILogger logger;
        private readonly Store.OperacionesVuelo Store_OperacionesVuelo;

        public NovedadController(INovedadAplicacion novedad, ILogger l, Store.OperacionesVuelo store_OperacionesVuelo)
        {
            novedadAplicacion = novedad;
            logger = l;
            this.Store_OperacionesVuelo = store_OperacionesVuelo;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IList<NovedadOtd>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IList<NovedadOtd>>> ObtenerTodosAsync()
        {
            try
            {
                var respuesta = await novedadAplicacion.ObtenerTodosAsync();
                logger.Information("Se ejecutó NovedadController.ObtenerTodosAsync");
                return Ok(respuesta);

            }
            catch (Exception err)
            {
                logger.Error(err, "Error en NovedadController.ObtenerTodosAsync");
                return BadRequest();
            }
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(IList<NovedadOtd>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IList<NovedadOtd>>> ObtenerTodosPorOperacionAsync(int id)
        {
            try
            {
                //var respuesta = await novedadAplicacion.ObtenerTodosPorOperacionAsync(id);
                var respuesta = this.Store_OperacionesVuelo.TraerNovedadesVuelosErr(id);

                logger.Information("Se ejecutó NovedadController.ObtenerTodosPorOperacionAsync");
                return Ok(respuesta);

            }
            catch (Exception err)
            {
                logger.Error(err, "Error en NovedadController.ObtenerTodosPorOperacionAsync");
                return BadRequest();
            }
        }

        [HttpGet]
        [ProducesResponseType(typeof(IList<NovedadesAgrupadasOtd>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IList<NovedadesAgrupadasOtd>>> ObtenerNovedadesAgrupadasAsync(string idAerolinea,string fechaInicio,string fechaFinal)
        {
            try
            {
                var respuesta = this.Store_OperacionesVuelo.TraerNovedadesAgrupadas(idAerolinea,fechaInicio,fechaFinal);

                logger.Information("Se ejecutó NovedadController.ObtenerNovedadesAgrupadasAsync");
                return Ok(respuesta);

            }
            catch (Exception err)
            {
                logger.Error(err, "Error en NovedadController.ObtenerNovedadesAgrupadasAsync");
                return BadRequest();
            }
        }


        [HttpPost]
        public async Task<IActionResult> InsertarAsync([FromBody]NovedadOtd novedadOtd)
        {
            if (novedadOtd == null)
            {
                logger.Warning("Entidad para insertar vacia");
                return BadRequest();
            }

            try
            {
                await novedadAplicacion.InsertarAsync(novedadOtd).ConfigureAwait(false);
                logger.Information("Insertó: {@entidad}" + novedadOtd);
                return Ok();
            }
            catch (Exception err)
            {
                logger.Error(err, "Error insertando: {@entidad} ", novedadOtd);
                return BadRequest();
            }
        }

        [HttpPost]
        public async Task<IActionResult> ActualizarAsync([FromBody]NovedadOtd novedadOtd)
        {
            if (novedadOtd == null)
            {
                logger.Warning("Entidad para actualizar vacia");
                return BadRequest();
            }

            try
            {
                await novedadAplicacion.ActualizarAsync(novedadOtd).ConfigureAwait(false);
                logger.Information("Actualizó: {@entidad}", novedadOtd);
                return Ok();
            }
            catch (Exception err)
            {
                logger.Error(err, "Error actualizando: {@entidad}", novedadOtd);
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
                await novedadAplicacion.EliminarAsync(id).ConfigureAwait(false);
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
        [ProducesResponseType(typeof(NovedadOtd), StatusCodes.Status200OK)]
        public async Task<ActionResult<NovedadOtd>> ObtenerAsync(int id)
        {
            if (id < 1)
            {
                logger.Warning("Identificador para consultar no válido");
                return BadRequest();
            }

            try
            {
                var respuesta = await novedadAplicacion.ObtenerAsync(id).ConfigureAwait(false);
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