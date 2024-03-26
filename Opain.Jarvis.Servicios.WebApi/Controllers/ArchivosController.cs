using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Opain.Jarvis.Aplicacion.Interfaces;
using Opain.Jarvis.Dominio.Entidades;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Opain.Jarvis.Servicios.WebApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [AllowAnonymous]
    public class ArchivosController : ControllerBase
    {
        private readonly IArchivoAplicacion archivoAplicacion;
        private readonly ILogger logger;

        public ArchivosController(IArchivoAplicacion archivo, ILogger l)
        {
            archivoAplicacion = archivo;
            logger = l;
        }

        [HttpPost]
        public async Task<IActionResult> InsertarAsync([FromBody]ArchivoOtd archivoOtd)
        {
            if (archivoOtd == null)
            {
                logger.Warning("Entidad para insertar vacia");
                return BadRequest();
            }

            try
            {
                await archivoAplicacion.InsertarAsync(archivoOtd);
                logger.Information("Insertó {@entidad}" + archivoOtd);
                return Ok();
            }
            catch (Exception err)
            {
                logger.Error(err, "Error insertando {@entidad} ", archivoOtd);
                return BadRequest();
            }
        }

        [HttpPost]
        public async Task<IActionResult> InsertarMasivoAsync([FromBody]IList<ArchivoOtd> archivosOtd)
        {
            if (archivosOtd == null)
            {
                logger.Warning("Entidad para insertar vacia");
                return BadRequest();
            }

            try
            {
                await archivoAplicacion.InsertarMasivoAsync(archivosOtd);
                logger.Information("Insertó {@cantidad} registros", archivosOtd.Count);
                return Ok();
            }
            catch (Exception err)
            {
                logger.Error(err, "Error insertando {@cantidad} registros", archivosOtd.Count);
                return BadRequest();
            }
        }

        [HttpPost]
        public async Task<IActionResult> ActualizarAsync([FromBody]ArchivoOtd archivoOtd)
        {
            if (archivoOtd == null)
            {
                logger.Warning("Entidad para actualizar vacia");
                return BadRequest();
            }

            try
            {
                await archivoAplicacion.ActualizarAsync(archivoOtd);
                logger.Information("Actualizó: {@entidad}", archivoOtd);
                return Ok();
            }
            catch (Exception err)
            {
                logger.Error(err, "Error actualizando: {@entidad}", archivoOtd);
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
                await archivoAplicacion.EliminarAsync(id);
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
        [ProducesResponseType(typeof(ArchivoOtd), StatusCodes.Status200OK)]
        public async Task<ActionResult<ArchivoOtd>> ObtenerAsync(int id)
        {
            if (id < 1)
            {
                logger.Warning("Identificador para consultar no válido");
                return BadRequest();
            }

            try
            {
                var respuesta = await archivoAplicacion.ObtenerAsync(id);
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
        [ProducesResponseType(typeof(ArchivoOtd), StatusCodes.Status200OK)]
        public async Task<ActionResult<ArchivoOtd>> ObtenerPorOperacionAsync(int idOperacion)
        {
            try
            {
                var respuesta = await archivoAplicacion.ObtenerPorOperacionAsync(idOperacion);
                logger.Information("Consultó: {@entidad}", respuesta);
                return Ok(respuesta);
            }
            catch (Exception err)
            {
                logger.Error(err, "Error consultando id: {@id}", idOperacion);
                return BadRequest();
            }
        }

        [HttpPost]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        public async Task<ActionResult<bool>> ValidarAsync([FromBody]ArchivoOtd archivoOtd)
        {
            if (archivoOtd == null)
            {
                logger.Warning("Entidad para validar vacia");
                return BadRequest(false);
            }

            try
            {
                IList<ArchivoOtd> listadoArchivos = await archivoAplicacion.ObtenerPorOperacionAsync(archivoOtd.Operacion);

                if (listadoArchivos.Count > 0)
                {
                    foreach (var item in listadoArchivos.Where(x => x.Tipo.Equals(archivoOtd.Tipo)))
                    {
                        await archivoAplicacion.EliminarAsync(item.Id);
                    }
                }

                await archivoAplicacion.InsertarAsync(archivoOtd);
                logger.Information("Validó: {@entidad}", archivoOtd);
                return Ok(true);
            }
            catch (Exception err)
            {
                logger.Error(err, "Error validando: {@entidad}", archivoOtd);
                return BadRequest(false);
            }
        }

    }
}