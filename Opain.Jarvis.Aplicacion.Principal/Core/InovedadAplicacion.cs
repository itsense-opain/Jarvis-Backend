using Opain.Jarvis.Dominio.Entidades;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Opain.Jarvis.Aplicacion.Interfaces
{
    public interface INovedadAplicacion
    {
        Task InsertarAsync(NovedadOtd novedadOtd);
        Task ActualizarAsync(NovedadOtd novedadOtd);
        Task EliminarAsync(int id);
        Task<NovedadOtd> ObtenerAsync(int id);
        Task<IList<NovedadOtd>> ObtenerTodosPorOperacionAsync(int id);
        Task<IList<NovedadOtd>> ObtenerTodosAsync();
    }
}
