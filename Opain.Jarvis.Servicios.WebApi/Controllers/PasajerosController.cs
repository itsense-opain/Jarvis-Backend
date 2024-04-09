using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Opain.Jarvis.Aplicacion.Interfaces;
using Opain.Jarvis.Dominio.Entidades;
using Serilog;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Opain.Jarvis.Servicios.WebApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class PasajerosController : ControllerBase
    {
        private readonly IPasajeroAplicacion pasajeroAplicacion;
        private readonly ILogger logger;
        private readonly Store.OperacionesVuelo Store_OperacionesVuelo;

        public PasajerosController(IPasajeroAplicacion pas, ILogger l, Store.OperacionesVuelo store_OperacionesVuelo)
        {
            pasajeroAplicacion = pas;
            logger = l;
            this.Store_OperacionesVuelo = store_OperacionesVuelo;
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(IList<PasajeroOtd>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IList<PasajeroOtd>>> ObtenerTodosAsync(int id)
        {
            try
            {
                var respuesta = await pasajeroAplicacion.ObtenerTodosAsync(id);
                logger.Information("Se ejecutó PasajerosController.ObtenerTodosAsync");
                return Ok(respuesta);

            }
            catch (Exception err)
            {
                logger.Error(err, "Error en PasajerosController.ObtenerTodosAsync");
                return BadRequest();
            }
        }

        [HttpPost]
        public async Task<IActionResult> InsertarAsync([FromBody]PasajeroOtd pasajeroOtd)
        {
            if (pasajeroOtd == null)
            {
                logger.Warning("Entidad para insertar vacia");
                return BadRequest();
            }

            try
            {
                await pasajeroAplicacion.InsertarAsync(pasajeroOtd).ConfigureAwait(false);

                if (pasajeroOtd.Categoria.Equals("EX")) {
                    ExentoODT ex = new ExentoODT();
                    ex.id_vuelo = pasajeroOtd.Operacion.ToString();
                    ex.nombre = pasajeroOtd.NombrePasajero;
                    ex.tipo_exento = "EX";
                    ex.realiza_viaje = pasajeroOtd.Realiza_viaje;
                    ex.motivo_exencion = pasajeroOtd.Motivo_exencion;                    

                    this.Store_OperacionesVuelo.InsOrUpdExento(ex);
                }
                

                logger.Information("Insertó: {@entidad}" + pasajeroOtd);
                return Ok();
            }
            catch (Exception err)
            {
                logger.Error(err, "Error insertando: {@entidad} ", pasajeroOtd);
                return BadRequest();
            }
        }

        [HttpPost]
        public async Task<IActionResult> InsertarMasivoAsync([FromBody]IList<PasajeroOtd> pasajeroOtd)
        {
            if (pasajeroOtd == null)
            {
                logger.Warning("Entidad para insertar vacia");
                return BadRequest();
            }

            try
            {
                await pasajeroAplicacion.InsertarMasivoAsync(pasajeroOtd).ConfigureAwait(false);
                
                //foreach (var pax in pasajeroOtd)
                //{
                //    if (pax.Categoria.Equals("EX"))
                //    {
                //        ExentoODT ex = new ExentoODT();
                //        ex.id_vuelo = pax.Operacion.ToString();
                //        ex.nombre = pax.NombrePasajero;
                //        ex.tipo_exento = "EX";
                //        ex.realiza_viaje = "";
                //        ex.motivo_exencion = "";
                //        ex.id_pax = pax.Id;

                //        this.Store_OperacionesVuelo.InsOrUpdExento(ex);
                //    }
                //}
                
                logger.Information("Insertó {@cantidad} registros", pasajeroOtd.Count);
                return Ok();
            }
            catch (Exception err)
            {
                logger.Error(err, "Error insertando {@cantidad} registros", pasajeroOtd.Count);
                return BadRequest();
            }

        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(PasajeroOtd), StatusCodes.Status200OK)]
        public async Task<ActionResult<PasajeroOtd>> ObtenerAsync(int id)
        {
            try
            {
                var respuesta = await pasajeroAplicacion.ObtenerAsync(id);
                logger.Information("Se ejecutó PasajerosController.ObtenerAsync");
                return Ok(respuesta);

            }
            catch (Exception err)
            {
                logger.Error(err, "Error en PasajerosController.ObtenerAsync");
                return BadRequest();
            }
        }

        [HttpPost]
        public async Task<IActionResult> ActualizarAsync([FromBody]PasajeroOtd pasajeroOtd)
        {
            if (pasajeroOtd == null)
            {
                logger.Warning("Entidad para actualizar vacia");
                return BadRequest();
            }

            try
            {                

                await pasajeroAplicacion.ActualizarAsync(pasajeroOtd).ConfigureAwait(false);

                if (pasajeroOtd.Categoria.Equals("EX"))
                {
                    ExentoODT ex = new ExentoODT();
                    ex.id_vuelo = pasajeroOtd.Operacion.ToString();
                    ex.nombre = pasajeroOtd.NombrePasajero;
                    ex.tipo_exento = "EX";
                    ex.realiza_viaje = pasajeroOtd.Realiza_viaje;
                    ex.motivo_exencion = pasajeroOtd.Motivo_exencion;

                    this.Store_OperacionesVuelo.InsOrUpdExento(ex);
                }
                else {
                    this.Store_OperacionesVuelo.ActualizaExentos(pasajeroOtd.Id);
                }                

                logger.Information("Actualizó: {@entidad}", pasajeroOtd);
                return Ok();
            }
            catch (Exception err)
            {
                logger.Error(err, "Error actualizando: {@entidad}", pasajeroOtd);
                return BadRequest();
            }
        }

        [HttpPost]
        public async Task<IActionResult> ActualizarExtentosJAsync([FromBody]PasajeroOtd pasajeroOtd)
        {
            if (pasajeroOtd == null)
            {
                logger.Warning("Entidad para actualizar vacia");
                return BadRequest();
            }

            try
            {
                //await pasajeroAplicacion.ActualizarAsync(pasajeroOtd).ConfigureAwait(false);
                await ActualizarExtentosAsync(pasajeroOtd).ConfigureAwait(false);

                logger.Information("Actualizó: {@entidad}", pasajeroOtd);
                return Ok();
            }
            catch (Exception err)
            {
                logger.Error(err, "Error actualizando: {@entidad}", pasajeroOtd);
                return BadRequest();
            }
        }


        /// <summary>
        /// Consultar la tabla exentos.
        /// </summary>
        /// <param name="numerovuelo"></param>
        /// <param name="fecha"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(ExentoODT), StatusCodes.Status200OK)]
        public async Task<ActionResult<IList<ExentoODT>>> ObtenerExentosAsync(string numerovuelo, string fecha)
        {
            try
            {
                var respuesta = this.Store_OperacionesVuelo.TraerExento(numerovuelo, fecha,true);
                logger.Information("Se ejecutó PasajerosController.ObtenerExentosAsync");
                return Ok(respuesta);

            }
            catch (Exception err)
            {
                logger.Error(err, "Error en PasajerosController.ObtenerExentosAsync");
                return BadRequest();
            }
        }
        

        [HttpPost]
        public async Task<IActionResult> ActualizarExtentosAsync([FromBody]PasajeroOtd pasajeroOtd)
        {
            if (pasajeroOtd == null)
            {
                logger.Warning("Entidad para actualizar vacia");
                return BadRequest();
            }

            try
            {
                this.Store_OperacionesVuelo.ActualizarSpExento(pasajeroOtd);

                logger.Information("Actualizó: {@entidad}", pasajeroOtd);
                return Ok();
            }
            catch (Exception err)
            {
                logger.Error(err, "Error actualizando: {@entidad}", pasajeroOtd);
                return BadRequest();
            }
        }
    }
}