using Opain.Jarvis.Dominio.Entidades;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Opain.Jarvis.Infraestructura.Datos.Core
{
    public interface ICargueRepositorio
    {
        Task<RutaArchivos> InsertarAsync(RutaArchivos cargue);

        Task<IList<RutaArchivos>> ObtenerTodosAsync(DateTime inicio, DateTime fin);
    }
}
