using MediatR;
using Persistencia.DapperConexion.Instructor;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Aplicacion.Instructores
{
    public class Eliminar
    {
        public class Ejecuta : IRequest
        {
            public Guid InstructorId { get; set; }
        }
        public class Manejador : IRequestHandler<Ejecuta>
        {
            private readonly IInstructor instructor;
            public Manejador(IInstructor _instructor)
            {
                instructor = _instructor;
            }

            public async Task<Unit> Handle(Ejecuta request, CancellationToken cancellationToken)
            {
                var resultado = await instructor.Eliminar(request.InstructorId);
                if (resultado > 0)
                {
                    return Unit.Value;
                }
                throw new Exception("No se pudo eliminar el instructor");
            }
                       
        }
    }
}
