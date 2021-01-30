using Aplicacion.ManejadorError;
using MediatR;
using Persistencia.DapperConexion.Instructor;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Aplicacion.Instructores
{
    public class ConsultaId
    {
        public class Ejecuta : IRequest<InstructorModel>
        {
            public Guid InstructorId { get; set; }
        }
        public class Manejador : IRequestHandler<Ejecuta, InstructorModel>
        {
            private readonly IInstructor instructor;
            public Manejador(IInstructor _instructor)
            {
                instructor = _instructor;
            }

            public async Task<InstructorModel> Handle(Ejecuta request, CancellationToken cancellationToken)
            {
                var resultado = await instructor.ObtenerPorId(request.InstructorId);
                if (resultado == null)
                {
                    throw new ManejadorException(HttpStatusCode.NotFound, new { Mensaje = "No se pudo obtener el instructor" });
                }
                return resultado;
            }
        }
       
    }
}
