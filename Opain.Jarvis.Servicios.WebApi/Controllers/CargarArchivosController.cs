using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Opain.Jarvis.Dominio.Entidades;
using Serilog;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
namespace Opain.Jarvis.Servicios.WebApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class CargarArchivosController : ControllerBase
    {
        private readonly IHostingEnvironment hostingEnvironment;
        private readonly ILogger logger;
        
        

        public CargarArchivosController(IHostingEnvironment env, ILogger l, Microsoft.Extensions.Configuration.IConfiguration configuration)
        {
            hostingEnvironment = env ?? throw new ArgumentNullException(nameof(env));
            logger = l;
            Configuration = configuration;
        }
        public Microsoft.Extensions.Configuration.IConfiguration Configuration { get; }

        [HttpPost]
        [AllowAnonymous]

        public async Task<IActionResult> CargarArchivo(IFormFile archivo, string carpeta)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(hostingEnvironment.WebRootPath))
                {
                    hostingEnvironment.WebRootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
                }

                var appSettingsSection = Configuration.GetSection("Config");

                var appSettings = appSettingsSection.Get<Servicios.WebApi.Helpers.AppSettings>();

                var RutaArchivos = appSettings.RutaArchivos;

                //var ruta = Path.Combine(hostingEnvironment.WebRootPath, carpeta);
                var ruta = Path.Combine(RutaArchivos, carpeta);

                if (!Directory.Exists(ruta))
                {
                    Directory.CreateDirectory(ruta);
                }

                if (archivo.Length > 0)
                {
                    using (var fileStream = new FileStream(Path.Combine(ruta, archivo.FileName), FileMode.Create))
                    {
                        await archivo.CopyToAsync(fileStream).ConfigureAwait(false);
                    }
                }
                logger.Information("Cargó archivo {@nombre} en {@carpeta}", archivo.Name, carpeta);

                // debe enviar correo exitoso

                return Ok();

            }
            catch (Exception err)
            {
                logger.Error(err, "Error cargando archivo {@nombre} en {@carpeta}", archivo.Name, carpeta);

                //debe enviar correo rechazado
                
                return BadRequest();

                
            }
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> CrearArchivo(string carpeta, string texto, string tipo)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(hostingEnvironment.WebRootPath))
                {
                    hostingEnvironment.WebRootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
                }

                var appSettingsSection = Configuration.GetSection("Config");

                var appSettings = appSettingsSection.Get<Servicios.WebApi.Helpers.AppSettings>();

                var RutaArchivos = appSettings.RutaArchivos;
                                
                var ruta = Path.Combine(RutaArchivos, carpeta);

                if (!Directory.Exists(ruta))
                {
                    Directory.CreateDirectory(ruta);
                }

                // creo el .txt si no existe
                if (tipo == "PAX")
                {
                    
                        using (StreamWriter file = new StreamWriter(ruta + "\\PasajerosManual.txt", true))
                        {
                            file.WriteLine(texto); //se agrega información al documento
                            file.Close();
                        }
                   
                }

                // creo el .txt si no existe
                if (tipo == "TT")
                {

                    using (StreamWriter file = new StreamWriter(ruta + "\\TransitosManual.txt", true))
                    {
                        file.WriteLine(texto); //se agrega información al documento
                        file.Close();
                    }

                }

                // creo el .txt si no existe
                if (tipo == "FLY")
                {

                    using (StreamWriter file = new StreamWriter(ruta + "\\VuelosManual.txt", true))
                    {
                        file.WriteLine(texto); //se agrega información al documento
                        file.Close();
                    }

                }

                return Ok();

            }
            catch (Exception err)
            {
                logger.Error(err, "Error cargando archivo {@nombre} en {@carpeta}", "ArchivoManualPasajeros", carpeta);
                return BadRequest();
            }
        }

        //[HttpPost]
        //public async Task<IActionResult> EnviarCorreoSMTP(string destino, string asunto, string mensaje)
        //{
        //    try
        //    {

        //        //prueba Envio correo smtp

        //        String FROM = "jarvis@opain.com.co";
        //        String FROMNAME = "Sender Name";
        //        String TO = destino;
        //        String SMTP_USERNAME = "jarvis@opain.com.co";
        //        String SMTP_PASSWORD = "=hioE_b8ke2Y";
        //        String HOST = "slmp-550-66.slc.westdc.net";
        //        int PORT = 465;
        //        String SUBJECT = asunto;
        //        String BODY = mensaje;
        //        MailMessage message = new MailMessage();
        //        message.IsBodyHtml = true;
        //        message.From = new MailAddress(FROM, FROMNAME);
        //        message.To.Add(new MailAddress(TO));
        //        message.Subject = SUBJECT;
        //        message.Body = BODY;
        //        using (var client = new System.Net.Mail.SmtpClient(HOST, PORT))
        //        {
        //            client.Credentials =
        //                new NetworkCredential(SMTP_USERNAME, SMTP_PASSWORD);

        //            client.EnableSsl = true;
        //            try
        //            {
        //                Console.WriteLine("Attempting to send email...");
        //                client.Send(message);
        //                Console.WriteLine("Email sent!");
        //            }
        //            catch (Exception err)
        //            {
        //                logger.Error(err + err.InnerException.ToString(), "Error enviando correo");
        //                Console.WriteLine("The email was not sent.");
        //                Console.WriteLine("Error message: " + err.Message);
        //            }
        //        }
        //        return Ok();

        //    }
        //    catch (Exception err)
        //    {
        //        logger.Error(err + err.InnerException.ToString(), "Error enviando correo");
        //        return BadRequest();
        //    }
        //}

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> CargarArchivoBytes(ArchivoOtd archivo)
        {            

            string carpeta = archivo.carpeta;
            

            var base64EncodedBytes = System.Convert.FromBase64String(archivo.base64);
            byte[] archivo2 = base64EncodedBytes;
            
            string fecha = string.Format("{0}{1}{2}", DateTime.Now.Year, DateTime.Now.Month.ToString("00"), DateTime.Now.Day.ToString("00"));
            //var nombreArchivo = string.Concat("SoporteAerolinea", DateTime.Now.Year.ToString(), DateTime.Now.Month.ToString().PadLeft(2, '0'), DateTime.Now.Day.ToString(), "_", DateTime.Now.Hour.ToString(), DateTime.Now.Minute.ToString(), DateTime.Now.Second.ToString(), DateTime.Now.Millisecond.ToString(),".pdf");

            string nombreArchivo = archivo.Nombre + ".pdf"; // IdOperacionesVuelo
            if (archivo.Nombre.IndexOf(".")>0)
            {
                nombreArchivo = archivo.Nombre;
            }
            string RutaArchivos = Configuration.GetSection("Config:RutaArchivos").Value + "/" + carpeta + "/" + fecha;

            try
            {
                if (!Directory.Exists(RutaArchivos))
                {
                    Directory.CreateDirectory(RutaArchivos);
                }

                if (archivo2.Length > 0)
                {
                    if (System.IO.File.Exists(RutaArchivos+"/" + nombreArchivo))
                    {
                        //System.IO.File.Delete(RutaArchivos + nombreArchivo);
                        System.IO.File.Delete(Path.Combine(RutaArchivos, nombreArchivo));

                        //System.IO.File.Copy(RutaArchivos + nombreArchivo, RutaArchivos + nombreArchivo + DateTime.Now.ToString("yyyy-MM-dd"));
                        logger.Information("Elimino archivo: {@nombre} en {@carpeta}", nombreArchivo, carpeta);

                    }
                    FileStream fs = new FileStream(RutaArchivos +"/"+ nombreArchivo, FileMode.OpenOrCreate);
                    fs.Write(archivo2, 0, archivo2.Length);                    
                    fs.Close();
                }

                logger.Information("Cargó archivo {@nombre} en {@carpeta}", nombreArchivo, carpeta);
                return Ok();

            }
            catch (Exception err)
            {
                logger.Error(err, "Error cargando archivo {@nombre} en {@carpeta}", nombreArchivo, carpeta);
                return BadRequest();
            }
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerArchivoAsync(string carpeta, string nombreArchivo)
        {
            if (nombreArchivo == null)
            {
                logger.Warning("Nombre de archivo no enviado", nombreArchivo, carpeta);
                return Content("archivo no existe");
            }
            try
            {
                //var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", carpeta, nombreArchivo);
                string path = Path.Combine(Configuration.GetSection("Config:RutaArchivos").Value, carpeta, nombreArchivo);
                if (carpeta == "NA")
                {
                    path = Path.Combine(Configuration.GetSection("Config:RutaArchivos").Value, nombreArchivo);
                }                 
                //FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read);
                //return File(fs, "application/pdf");
                var memory = new MemoryStream();
                using (var stream = new FileStream(path, FileMode.Open))
                {
                    await stream.CopyToAsync(memory).ConfigureAwait(false);
                }
                memory.Position = 0;
                logger.Information("Consultó archivo {@archivo} en {@carpeta}", nombreArchivo, carpeta);
                return File(memory, GetContentType(path), Path.GetFileName(path));
            }
            catch (Exception err)
            {
                logger.Error(err, "Error consultando archivo {@nombre} en {@carpeta}", nombreArchivo, carpeta);
                return BadRequest();

            }
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ObtenerArchivoPDFAsync(string idAerolinea, string fecha,string idOperacionVuelo)
        {
            try
            {
                //var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", carpeta, nombreArchivo);
                //fecha = "20200702";
                logger.Error("parametros de entrada {@nombre} en {@carpeta} Verificando {@idOperacionVuelo}  ", idAerolinea, fecha, idOperacionVuelo);

                string RutaArchivos = Configuration.GetSection("Config:RutaArchivos").Value + "/" + idAerolinea;
                var ruta = Path.Combine(RutaArchivos, fecha);
                string nombreArchivo = idOperacionVuelo + ".pdf";
                string fullPath = ruta + "/" + nombreArchivo;

                var memory = new MemoryStream();
                using (var stream = new FileStream(fullPath, FileMode.Open))
                {
                    await stream.CopyToAsync(memory).ConfigureAwait(false);
                }

                memory.Position = 0;
                
                return File(memory, GetContentType(fullPath), Path.GetFileName(fullPath));
            }
            catch (Exception err)
            {
                logger.Error(err, "Error consultando archivo {@nombre} en {@carpeta}", idAerolinea, fecha);
                logger.Error("Parametro de entrada {@nombre} en {@carpeta} Verificando {@idOperacionVuelo}  ", idAerolinea, fecha, idOperacionVuelo);
                return BadRequest();

            }
        }


        private string GetContentType(string path)
        {
            Dictionary<string, string> types;
            try
            {
                types = GetMimeTypes();
                var ext = Path.GetExtension(path).ToLowerInvariant();
                return types[ext];
            }
            catch (Exception ex)
            {
                return  "application/octet-stream";
            }
  
        }

        //[HttpGet]
        //[ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        //public async Task<ActionResult<string>> confDataBase()
        //{
        //    string dbCxString = Configuration.GetSection("ConnectionStrings:ConexionJarvisBD").Value;
        //    // debo encriptarla antes de enviarla
        //    string cadena =Seguridad.Encriptar(dbCxString);

        //    return Ok(cadena);
        //}

        private Dictionary<string, string> GetMimeTypes()
        {
            return new Dictionary<string, string>
            {
                {".txt", "text/plain"},
                {".pdf", "application/pdf"},
                {".zip", "application/zip"},
                {".rar", "application/rar"},
                {".7z", "application/7z"},
                {".doc", "application/vnd.ms-word"},
                {".docx", "application/vnd.ms-word"},
                {".xls", "application/vnd.ms-excel"},
                {".xlsx", "application/vnd.openxmlformatsofficedocument.spreadsheetml.sheet"},
                {".png", "image/png"},
                {".jpg", "image/jpeg"},
                {".jpeg", "image/jpeg"},
                {".gif", "image/gif"},
                {".csv", "text/csv"}
            };
        }
    }
}