using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SendGrid;
using SendGrid.Helpers.Mail;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using Opain.Jarvis.Dominio.Entidades;
using Opain.Jarvis.Aplicacion.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.UI.Services;

using System.Net.Mail;
using System.Net;

namespace Opain.Jarvis.Servicios.WebApi.Helpers
{
    public class Mail
    {
        private readonly IUsuarioAplicacion usuarios;
        private readonly ILogger logger;
        public IConfiguration Configuration { get; }
        public Mail(IUsuarioAplicacion u, ILogger l, IConfiguration config)
        {
            usuarios = u;
            logger = l;
            Configuration = config;
        }
        public async Task<bool> Send(Notificacion OTDNotificacion)
        {
            if (Configuration.GetSection("SendGrid:EnviarCorreos").Value.Equals("true"))
            {
                string bccmail = Configuration.GetSection("SendGrid:bcc").Value;
                string bccname = Configuration.GetSection("SendGrid:bcc").Value;
                string BuzonSalida = Configuration.GetSection("SendGrid:RemitenteEmail").Value;
                string NombreBuzonSalida = Configuration.GetSection("SendGrid:RemitenteNombre").Value;
                var subject = OTDNotificacion.Asunto;

                var smtpClient = new SmtpClient("smtp.office365.com")
                {
                    Port = 587,
                    Credentials = new NetworkCredential(Configuration.GetSection("SendGrid:RemitenteEmailOffice365").Value,
                    Configuration.GetSection("SendGrid:PasswordeOffice365").Value),
                    EnableSsl = true
                };
                var _emailMessage = new MailMessage();
                _emailMessage.From = new MailAddress(Configuration.GetSection("SendGrid:RemitenteEmailOffice365").Value);
                if (OTDNotificacion.Destinos != "")
                {
                    MailAddress destino;
                    foreach (string para in OTDNotificacion.Destinos.Split(";"))
                    {
                        string[] Name = para.Split("@");
                        destino = new MailAddress(para, Name[0]);
                        _emailMessage.To.Add(destino);
                    }
                }
                if (OTDNotificacion.Copias != "")
                {
                    MailAddress copia;
                    foreach (string para in OTDNotificacion.Copias.Split(";"))
                    {
                        string[] Name = para.Split("@");
                        copia = new MailAddress(para, Name[0]);
                        _emailMessage.CC.Add(para);
                    }
                }
                if (bccmail!="")
                {
                    MailAddress copiaOculta = new MailAddress(bccmail, bccname);
                    _emailMessage.Bcc.Add(copiaOculta);
                }

                _emailMessage.Subject = OTDNotificacion.Asunto;
                _emailMessage.Body = OTDNotificacion.Cuerpo;
                _emailMessage.IsBodyHtml = true;
                // Envío de correo electrónico
                try
                {
                    smtpClient.Send(_emailMessage);
                    return true;
                }
                catch (Exception o)
                {
                    logger.Error(o, "Email : Send ");
                    return false;
                }

                #region Ayuda
                //    string key = Configuration.GetSection("SendGrid:Key").Value;
                //    string bccmail = Configuration.GetSection("SendGrid:bcc").Value;
                //    string bccname = Configuration.GetSection("SendGrid:bcc").Value;
                //    string BuzonSalida = Configuration.GetSection("SendGrid:RemitenteEmail").Value;
                //    string NombreBuzonSalida = Configuration.GetSection("SendGrid:RemitenteNombre").Value;

                //    var client = new SendGridClient(key);
                //    var from = new EmailAddress(BuzonSalida, NombreBuzonSalida);
                //    var subject = OTDNotificacion.Asunto;

                //    List<EmailAddress> lTos = new List<EmailAddress>();
                //    EmailAddress to;
                //    if (OTDNotificacion.Destinos != "")
                //    {
                //        foreach (string para in OTDNotificacion.Destinos.Split(";"))
                //        {
                //            to = new EmailAddress();
                //            string[] Name = para.Split("@");
                //            to.Email = para;
                //            to.Name = Name[0];
                //            lTos.Add(to);
                //        }
                //    }

                //    List<EmailAddress> lCC = new List<EmailAddress>();
                //    EmailAddress cc;
                //    if (OTDNotificacion.Copias != "")
                //    {
                //        foreach (string copia in OTDNotificacion.Copias.Split(";"))
                //        {
                //            cc = new EmailAddress();
                //            string[] Name = copia.Split("@");
                //            cc.Email = copia;
                //            cc.Name = Name[0];
                //            lCC.Add(cc);
                //        }
                //    }

                //    var plainTextContent = OTDNotificacion.Cuerpo;
                //    var htmlContent = OTDNotificacion.Cuerpo;
                //    var msg = new SendGridMessage()
                //    {
                //        From = from,
                //        Subject = subject,
                //        HtmlContent = htmlContent
                //    };
                //    if (OTDNotificacion.Destinos != "")
                //    {
                //        msg.AddTos(lTos);
                //        msg.AddBcc(bccmail, bccname);
                //    }
                //    if (OTDNotificacion.Copias != "")
                //    {
                //        msg.AddCcs(lCC);
                //    }

                //    var response = await client.SendEmailAsync(msg);
                //    try
                //    {
                //        SendGrid.Response result = (SendGrid.Response)response;
                //        if (result.StatusCode != System.Net.HttpStatusCode.Accepted)
                //        {
                //            logger.Information("Email : Send : Mensaje no pudo ser enviado");
                //            return false;
                //            //throw new Exception("Mensaje no pudo ser enviado");
                //        }
                //        return true;
                //    }
                //    catch (Exception o)
                //    {
                //        logger.Error(o, "Email : Send ");
                //        return false;
                //    }
                //}
                //else
                //{
                //    return false;
                //}
                #endregion
            }
            else
            {
                return false;
            }
        }
    }
}
