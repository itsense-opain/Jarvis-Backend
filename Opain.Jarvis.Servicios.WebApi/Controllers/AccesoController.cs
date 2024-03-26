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
    public class AccesoController : ControllerBase
    {
        private readonly IAccesoAplicacion accesoAplicacion;
        private readonly ILogger logger;

        public AccesoController(IAccesoAplicacion acceso, ILogger l)
        {
            accesoAplicacion = acceso;
            logger = l;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IList<AccesoOtd>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IList<AccesoOtd>>> ObtenerTodosAsync(string inicio, string fin,string aerolinea)
        {
            IList<AccesoOtd> accesos = new List<AccesoOtd>();
            try
            {
                DateTime fechaInicio;
                DateTime fechaFinal;
                //int aerolineas;

                if (!string.IsNullOrEmpty(inicio) && !string.IsNullOrEmpty(fin) && !string.IsNullOrEmpty(aerolinea))
                {
                    var matrizFechaInicio = inicio.Split("/");
                    var matrizFechaFinal = fin.Split("/");

                    try
                    {
                        fechaInicio = new DateTime(int.Parse(matrizFechaInicio[2]), int.Parse(matrizFechaInicio[1]), int.Parse(matrizFechaInicio[0]));
                        fechaFinal = new DateTime(int.Parse(matrizFechaFinal[2]), int.Parse(matrizFechaFinal[1]), int.Parse(matrizFechaFinal[0]));
                        accesos = await accesoAplicacion.ObtenerTodosAsync(fechaInicio, fechaFinal, aerolinea).ConfigureAwait(false);
                        logger.Information("Consultó {@cantidad} registros", accesos.Count);
                    }
                    catch (Exception err)
                    {
                        logger.Error(err, "Error transformado fechas: {@fi}, {@ff}", inicio, fin);
                    }
                }

                
                return Ok(accesos);
            }
            catch (Exception err)
            {
                logger.Error(err, "Error consultando todos los accesos: {@fi}, {@ff}", inicio, fin);
                return BadRequest();
            }
        }

        [HttpPost]
        public async Task<IActionResult> InsertarAsync([FromBody]AccesoOtd accesoOtd)
        {
            if (accesoOtd == null)
            {
                logger.Warning("Entidad para insertar vacia");
                return BadRequest();
            }

            try
            {
                await accesoAplicacion.InsertarAsync(accesoOtd).ConfigureAwait(false);
                logger.Information("Insertó: {@entidad}" + accesoOtd);
                return Ok();
            }
            catch (Exception err)
            {
                logger.Error(err, "Error insertando: {@entidad} ", accesoOtd);
                return BadRequest();
            }
        }
    }
}