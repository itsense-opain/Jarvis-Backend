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
    public class ConsecutivoCargueController : ControllerBase
    {
        private readonly IConsecutivoCargueAplicacion consecutivoCargueAplicacion;
        private readonly ILogger logger;

        public ConsecutivoCargueController(IConsecutivoCargueAplicacion consecutivoCargue, ILogger l)
        {
            consecutivoCargueAplicacion = consecutivoCargue;
            logger = l;
        }

        [HttpPost]
        [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
        public async Task<ActionResult<int>> InsertarAsync([FromBody]ConsecutivoCargueOtd cargueOtd)
        {
            if (cargueOtd == null)
            {
                logger.Warning("Entidad para insertar vacia");
                return BadRequest();
            }

            try
            {
                var id = await consecutivoCargueAplicacion.InsertarAsync(cargueOtd).ConfigureAwait(false);
                logger.Information("Insertó: {@entidad}" + cargueOtd);
                return Ok(id);
            }
            catch (Exception err)
            {
                logger.Error(err, "Error insertando: {@entidad} ", cargueOtd);
                return BadRequest();
            }
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ConsecutivoCargueOtd), StatusCodes.Status200OK)]
        public async Task<ActionResult<ConsecutivoCargueOtd>> ObtenerAsync(int id)
        {
            if (id < 1)
            {
                logger.Warning("Identificador para consultar no válido");
                return BadRequest();
            }

            try
            {
                var respuesta = await consecutivoCargueAplicacion.ObtenerAsync(id).ConfigureAwait(false);
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