﻿using AutoMapper;
using Opain.Jarvis.Aplicacion.Interfaces;
using Opain.Jarvis.Dominio.Entidades;
using Opain.Jarvis.Dominio.Entidades;
using Opain.Jarvis.Infraestructura.Datos.Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Opain.Jarvis.Aplicacion.Principal
{
    public class CargueAplicacion : ICargueAplicacion
    {
        private readonly ICargueRepositorio cargueRepositorio;
        private readonly IMapper mapper;

        public CargueAplicacion(ICargueRepositorio cargue, IMapper m)
        {
            mapper = m;
            cargueRepositorio = cargue;
        }

        public async Task<CargueOtd> InsertarAsync(CargueOtd cargueOtd)
        {
            var cargue = mapper.Map<CargueOtd, RutaArchivos>(cargueOtd);
            await cargueRepositorio.InsertarAsync(cargue);
            return  mapper.Map<RutaArchivos, CargueOtd>(cargue);
        }

        public async Task<IList<CargueOtd>> ObtenerTodosAsync(DateTime inicio, DateTime fin)
        {
            IList<CargueOtd> carguesOtd = new List<CargueOtd>();

            var cargues = await cargueRepositorio.ObtenerTodosAsync(inicio, fin);

            foreach (var item in cargues)
            {
                var cargue = mapper.Map<RutaArchivos, CargueOtd>(item);

                carguesOtd.Add(cargue);
            }
            return carguesOtd;
        }
    }
}
