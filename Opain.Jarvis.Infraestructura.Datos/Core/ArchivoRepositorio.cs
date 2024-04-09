using Microsoft.EntityFrameworkCore;
using Opain.Jarvis.Dominio.Entidades;
using Opain.Jarvis.Infraestructura.Datos;

using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace Opain.Jarvis.Infraestructura.Datos.Core
{
    public class ArchivoRepositorio : IArchivoRepositorio
    {
        private readonly ContextoOpain _contexto;


        public ArchivoRepositorio(ContextoOpain contexto)
        {
            _contexto = contexto;
        }

        public async Task InsertarAsync(RutaArchivos archivo)
        {
            await _contexto.AddAsync(archivo);
            await _contexto.SaveChangesAsync();
        }

        public async Task ActualizarAsync(RutaArchivos archivo)
        {

            _contexto.Update(archivo);
            await _contexto.SaveChangesAsync();
        }

        public async Task EliminarAsync(int id)
        {

            var tipoVuelo = await ObtenerAsync(id);
            _contexto.RutaArchivos.Remove(tipoVuelo);
            _contexto.SaveChanges();


        }

        public async Task<RutaArchivos> ObtenerAsync(int id)
        {
            return await _contexto.RutaArchivos.FindAsync(id);
        }

        public async Task InsertarMasivoAsync(IList<RutaArchivos> archivo)
        {
            await _contexto.AddRangeAsync(archivo);
            await _contexto.SaveChangesAsync();
        }

        public async Task<IList<RutaArchivos>> ObtenerPorOperacionAsync(int idOperacion)
        {
            return await _contexto.RutaArchivos.Where(x => x.IdOperacionVuelo == idOperacion).ToListAsync();
        }
    }
}
