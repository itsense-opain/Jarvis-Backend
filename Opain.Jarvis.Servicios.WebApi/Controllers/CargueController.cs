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
    
    public class CargueController : ControllerBase
    {
        private readonly ICargueAplicacion cargueAplicacion;
        private readonly ILogger logger;
        private readonly Store.OperacionesVuelo Store_OperacionesVuelo;
        /// <summary>
        /// Cargue de archivos
        /// </summary>
        /// <param name="cargue"></param>
        /// <param name="l"></param>
        /// <param name="store_OperacionesVuelo"></param>
        public CargueController(ICargueAplicacion cargue, ILogger l,
            Store.OperacionesVuelo store_OperacionesVuelo)
        {
            cargueAplicacion = cargue;
            logger = l;
            this.Store_OperacionesVuelo = store_OperacionesVuelo;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IList<CargueOtd>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IList<CargueOtd>>> ObtenerTodosAsync(string inicio, string fin)
        {
            IList<CargueOtd> cargues = new List<CargueOtd>();
            try
            {
                DateTime fechaInicio;
                DateTime fechaFinal;

                if (!string.IsNullOrEmpty(inicio) && !string.IsNullOrEmpty(fin))
                {
                    var matrizFechaInicio = inicio.Split("/");
                    var matrizFechaFinal = fin.Split("/");

                    try
                    {
                        fechaInicio = new DateTime(int.Parse(matrizFechaInicio[2]), int.Parse(matrizFechaInicio[1]), int.Parse(matrizFechaInicio[0]));
                        fechaFinal = new DateTime(int.Parse(matrizFechaFinal[2]), int.Parse(matrizFechaFinal[1]), int.Parse(matrizFechaFinal[0]));
                        cargues = await cargueAplicacion.ObtenerTodosAsync(fechaInicio, fechaFinal).ConfigureAwait(false);
                        logger.Information("Consultó {@cantidad} registros", cargues.Count);
                    }
                    catch (Exception err)
                    {
                        logger.Error(err, "Error transformado fechas: {@fi}, {@ff}", inicio, fin);
                    }
                }


                return Ok(cargues);
            }
            catch (Exception err)
            {
                logger.Error(err, "Error consultando todos los cargues: {@fi}, {@ff}", inicio, fin);
                return BadRequest();
            }
        }
                             

        [HttpGet]
        //[ProducesResponseType(typeof(IList<CargueOtd>), StatusCodes.Status200OK)]
        public async Task<ActionResult<CargueOtd>> ConsultarIdCargueAsync(int idCargue)
        {
            CargueOtd cargues = new CargueOtd();
            try
            {

                if (idCargue>=0)
                {

                    try
                    {
                        cargues = this.Store_OperacionesVuelo.ConsultarIdCargue(idCargue);
                        logger.Information("Consultó {@cantidad} registros", cargues.NombreArchivo);
                    }
                    catch (Exception err)
                    {
                        //logger.Error(err, "Error consultadon cargue: {@fi}", cargues);
                    }
                }

                return Ok(cargues);
            }
            catch (Exception err)
            {
                logger.Error(err, "Error consultando todos los cargues: {@fi}", cargues);
                return BadRequest();
            }
        }





        /// <summary>
        /// /
        /// </summary>
        /// <param name="accesoOtd"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> InsertarAsync([FromBody]CargueOtd accesoOtd)
        {
            if (accesoOtd == null)
            {
                logger.Warning("Entidad para insertar vacia");
                return BadRequest();
            }

            try
            {
                CargueOtd cargue =await cargueAplicacion.InsertarAsync(accesoOtd).ConfigureAwait(false);
                logger.Information("Insertó: {@entidad}" + accesoOtd);
                return Ok(cargue);
            }
            catch (Exception err)
            {
                logger.Error(err, "Error insertando: {@entidad} ", accesoOtd);
                return BadRequest();
            }
        }
    }
}