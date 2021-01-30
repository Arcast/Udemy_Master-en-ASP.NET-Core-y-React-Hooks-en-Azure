using MediatR;
using Persistencia.DapperConexion.Paginacion;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Aplicacion.Cursos
{
    public class PaginacionCurso
    {
        public class Ejecuta : IRequest<PaginacionModel>
        {
            public string Titulo { get; set; }
            public int NumeroPagina { get; set; }
            public int CantidadElementos { get; set; }

        }

        public class Manejador : IRequestHandler<Ejecuta, PaginacionModel>
        {
            private readonly IPaginacion paginacion;
            public Manejador(IPaginacion _paginacion)
            {
                paginacion = _paginacion;
            }
            public async Task<PaginacionModel> Handle(Ejecuta request, CancellationToken cancellationToken)
            {
                var storeProcedure = "usp_obtenerPaginacion";
                var Ordenamiento = "Titulo";
                var Parametros = new Dictionary<string, object>();

                Parametros.Add("NombreCurso", request.Titulo);

                return await paginacion.devolverPaginacion(storeProcedure, request.NumeroPagina, request.CantidadElementos, Parametros, Ordenamiento );
            }
        }

    }
}
