using Microsoft.AspNetCore.Identity;
using Opain.Jarvis.Dominio.Entidades;
using Opain.Jarvis.Infraestructura.Datos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Opain.Jarvis.Servicios.WebApi.Helpers
{
    public class InicializarDatos
    {
        private readonly RoleManager<Rol> rolManager;
        private readonly UserManager<Usuario> userManager;
        private readonly ContextoOpain db;

        public InicializarDatos(RoleManager<Rol> rm, UserManager<Usuario> um, ContextoOpain contexto)
        {
            rolManager = rm;
            userManager = um;
            db = contexto;
        }

        public async Task Acceso()
        {
            var roles = new string[] 
            {
                "AEROLINEA",
                "SUPERVISOR",
                "CARGA",
                "SUPERVISOR CARGA",
                "TECNOLOGIA",
                "OPAIN",
                "ADMINISTRADOR",
                "EXTERNO"
            };

            foreach(var item in roles)
            {
                await CrearRol(item);
            }

            Usuario usuario = new Usuario
            {
                UserName = "123456789",
                Activo = true,
                Apellido = "OPAIN",
                Cargo = "ADMINISTRADOR SISTEMA",
                Email = "admin@opain.co",
                Nombre = "ADMIN.",
                NumeroDocumento = "123456789",
                PhoneNumber = "3000000000",
                Telefono = "3000000000",
                TipoDocumento = "CC"
            };

            await CrearUsuario(usuario);

            await AsignarRol("123456789", "ADMINISTRADOR");

        }

        public async Task CrearUsuario(Usuario usuario)
        {
            var estado = await userManager.FindByNameAsync(usuario.UserName);

            if (estado == null)
            {
                await userManager.CreateAsync(usuario, "Opain2019*");
            }
        }

        public async Task CrearRol(string nombre)
        {
            var estado = await rolManager.FindByNameAsync(nombre);

            if (estado == null)
            {
                Rol rol = new Rol() { Name = nombre };
                await rolManager.CreateAsync(rol);
            }
        }

        public async Task AsignarRol(string username, string rol)
        {
            var usuario = await userManager.FindByNameAsync(username);
            await userManager.AddToRoleAsync(usuario, rol);
        }
    }
}
