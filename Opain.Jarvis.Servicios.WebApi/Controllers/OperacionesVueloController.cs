using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Opain.Jarvis.Aplicacion.Interfaces;
using Opain.Jarvis.Dominio.Entidades;
using Serilog;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authorization;

namespace Opain.Jarvis.Servicios.WebApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class OperacionesVueloController : Controller
    {
        private readonly IUsuarioAplicacion usuarios;
        private readonly ILogger logger;
        public IConfiguration Configuration { get; }
        private readonly Store.Metodos.OperacionesVuelo StoreProcedure;
        public OperacionesVueloController(IUsuarioAplicacion u, ILogger l, Store.Metodos.OperacionesVuelo store)
        {
            usuarios = u;
            logger = l.ForContext<UsuariosController>();
            this.StoreProcedure = store;
        }
        // GET: TraceController
        [HttpPost]
        [ProducesResponseType(typeof(OperacionVueloOTDRequest), StatusCodes.Status200OK)]
        [AllowAnonymous]
        public async Task<ActionResult<IList<OperacionVueloOtd>>> Find(OperacionVueloOTDRequest otd)
        {
            IList<OperacionVueloOtd> vuelos = await this.StoreProcedure.Find(otd);
            logger.Information("Consultó {@cantidad} registros", vuelos.Count);
            return Ok(vuelos);            
        }
    }
}
