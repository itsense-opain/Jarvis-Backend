using Opain.Jarvis.Dominio.Entidades;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Opain.Jarvis.Infraestructura.Datos.Core
{
    public interface IArchivoRepositorio
    {
        Task InsertarAsync(RutaArchivos archivo);
        Task InsertarMasivoAsync(IList<RutaArchivos> archivo);
        Task ActualizarAsync(RutaArchivos archivo);
        Task EliminarAsync(int id);
        Task<RutaArchivos> ObtenerAsync(int id);
        Task<IList<RutaArchivos>> ObtenerPorOperacionAsync(int idOperacion);
    }
}