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
    public class TraceController : Controller
    {
        private readonly IUsuarioAplicacion usuarios;
        private readonly ILogger logger;
        public IConfiguration Configuration { get; }
        private readonly Store.Trace StoreProcedure;
        public TraceController(IUsuarioAplicacion u, ILogger l, Store.Trace store)
        {
            usuarios = u;
            logger = l.ForContext<UsuariosController>();
            this.StoreProcedure = store;
        }
        // GET: TraceController
        [HttpPost]
        [ProducesResponseType(typeof(TraceOtd), StatusCodes.Status200OK)]
        [AllowAnonymous]
        public async Task<ActionResult> SaveAsync(TraceOtd OTDTrace)
        {
            this.StoreProcedure.Save(OTDTrace);
            return Ok();
        }

       
    }
}
