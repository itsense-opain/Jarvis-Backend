
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Opain.Jarvis.Aplicacion.Interfaces;
using Opain.Jarvis.Dominio.Entidades;
using Opain.Jarvis.Servicios.WebApi.Helpers;
using Serilog;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.IO;
using System.IO.Compression;
using Microsoft.AspNetCore.Authorization;
using System.Data;
using System.Globalization;
using Newtonsoft.Json;

namespace Opain.Jarvis.Servicios.WebApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public partial class VuelosController : ControllerBase
    {
        private readonly IOperacionVueloAplicacion operacionVueloAplicacion;
        private readonly IAerolineaAplicacion aerolineaAplicacion;
        private readonly IUsuarioAplicacion usuarioAplicacion;
        private readonly ILogger logger;
        private readonly Store.OperacionesVuelo Store_OperacionesVuelo;
        private readonly IHttpContextAccessor HttpContextAccessor;
        private readonly Store.Metodos.OperacionesVuelo StoreProcedure;

        public VuelosController(IOperacionVueloAplicacion opv, ILogger l,
            Store.OperacionesVuelo store_OperacionesVuelo, IHttpContextAccessor
            httpContextAccessor, Microsoft.Extensions.Configuration.IConfiguration configuration,
            IAerolineaAplicacion aerolinea, IUsuarioAplicacion usuario,
            Store.Metodos.OperacionesVuelo store)
        {
            operacionVueloAplicacion = opv;
            logger = l.ForContext<VuelosController>();
            this.Store_OperacionesVuelo = store_OperacionesVuelo;
            this.HttpContextAccessor = httpContextAccessor;
            Configuration = configuration;
            aerolineaAplicacion = aerolinea;
            usuarioAplicacion = usuario;
            this.StoreProcedure = store;
        }
        public Microsoft.Extensions.Configuration.IConfiguration Configuration { get; }
        [HttpGet]
        public async Task<IActionResult> Validar(int id)
        {
            if (id < 1)
            {
                logger.Warning("Identificador para validar no válido");
                return BadRequest();
            }

            try
            {
                Store_OperacionesVuelo.ValidarVuelo(id);

                //llamamos validacion TT

                //List<int> ids = new List<int>();
                //ids.Add(id);
                //string resultado = await operacionVueloAplicacion.ConfirmarVuelosAsync(ids).ConfigureAwait(false);

                logger.Information("Validar id: {@id}", id);
                return Ok();
            }
            catch (Exception err)
            {
                logger.Error(err, "Error validar: {@id}", id);
                return BadRequest();
            }
        }

        [HttpPost]
        public async Task<IActionResult> InsertarAsync([FromBody] OperacionVueloOtd operacionVueloOtd)
        {
            if (operacionVueloOtd == null)
            {
                logger.Warning("Entidad para insertar vacia");
                return BadRequest();
            }

            try
            {
                operacionVueloOtd.EstadoProceso = "2";
                operacionVueloOtd.Id_Daily = "0";
                await operacionVueloAplicacion.InsertarAsync(operacionVueloOtd).ConfigureAwait(false);
                logger.Information("Insertó: {@entidad}" + operacionVueloOtd);
                return Ok();
            }
            catch (Exception err)
            {
                logger.Error(err, "Error insertando: {@entidad} ", operacionVueloOtd);
                return BadRequest();
            }
        }

        [HttpPost]
        public async Task<IActionResult> InsertarMasivoAsync([FromBody] IList<OperacionVueloOtd> operacionesVueloOtd)
        {
            if (operacionesVueloOtd == null)
            {
                logger.Warning("Entidad para insertar vacia");
                return BadRequest();
            }

            try
            {
                await operacionVueloAplicacion.InsertarMasivoAsync(operacionesVueloOtd).ConfigureAwait(false);
                logger.Information("Insertó {@cantidad} registros", operacionesVueloOtd.Count);
                return Ok();
            }
            catch (Exception err)
            {
                logger.Error(err, "Error insertando {@cantidad} registros", operacionesVueloOtd.Count);
                return BadRequest();
            }

        }

        [HttpPost]
        public async Task<IActionResult> ActualizarAsync([FromBody] OperacionVueloOtd operacionVueloOtd)
        {
            if (operacionVueloOtd == null)
            {
                logger.Warning("Entidad para actualizar vacía");
                return BadRequest();
            }

            try
            {
                await operacionVueloAplicacion.ActualizarAsync(operacionVueloOtd).ConfigureAwait(false);
                logger.Information("Actualizó: {@entidad}", operacionVueloOtd);
                // OperacionVuelos_Validar(operacionVueloOtd.Id);  estas validaciones no van aqui
                return Ok();
            }
            catch (Exception err)
            {
                logger.Error(err, "Error actualizando: {@entidad}", operacionVueloOtd);
                return BadRequest();
            }
        }

        private void OperacionVuelos_Validar(int id)
        {
            this.Store_OperacionesVuelo.OperacionVuelos_Validar(id);
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
                await operacionVueloAplicacion.EliminarAsync(id).ConfigureAwait(false);
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
        [ProducesResponseType(typeof(OperacionVueloOtd), StatusCodes.Status200OK)]
        public async Task<ActionResult<OperacionVueloOtd>> ObtenerAsync(int id)
        {
            if (id < 1)
            {
                logger.Warning("Identificador para consultar no válido");
                return BadRequest();
            }

            try
            {
                var respuesta = await operacionVueloAplicacion.ObtenerAsync(id).ConfigureAwait(false);
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
        public ActionResult TraerDatosValidacionManual(int id)
        {

            int CantPasajeros = 0;
            int CantTransitos = 0;
            int CantInfantes = 0;
            int CantTTL = 0;
            int CantTTC = 0;
            int CantEX = 0;
            int CantTRIP = 0;
            decimal TasaCOP = 0;
            decimal TasaUSD = 0;

            this.Store_OperacionesVuelo.TraerDatosValidacionManual(id, out CantPasajeros, out CantTransitos, out CantInfantes, out CantTTL, out CantTTC, out CantEX, out CantTRIP, out TasaCOP, out TasaUSD);

            Dominio.Entidades.RespuestaValidacionManual Respuesta = new Dominio.Entidades.RespuestaValidacionManual();

            Respuesta.CantPasajeros = CantPasajeros;
            Respuesta.CantTransitos = CantTransitos;
            Respuesta.CantInfantes = CantInfantes;
            Respuesta.CantTTL = CantTTL;
            Respuesta.CantTTC = CantTTC;
            Respuesta.CantEX = CantEX;
            Respuesta.CantTRIP = CantTRIP;
            Respuesta.TasaCOP = TasaCOP;
            Respuesta.TasaUSD = TasaUSD;


            return Ok(Respuesta);
        }

        [HttpPost]
        public ActionResult InsertValidacionManual(Dictionary<string, string> formCollection)
        {
            if (formCollection.Where(c => c.Key.Equals("idVuelo")).Count() == 0)
            {
                return BadRequest();
            }

            int idValidaManual = 0;

            if (!int.TryParse(formCollection["idVuelo"], out int idVuelo))
            {
                return BadRequest();
            }
            if (idVuelo <= 0)
            {
                return BadRequest();
            }

            /*
            if (!int.TryParse(formCollection["CantPasajeros_Old"], out int CantPasajeros_Old))
            {
                CantPasajeros_Old = 0;
            }

            if (!int.TryParse(formCollection["CantPasajeros_New"], out int CantPasajeros_New))
            {
                CantPasajeros_New = 0;
            }
            
            string CausalPasajeros = formCollection["CausalPasajeros"].ToString();

            if (!int.TryParse(formCollection["CantTransitos_Old"], out int CantTransitos_Old))
            {
                CantTransitos_Old = 0;
            }

            if (!int.TryParse(formCollection["CantTransitos_New"], out int CantTransitos_New))
            {
                CantTransitos_New = 0;
            }
            
            string CausalTransitos = formCollection["CausalTransitos"].ToString();

            if (!int.TryParse(formCollection["CantInfantes_Old"], out int CantInfantes_Old))
            {
                CantInfantes_Old = 0;
            }

            if (!int.TryParse(formCollection["CantInfantes_New"], out int CantInfantes_New))
            {
                CantInfantes_New = 0;
            }
            
            string CausalInfantes = formCollection["CausalInfantes"].ToString();

            if (!int.TryParse(formCollection["CantTTL_Old"], out int CantTTL_Old))
            {
                CantTTL_Old = 0;
            }

            if (!int.TryParse(formCollection["CantTTL_New"], out int CantTTL_New))
            {
                CantTTL_New = 0;
            }
            
            string CausalTTL = formCollection["CausalTTL"].ToString();

            if (!int.TryParse(formCollection["CantTTC_Old"], out int CantTTC_Old))
            {
                CantTTC_Old = 0;
            }

            if (!int.TryParse(formCollection["CantTTC_New"], out int CantTTC_New))
            {
                CantTTC_New = 0;
            }

            string CausalTTC = formCollection["CausalTTC"].ToString();

            if (!int.TryParse(formCollection["CantEX_Old"], out int CantEX_Old))
            {
                CantEX_Old = 0;
            }

            if (!int.TryParse(formCollection["CantEX_New"], out int CantEX_New))
            {
                CantEX_New = 0;
            }
            
            string CausalEX = formCollection["CausalEX"].ToString();

            if (!int.TryParse(formCollection["CantTRIP_Old"], out int CantTRIP_Old))
            {
                CantTRIP_Old = 0;
            }

            if (!int.TryParse(formCollection["CantTRIP_New"], out int CantTRIP_New))
            {
                CantTRIP_New = 0;
            }

            string CausalTRIP = formCollection["CausalTRIP"].ToString();

            if (!int.TryParse(formCollection["CantPagoCOP_Old"], out int CantPagoCOP_Old))
            {
                CantPagoCOP_Old = 0;
            }

            if (!int.TryParse(formCollection["CantPagoCOP_New"], out int CantPagoCOP_New))
            {
                CantPagoCOP_New = 0;
            }
            

            string CausalPagoCOP = formCollection["CausalPagoCOP"].ToString();

            if (!int.TryParse(formCollection["CantPagoUSD_Old"], out int CantPagoUSD_Old))
            {
                CantPagoUSD_Old = 0;
            }

            if (!int.TryParse(formCollection["CantPagoUSD_New"], out int CantPagoUSD_New))
            {
                CantPagoUSD_New = 0;
            }
            */

            string CausalPagoUSD = formCollection["CausalPagoUSD"].ToString();

            this.Store_OperacionesVuelo.InsertValidaManual
                                  (idVuelo, formCollection["CantPasajeros_Old"], formCollection["CantPasajeros_New"], formCollection["CausalPasajeros"],
                                        formCollection["CantTransitos_Old"], formCollection["CantTransitos_New"], formCollection["CausalTransitos"],
                                        formCollection["CantInfantes_Old"], formCollection["CantInfantes_New"], formCollection["CausalInfantes"],
                                        formCollection["CantTTL_Old"], formCollection["CantTTL_New"], formCollection["CausalTTL"],
                                        formCollection["CantTTC_Old"], formCollection["CantTTC_New"], formCollection["CausalTTC"],
                                        formCollection["CantEX_Old"], formCollection["CantEX_New"], formCollection["CausalEX"],
                                        formCollection["CantTRIP_Old"], formCollection["CantTRIP_New"], formCollection["CausalTRIP"],
                                        formCollection["CantPagoCOP_Old"], formCollection["CantPagoCOP_New"], formCollection["CausalPagoCOP"],
                                        formCollection["CantPagoUSD_Old"], formCollection["CantPagoUSD_New"], formCollection["CausalPagoUSD"]
                                     );

            return Ok();
        }

        [HttpGet]
        [ProducesResponseType(typeof(Dominio.Entidades.RespuestaValidacionManual), StatusCodes.Status200OK)]
        public async Task<ActionResult<Dominio.Entidades.RespuestaValidacionManual>> TraerDatosValidacionManual1(int id)
        {

            int CantPasajeros = 0;
            int CantTransitos = 0;
            int CantInfantes = 0;
            int CantTTL = 0;
            int CantTTC = 0;
            int CantEX = 0;
            int CantTRIP = 0;
            decimal TasaCOP = 0;
            decimal TasaUSD = 0;

            this.Store_OperacionesVuelo.TraerDatosValidacionManual(id, out CantPasajeros, out CantTransitos, out CantInfantes, out CantTTL, out CantTTC, out CantEX, out CantTRIP, out TasaCOP, out TasaUSD);

            Dominio.Entidades.RespuestaValidacionManual Respuesta = new Dominio.Entidades.RespuestaValidacionManual();

            Respuesta.CantPasajeros = CantPasajeros;
            Respuesta.CantTransitos = CantTransitos;
            Respuesta.CantInfantes = CantInfantes;
            Respuesta.CantTTL = CantTTL;
            Respuesta.CantTTC = CantTTC;
            Respuesta.CantEX = CantEX;
            Respuesta.CantTRIP = CantTRIP;
            Respuesta.TasaCOP = TasaCOP;
            Respuesta.TasaUSD = TasaUSD;


            return Respuesta;
        }

        [HttpGet]
        [ProducesResponseType(typeof(Dominio.Entidades.RespuestaGrilla<OperacionVueloOtd>), StatusCodes.Status200OK)]
        public async Task<ActionResult<Dominio.Entidades.RespuestaGrilla<OperacionVueloOtd>>> PaginarVuelosPendientes(int idAerolinea, int pagina, int registroPorPagina)
        {
            Dominio.Entidades.RespuestaGrilla<OperacionVueloOtd> Respuesta = new Dominio.Entidades.RespuestaGrilla<OperacionVueloOtd>();

            Respuesta.Resultado = this.Store_OperacionesVuelo.TraerPendientesConfirmar(idAerolinea, pagina, registroPorPagina, out int Registros);
            Respuesta.CantidadRegistros = Registros;

            var regPag = registroPorPagina == default ? 10 : registroPorPagina;
            int Resto = Registros % regPag;
            Resto = Resto > 0 ? 1 : 0;
            Respuesta.CantidadPaginas = (Registros / regPag) + Resto;

            return Ok(Respuesta);
        }
        [HttpGet]
        [ProducesResponseType(typeof(Dominio.Entidades.RespuestaGrilla<OperacionVueloOtd>), StatusCodes.Status200OK)]
        public async Task<ActionResult<Dominio.Entidades.RespuestaGrilla<OperacionVueloOtd>>> PaginarVuelosPendientesSoporte(int idAerolinea, int pagina, int registroPorPagina)
        {
            Dominio.Entidades.RespuestaGrilla<OperacionVueloOtd> Respuesta = new Dominio.Entidades.RespuestaGrilla<OperacionVueloOtd>();

            Respuesta.Resultado = this.Store_OperacionesVuelo.TraerPendientesConfirmarSoporte(idAerolinea, pagina, registroPorPagina, out int Registros);
            Respuesta.CantidadRegistros = Registros;

            var regPag = registroPorPagina == default ? 10 : registroPorPagina;
            int Resto = Registros % regPag;
            Resto = Resto > 0 ? 1 : 0;
            Respuesta.CantidadPaginas = (Registros / regPag) + Resto;

            return Ok(Respuesta);
        }

        [HttpGet]
        [ProducesResponseType(typeof(Dominio.Entidades.RespuestaGrilla<OperacionVueloOtd>), StatusCodes.Status200OK)]
        public async Task<ActionResult<Dominio.Entidades.RespuestaGrilla<OperacionVueloOtd>>> PaginarVuelosConfirmados(int idAerolinea, int pagina, int registroPorPagina)
        {
            Dominio.Entidades.RespuestaGrilla<OperacionVueloOtd> Respuesta = new Dominio.Entidades.RespuestaGrilla<OperacionVueloOtd>();

            Respuesta.Resultado = this.Store_OperacionesVuelo.TraerVuelosConfirmados(idAerolinea, pagina, registroPorPagina, out int Registros);
            Respuesta.CantidadRegistros = Registros;

            int Resto = Registros % registroPorPagina;
            Resto = Resto > 0 ? 1 : 0;
            Respuesta.CantidadPaginas = (Registros / registroPorPagina) + Resto;

            return Ok(Respuesta);
        }

        [HttpGet]
        [ProducesResponseType(typeof(IList<OperacionVueloOtd>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IList<OperacionVueloOtd>>> ObtenerTodosAsync(string fechaVueloInicio, string fechaVueloFinal, string bandera = "0")
        {
            try
            {
                int dia = DateTime.Now.Day;
                int mes = DateTime.Now.Month;
                int anio = DateTime.Now.Year;

                DateTime fechaInicio = new DateTime();
                DateTime fechaFinal = new DateTime();

                if (!string.IsNullOrEmpty(fechaVueloInicio) && !string.IsNullOrEmpty(fechaVueloFinal))
                {
                    var matrizFechaInicio = fechaVueloInicio.Split("-");
                    var matrizFechaFinal = fechaVueloFinal.Split("-");
                    try
                    {
                        fechaInicio = new DateTime(int.Parse(matrizFechaInicio[0]), int.Parse(matrizFechaInicio[1]), int.Parse(matrizFechaInicio[2]), 0, 0, 0);
                        fechaFinal = new DateTime(int.Parse(matrizFechaFinal[0]), int.Parse(matrizFechaFinal[1]), int.Parse(matrizFechaFinal[2]), 0, 0, 0);
                    }
                    catch (Exception)
                    {
                        if (dia > 1 && dia < 17)
                        {
                            fechaInicio = new DateTime(anio, mes, 1, 0, 0, 0);
                            fechaFinal = new DateTime(anio, mes, 17, 0, 0, 0);
                        }
                        else
                        {
                            fechaInicio = new DateTime(anio, mes, 16, 0, 0, 0);

                            if (mes == 12)
                            {
                                fechaFinal = new DateTime(anio + 1, 1, 02);
                            }
                            else
                            {
                                fechaFinal = new DateTime(anio, mes + 1, 02);
                            }
                        }
                    }
                }
                else
                {

                    //fechaInicio = DateTime.Now.AddMonths(mese);

                    string value = string.Empty;
                    if (Configuration.GetSection("ParametroFechas:Meses").Value != null)
                    {
                        value = Configuration.GetSection("ParametroFechas:Meses").Value;
                        fechaInicio = DateTime.Now.AddDays(int.Parse(value));
                    }
                    fechaFinal = DateTime.Now.AddDays(5);

                }
                IList<OperacionVueloOtd> vuelos = await operacionVueloAplicacion.ObtenerTodosAsync(fechaInicio, fechaFinal, bandera).ConfigureAwait(false);
                logger.Information("Consultó {@cantidad} registros", vuelos.Count);
                return Ok(vuelos);
            }
            catch (Exception err)
            {
                logger.Error(err, "Error consultando todos los vuelos: {@fi}, {@ff}", fechaVueloInicio, fechaVueloFinal);
                return BadRequest();
            }
        }

        [HttpGet]
        [ProducesResponseType(typeof(IList<OperacionVueloOtd>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IList<OperacionVueloOtd>>> ObtenerTodosAerolineaAsync(int IDAerolinea, string fechaVueloInicio, string fechaVueloFinal, string bandera = "0", string tipoVueloHistorico = "")
        {
            try
            {
                int dia = DateTime.Now.Day;
                int mes = DateTime.Now.Month;
                int anio = DateTime.Now.Year;

                DateTime fechaInicio = DateTime.Now;
                DateTime fechaFinal = DateTime.Now;
                if (fechaVueloInicio != null && fechaVueloFinal != null)
                {
                    if (!string.IsNullOrEmpty(fechaVueloInicio.ToString()) && !string.IsNullOrEmpty(fechaVueloFinal.ToString()))
                    {
                        var matrizFechaInicio = fechaVueloInicio.ToString().Split("-");
                        var matrizFechaFinal = fechaVueloFinal.ToString().Split("-");
                        try
                        {
                            fechaInicio = new DateTime(int.Parse(matrizFechaInicio[0]), int.Parse(matrizFechaInicio[1]), int.Parse(matrizFechaInicio[2]), 0, 0, 0);
                            fechaFinal = new DateTime(int.Parse(matrizFechaFinal[0]), int.Parse(matrizFechaFinal[1]), int.Parse(matrizFechaFinal[2]), 0, 0, 0);
                        }
                        catch (Exception ex)
                        {

                            try
                            {
                                fechaInicio = new DateTime(int.Parse(matrizFechaInicio[2]), int.Parse(matrizFechaInicio[1]), int.Parse(matrizFechaInicio[0]), 0, 0, 0);
                                fechaFinal = new DateTime(int.Parse(matrizFechaFinal[2]), int.Parse(matrizFechaFinal[1]), int.Parse(matrizFechaFinal[0]), 0, 0, 0);

                            }
                            catch (Exception)
                            {

                                if (dia > 1 && dia < 17)
                                {
                                    fechaInicio = new DateTime(anio, mes, 1, 0, 0, 0);
                                    fechaFinal = new DateTime(anio, mes, 17, 0, 0, 0);
                                }
                                else
                                {
                                    fechaInicio = new DateTime(anio, mes, 16, 0, 0, 0);

                                    if (mes == 12)
                                    {
                                        fechaFinal = new DateTime(anio + 1, mes + 1, 02);
                                    }
                                    else
                                    {
                                        fechaFinal = new DateTime(anio, mes + 1, 02);
                                    }
                                }
                            }
                        }
                    }






                }
                else
                {
                    var diaactual = DateTime.Now.Day;
                    int diainicial = 15;

                    fechaInicio = DateTime.Now.AddDays(-diainicial).Date;
                    fechaFinal = DateTime.Now.AddDays(5).Date;
                }
                IList<OperacionVueloOtd> vuelos = await operacionVueloAplicacion.ObtenerTodosAerolineaAsync(IDAerolinea, fechaInicio, fechaFinal, bandera, tipoVueloHistorico).ConfigureAwait(false);
                logger.Information("Consultó {@cantidad} registros", vuelos.Count);
                return Ok(vuelos);
            }
            catch (Exception err)
            {
                logger.Error(err, "Error consultando todos los vuelos: {@fi}, {@ff}", fechaVueloInicio, fechaVueloFinal);
                return BadRequest();
            }
        }

        [HttpPost]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        public async Task<ActionResult<string>> ConfirmarVuelosAsync([FromBody] List<int> ids)
        {
            if (ids == null || ids.Count == 0)
            {
                logger.Warning("Entidad para insertar vacia");
                return BadRequest();
            }

            try
            {

                string resultado = await operacionVueloAplicacion.ConfirmarVuelosAsync(ids).ConfigureAwait(false);
                logger.Information("Confirmo {@cantidad} registros", ids.Count);
                return Ok(resultado);
            }
            catch (Exception err)
            {
                logger.Error(err, "Error confirmando {@cantidad} registros", ids.Count);
                return BadRequest();
            }

        }

        [HttpGet]
        public async Task<ActionResult> VuelosPendientesConfirmarObtenerTodos(int id)
        {
            if (id < 1)
            {
                logger.Warning("Identificador para consultar no válido");
                return BadRequest();
            }

            List<OperacionVueloOtd> Respuesta = new List<OperacionVueloOtd>();

            Respuesta = this.Store_OperacionesVuelo.VuelosPendientesConfirmarObtenerTodos(id);

            return Ok(Respuesta);

        }


        [HttpGet]
        public async Task<ActionResult> VuelosPendientesProcesados(int id, string fecha)
        {
            if (id < 1)
            {
                logger.Warning("Identificador para consultar no válido");
                return BadRequest();
            }



            int Respuesta = this.Store_OperacionesVuelo.VuelosPendientesProcesados(id, fecha);

            return Ok(Respuesta);

        }


        [HttpPost]
        public async Task<IActionResult> InsertaCargueVuelos(OperacionVueloOtd operacionVuelo)
        {
            this.Store_OperacionesVuelo.InsertaCargueVuelos(operacionVuelo, "NO");
            return Ok(operacionVuelo);
        }

        [HttpPost]
        public async Task<IActionResult> InsertaCargueContingencia(OperacionVueloOtd operacionVuelo)
        {
            this.Store_OperacionesVuelo.InsertaCargueVuelos(operacionVuelo, "SI");
            return Ok();
        }

        private void CopyAll(DirectoryInfo source, DirectoryInfo target)
        {
            Directory.CreateDirectory(target.FullName);
            foreach (FileInfo fi in source.GetFiles())
            {
                Console.WriteLine(@"Copying {0}\{1}", target.FullName, fi.Name);
                fi.CopyTo(Path.Combine(target.FullName, fi.Name), true);
            }
            foreach (DirectoryInfo diSourceSubDir in source.GetDirectories())
            {
                DirectoryInfo nextTargetSubDir =
                    target.CreateSubdirectory(diSourceSubDir.Name);
                CopyAll(diSourceSubDir, nextTargetSubDir);
            }
        }
        private void Copy(string sourceDirectory, string targetDirectory)
        {
            var diSource = new DirectoryInfo(sourceDirectory);
            var diTarget = new DirectoryInfo(targetDirectory);
            CopyAll(diSource, diTarget);
        }

        /// <summary>
        /// Informacion de Api Traer zip con sus respectivos parametros
        /// </summary>
        /// <param name="idAerolinea"></param>
        /// <param name="NombreAerolinea"></param>
        /// <param name="TodasAerolinea"></param>
        /// <param name="fechafinal"></param>
        /// <param name="fechainicial"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> TraerZips(string idAerolinea, string NombreAerolinea, string TodasAerolinea, string fechafinal, string fechainicial)
        {
            string rutaArchivos = "";
            string rutaArchivosSalida = "";
            int existe = 0;
            string rutaTemp = string.Empty;
            string NombreZip = "";

            logger.Error("0. Parametros Fecha : " + Convert.ToDateTime(fechainicial).ToString() + " " + Convert.ToDateTime(fechafinal).ToString());

            IList<AerolineaOtd> respuestaaero = new List<AerolineaOtd>();
            try
            {
                rutaArchivos = Configuration.GetSection("Config:RutaArchivos").Value;
                rutaArchivosSalida = rutaArchivos;
                existe = 0;

                rutaTemp = Configuration.GetSection("Config:RutaArchivos").Value;
                rutaTemp = rutaTemp + "//Temp";
                if (Directory.Exists(rutaTemp))
                {
                    Directory.Delete(rutaTemp, true);
                }

                string[] files = Directory.GetFiles(rutaArchivosSalida, "*.zip");

                foreach (string file in files)

                    System.IO.File.Delete(file);

                // Busco los archivos en esa ruta de esa aerolinea y fecha
                if (idAerolinea == "0")
                {
                    var aerolinea = await aerolineaAplicacion.ObtenerTodosAsync().ConfigureAwait(false);

                    respuestaaero = aerolinea.Where(x => x.IdEstado.Equals("True")).ToList();
                    NombreZip = "Todas.zip";
                }
                else
                {
                    Task<IList<AerolineaOtd>> _aerolineaAplicacion = aerolineaAplicacion.ObtenerHorarioIdAerolineaAsync(Convert.ToInt32(idAerolinea));
                    respuestaaero = _aerolineaAplicacion.Result;
                    NombreZip = idAerolinea + ".zip";
                }
            }
            catch (Exception ExceptionPrerequisitos)
            {
                logger.Error(ExceptionPrerequisitos.Message, "For respuestaaero ");
                return Ok("No hay datos en la fecha seleccionada. Excepcion");
            }

            foreach (var item in respuestaaero)
            {
                try
                {
                    rutaArchivos = Configuration.GetSection("Config:RutaArchivos").Value;
                    rutaArchivos = rutaArchivos + "/" + item.Sigla.TrimEnd();
                    if (Directory.Exists(rutaArchivos))
                    {
                        // Busco los archivos de esas fechas saco dias entre esas fechas
                        TimeSpan difFechas = Convert.ToDateTime(fechafinal) - Convert.ToDateTime(fechainicial);
                        int dias = difFechas.Days;
                        DateTime limiteinicial = Convert.ToDateTime(fechainicial);
                        if (!Directory.Exists(rutaTemp))
                        {
                            Directory.CreateDirectory(rutaTemp);
                        }

                        for (int i = 0; i <= dias; i++)
                        {
                            logger.Information("1. Ruta de archivos : " + rutaArchivos);
                            string rutaparcial = rutaArchivos;
                            string rutaAerolineaFecha = String.Format("{0}{1}{2}", limiteinicial.Year, limiteinicial.Month.ToString().PadLeft(2, '0'), limiteinicial.Day.ToString().PadLeft(2, '0'));
                            rutaparcial = rutaparcial + "/" + rutaAerolineaFecha;
                            logger.Information("2. Ruta parcial : " + rutaparcial);
                            try
                            {
                                if (Directory.Exists(rutaparcial))
                                {
                                    //string[] allfiles = Directory.GetDirectories(rutaparcial);

                                    //foreach (string f in allfiles)
                                    //{
                                    //    string files = f.Replace("\\", "/");
                                    //    logger.Information("3. archivo : " + files);
                                    //    try
                                    //    {
                                    //        int idVuelo = 0;
                                    //        DirectoryInfo di = new DirectoryInfo(files);
                                    //        string vuelo = files.Split("/")[3];
                                    //        if (di.GetFiles().Where(x => x.Name.Contains("MANIFIESTO")).Count() > 0)
                                    //        {
                                    //            idVuelo = Convert.ToInt32(di.GetFiles().Where(x => x.Name.Contains("MANIFIESTO")).FirstOrDefault().Name.Split("_")[1].Split(".")[0]);
                                    //            logger.Error("3.1 Numero de vuelo : " + idVuelo.ToString());
                                    //        }
                                    //        if (di.GetFiles().Where(x => x.Name.Contains("Transitos")).Count() == 0)
                                    //        {
                                    //            if (idVuelo > 0)
                                    //            {
                                    //                await ArchivosTransitos(idVuelo, files, rutaAerolineaFecha);
                                    //                logger.Error("3.2 rutaaerolinea : " + rutaAerolineaFecha);
                                    //            }
                                    //        }
                                    //        if (di.GetFiles().Where(x => x.Name.Contains("CargaVuelos")).Count() == 0)
                                    //        {
                                    //            if (idVuelo > 0)
                                    //            {
                                    //                await archivosCargaVuelo(idVuelo, files, rutaAerolineaFecha);
                                    //                logger.Error("3.3 rutaaerolinea : " + rutaAerolineaFecha);
                                    //            }
                                    //        }
                                    //    }
                                    //    catch (Exception Excepcion)
                                    //    {
                                    //        logger.Error("3. archivo excepcion : " + files.ToString() + " " + Excepcion.Message);
                                    //    }

                                    existe = 1;
                                    if (!Directory.Exists(rutaTemp + "//" + rutaAerolineaFecha + "//" + item.Nombre.TrimEnd()))
                                    {
                                        Directory.CreateDirectory(rutaTemp + "//" + rutaAerolineaFecha + "//" + item.Nombre.TrimEnd());
                                    }
                                    DirectoryCopy(rutaparcial, rutaTemp + "//" + rutaAerolineaFecha + "//" + item.Nombre.TrimEnd(), true);


                                    // Añadase la carpeta a la temporal
                                }
                                limiteinicial = limiteinicial.AddDays(1);
                            }
                            catch (Exception ExcepcionFor)
                            {
                                logger.Error("2. Ruta parcial excepcion : " + ExcepcionFor.Message);
                            }
                        }

                    }

                }
                catch (Exception ExceptionFor)
                {
                    logger.Error(ExceptionFor.Message, "For respuestaaero ");
                }
            }

            byte[] fileBytes = new byte[0];

            try
            {
                // La carpeta temporal se zipea y luego se elimina
                string startPath = rutaTemp;

                string zipPath = rutaArchivosSalida + "\\" + NombreZip.TrimEnd();
                if (System.IO.File.Exists(zipPath))
                {
                    System.IO.File.Delete(zipPath);
                }

                if (existe == 0)
                {
                    return Ok("No hay datos en la fecha seleccionada");
                }
                else
                {
                    ZipFile.CreateFromDirectory(startPath, zipPath);
                    // Descarga el archivo en el explorador:)
                    fileBytes = System.IO.File.ReadAllBytes(zipPath);
                    //fileName = "DatosCargados.zip";                                      
                }
            }
            catch (Exception ExceptionCreateFile)
            {
                logger.Error(ExceptionCreateFile.Message, "Generacion del ZIP fallo ");
                return Ok("Se presento una falla al generar el archivo Zip");
            }
            logger.Error("Se genero el archivo " + NombreZip, "Generacion del ZIP exitoso ");
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, NombreZip);
        }
        [HttpGet]
        public void DirectoryCopy(string sourceDirName, string destDirName, bool copySubDirs)
        {
            // Get the subdirectories for the specified directory.
            DirectoryInfo dir = new DirectoryInfo(sourceDirName);
            DirectoryInfo[] dirs = dir.GetDirectories();

            if (!dir.Exists)
            {
                throw new DirectoryNotFoundException(
                    "Source directory does not exist or could not be found: "
                    + sourceDirName);
            }

            // If the destination directory doesn't exist, create it.
            if (!Directory.Exists(destDirName))
            {
                Directory.CreateDirectory(destDirName);
            }

            // Get the files in the directory and copy them to the new location.
            FileInfo[] files = dir.GetFiles();
            foreach (FileInfo file in files)
            {
                string temppath = Path.Combine(destDirName, file.Name);
                file.CopyTo(temppath, false);
            }

            // If copying subdirectories, copy them and their contents to new location.
            if (copySubDirs)
            {
                foreach (DirectoryInfo subdir in dirs)
                {
                    string temppath = Path.Combine(destDirName, subdir.Name);
                    DirectoryCopy(subdir.FullName, temppath, copySubDirs);
                }
            }
        }



        [HttpGet]
        [ProducesResponseType(typeof(DataTable), StatusCodes.Status200OK)]
        public async Task<ActionResult<DataTable>> TraerInformeCobro(int idAerolinea, string numVuelo, string fechaIni, string fechaFin)
        {
            CultureInfo cultureinfo = new CultureInfo("en-gb");
            DateTime tempDateIni = Convert.ToDateTime(fechaIni, cultureinfo);
            DateTime tempDateFin = Convert.ToDateTime(fechaFin, cultureinfo);

            var resultado = this.Store_OperacionesVuelo.TraerInformeCobro(idAerolinea, numVuelo,
                tempDateIni,
                tempDateFin.AddDays(1).AddTicks(-1));
            return Ok(resultado);
        }

        [HttpPost]
        public async Task<IActionResult> GuardarAuditoriaCargue(string usuario, string Evento, int Idvuelo)
        {
            this.Store_OperacionesVuelo.GuardarAuditoriaCargue(usuario, Evento, Idvuelo);
            return Ok();
        }


        [HttpGet]
        public async Task<ActionResult> archivosCargaVuelo(int idVueloOp, string Ruta, string Fecha)
        {
            var Respuesta = this.Store_OperacionesVuelo.archivosCargaVuelo(idVueloOp);

            String CadenaDataTable = "";

            foreach (DataRow row in Respuesta.Rows)
            {

                CadenaDataTable += Convert.ToDateTime(row["FechaVuelo"]).ToString("dd/MM/yyyy") + "," + row["TipoVuelo"] + "," + row["MatriculaVuelO"] + "," + row["NumeroVuelo"] + "," + row["Destino"] + "," + row["HoraVuelo"].ToString().Replace(":", "") + "," + row["TotalEmbarcados"] + "," + row["INF"] + "," + row["TTL"] + "," + row["TTC"] + "," + row["EX"] + "," + row["TRIP"] + "," + row["PagoCOP"] + "," + row["PagoUSD"];

            }

            //StreamWriter file = new StreamWriter(Ruta + "\\CargaVuelos" + Fecha.Split("/")[1] + ".txt", true);
            StreamWriter file = new StreamWriter(Ruta + "\\CargaVuelos" + Fecha + ".txt", true);
            file.WriteLine(CadenaDataTable);
            file.Close();

            return Ok(Respuesta);

        }

        [HttpGet]
        public async Task<ActionResult> ArchivosTransitos(int idVueloOp, string Ruta, string Fecha)
        {

            var Respuesta = this.Store_OperacionesVuelo.ArchivosTransitos(idVueloOp);

            if (Respuesta.Rows.Count == 0)
            {
                return Ok(Respuesta);
            }

            String CadenaDataTable = "";

            for (int i = 0; i < Respuesta.Rows.Count; i++)
            {
                if (i == 0)
                {
                    CadenaDataTable += string.Format("{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11}", Environment.NewLine, Convert.ToDateTime(Respuesta.Rows[i][0]).ToString("dd/MM/yyyy"), Respuesta.Rows[i][1].ToString().Replace(":", ""), Respuesta.Rows[i][2], Respuesta.Rows[i][3], Convert.ToDateTime(Respuesta.Rows[i][4]).ToString("dd/MM/yyyy"), Respuesta.Rows[i][5].ToString().Replace(":", ""), Respuesta.Rows[i][6], Respuesta.Rows[i][7], Respuesta.Rows[i][8], Respuesta.Rows[i][9], "Firmado");
                }
                else
                {
                    CadenaDataTable += string.Format("{0}{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11}", Environment.NewLine, Convert.ToDateTime(Respuesta.Rows[i][0]).ToString("dd/MM/yyyy"), Respuesta.Rows[i][1].ToString().Replace(":", ""), Respuesta.Rows[i][2], Respuesta.Rows[i][3], Convert.ToDateTime(Respuesta.Rows[i][4]).ToString("dd/MM/yyyy"), Respuesta.Rows[i][5].ToString().Replace(":", ""), Respuesta.Rows[i][6], Respuesta.Rows[i][7], Respuesta.Rows[i][8], Respuesta.Rows[i][9], "Firmado");
                }
            }

            //foreach (DataRow row in Respuesta.Rows)
            //{
            //    CadenaDataTable += string.Format("{0}{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11}", Environment.NewLine, Convert.ToDateTime(row["FechaLlegada"]).ToString("dd/MM/yyyy"), row["HoraLlegada"], row["NumeroVueloLlegada"], row["Origen"], Convert.ToDateTime(row["FechaSalida"]).ToString("dd/MM/yyyy"), row["HoraSalida"], row["NumeroVueloSalida"], row["Destino"], row["NombrePasajero"], row["Categoria"], "Firmado");
            //    //CadenaDataTable += Convert.ToDateTime(row["FechaLlegada"]).ToString("dd/MM/yyyy") + "," + row["HoraLlegada"] + "," + row["NumeroVueloLlegada"] + "," + row["Origen"] + "," + Convert.ToDateTime(row["FechaSalida"]).ToString("dd/MM/yyyy") + "," + row["HoraSalida"] + "," + row["NumeroVueloSalida"] + "," + row["Destino"] + "," + row["NombrePasajero"] + "," + row["Categoria"] + "," + "Firmado";

            //}

            //StreamWriter file = new StreamWriter(Ruta + "\\Transitos" + Fecha.Split("/")[1] + ".txt", true);
            StreamWriter file = new StreamWriter(Ruta + "\\Transitos" + Fecha + ".txt", true);
            file.WriteLine(CadenaDataTable);
            file.Close();

            return Ok(Respuesta);

        }

        [HttpGet]
        public void ExportDataTabletoFile(DataTable datatable, string delimited, bool exportcolumnsheader, string file, string Fecha)

        {

            StreamWriter str = new StreamWriter(file, false, System.Text.Encoding.Default);

            if (exportcolumnsheader)

            {

                string Columns = string.Empty;

                foreach (DataColumn column in datatable.Columns)

                {

                    Columns += column.ColumnName + delimited;

                }

                str.WriteLine(Columns.Remove(Columns.Length - 1, 1));

            }

            foreach (DataRow datarow in datatable.Rows)

            {

                string row = string.Empty;



                foreach (object items in datarow.ItemArray)

                {



                    row += items.ToString() + delimited;

                }

                str.WriteLine(row.Remove(row.Length - 1, 1));



            }

            str.Flush();

            str.Close();



        }

        /// <summary>
        /// Suspenders the asyncnc.
        /// Por medio de este método se pone un cierre de vuelo en estado suspendido
        /// para que la aerolínea pueda subir un nuevo cierre corregido.
        /// La tarea que remueve las suspensiones se ejecuta en la base de datos y se llama Quitar suspensiones
        /// 
        /// </summary>
        /// <param name="IDOperacion">Identificador de la operación de vuelo en la tabla de operaciones.</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> SuspenderAsync(string IDOperacion)
        {
            if (IDOperacion == "")
            {
                logger.Warning("Entidad para insertar vacía");
                return BadRequest();
            }
            try
            {
                await operacionVueloAplicacion.SuspenderAsync(int.Parse(IDOperacion));
                logger.Information("Vuelo paso a estado suspendido: " + IDOperacion);
                return Ok(true);
            }
            catch (Exception err)
            {
                logger.Error(err, "Error en la suspensión del vuelo: ", IDOperacion, err.Message);
                return BadRequest();
            }
        }

        private async Task SendMail(string _Asunto, string _Para, string _ConCopia, string _Mensaje)
        {
            string urlAPI = Configuration.GetSection("URIs:EmailSender").Value;
            Notificacion notificacion = new Notificacion
            {
                Asunto = _Asunto,
                Destinos = _Para,
                Copias = _ConCopia,
                Cuerpo = _Mensaje
            };
            Opain.Jarvis.Servicios.WebApi.Helpers.Mail oEmail = new Mail(usuarioAplicacion, logger, Configuration);

            bool result = await oEmail.Send(notificacion);
        }
        private async Task NotificacionRetiro(int id)
        {
            OperacionVueloOtd operacionVuelo = new OperacionVueloOtd();
            string Para = "";
            string ConCopia = "";
            try
            {
                operacionVuelo = await operacionVueloAplicacion.ObtenerAsync(id).ConfigureAwait(false);
                int idAreolinea = operacionVuelo.IdAerolinea;
                IList<UsuarioOtd> ListUsuarios = await usuarioAplicacion.ObtenerAsync().ConfigureAwait(false);

                foreach (var datosUsuario in ListUsuarios.Where(x => x.UsuarioAerolinea.Any()))
                {
                    foreach (var item in datosUsuario.UsuarioAerolinea)
                    {
                        if (idAreolinea == item.IdAerolinea)
                        {
                            Para = Para + datosUsuario.Email.Trim() + ";";
                            logger.Information($"Notificación de vuelo suspendido, Email Usuario: {datosUsuario.Email.Trim()} ");
                        }
                    }
                }
                IList<AerolineaOtd> lAerolineas = await aerolineaAplicacion.ObtenerTodosAsync();
                IList<AerolineaOtd> oAerolinea = lAerolineas.Where(a => a.Id == idAreolinea).ToList();
                string NombreAerolinea = oAerolinea[0].Nombre;

                string HostImg = Configuration.GetSection("SendGrid:HostImg").Value;
                string file = HostImg + "/images/firma.png";

                string Asunto = "Notificación de suspensión del vuelo " + operacionVuelo.Vuelo;
                logger.Information($"Notificación de suspensión del vuelo");

                if (Configuration.GetSection("SendGrid:cc").Value != null)
                {
                    ConCopia = Configuration.GetSection("SendGrid:cc").Value;
                }
                string Mensaje = "" +
                    "<p>Buen día estimada aerolínea, " + NombreAerolinea + "</p>" +
                    "<p>Nos permitimos informarle que el vuelo de salida (" + operacionVuelo.Vuelo + ") del " + operacionVuelo.Fecha.ToString("dd-MM-yyyy") + "</p>" +
                    "<p>Se le retira la suspensión y vuelve a quedar disponible para ser facturado, la razón obedece a que no llego el informe de vuelo " +
                    "que lo corregía. El sistema de JARVIS procederá a realizar la liquidación con la información suministrada inicialmente.</p>" +
                    "</br></br><p>Cordialmente,</p></br><b> Equipo de facturación </b></br><b> OPAIN S.A.</b></br> <p><img src='" + file + "' width='250' height='70'/></p><p></p> " +
                    "</br> Bogotá D.C. - Colombia </br>www.eldorado.aero </br>Call Center Aeropuerto(24 horas) Tel. + 57(1) 266 2000 ";

                if (Para.IndexOf(";") > 0)
                {
                    Para = Para.Substring(0, Para.Length - 1);
                }
                await SendMail(Asunto, Para, ConCopia, Mensaje);
            }
            catch (Exception ex)
            {
                logger.Error(ex, ex.Message);
            }
        }

        /// <summary>
        /// Notificacioneses the suspensiones retiradas asyncnc.
        /// Permite tomas aquellos cierres en estado 11 notifica a las aerolineas que los va a pasar
        /// a estado 4 porque nunca llego la pareja con la correción
        /// </summary>
        /// <returns>True si fucniona correctamente.</returns>
        [HttpGet]
        public async Task<IActionResult> NotificacionesSuspensionesRetiradasAsync()
        {
            IList<OperacionVueloOtd> operacionesVuelo = await operacionVueloAplicacion.ObtenerSuspencionesNotificar();
            OperacionVueloOtd operacionVuelo;
            if (operacionesVuelo.Count > 0)
            {
                foreach (var vuelo in operacionesVuelo)
                {
                    try
                    {
                        await NotificacionRetiro(vuelo.Id);
                        logger.Information("Vuelo notificado como retiro de suspensión: " + vuelo.Id, " Numero de vuelo " + vuelo.Vuelo);
                        await operacionVueloAplicacion.ActualizarSuspencionAsync(vuelo.Id);
                    }
                    catch (Exception err)
                    {
                        logger.Error(err, "Error retirando la suspensión del vuelo: ", vuelo.Id.ToString(), err.Message);
                    }
                }
            }
            return Ok(true);
        }

    }
}