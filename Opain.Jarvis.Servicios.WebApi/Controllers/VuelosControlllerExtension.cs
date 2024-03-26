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

    public partial class VuelosController : ControllerBase
    {
        /// <summary>
        /// Obtener datos : Permite realizar la extracción de datos de la estructura de Operaciones de vuelos
        /// enviado varios tipos de filtro personalizados y ejecutados por base de datos.
        /// </summary>
        /// <param name="oFiltro">Objeto de trasnferencia de datos con parametros de busqueda</param>
        /// <returns>Colección de datos</returns>
        [HttpPost]
        [ProducesResponseType(typeof(OperacionVueloOTDRequest), StatusCodes.Status200OK)]
        public async Task<IActionResult> ObtenerDatos(OperacionVueloOTDRequest oFiltro)
        {
            IList<OperacionVueloOtd> vuelos = await this.StoreProcedure.Find(oFiltro);
            logger.Information("Consultó Vuelos{@cantidad} registros", vuelos.Count);
            return Ok(vuelos);
        }
    }
}
