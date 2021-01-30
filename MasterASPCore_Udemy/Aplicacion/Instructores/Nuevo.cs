using FluentValidation;
using MediatR;
using Persistencia.DapperConexion.Instructor;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Aplicacion.Instructores
{
    public class Nuevo
    {
        public class Ejecuta : IRequest
        {            
            public string Nombres { get; set; }
            public string Apellidos { get; set; }
            public string Grado { get; set; }
           
        }

        public class ValidarEjecuta : AbstractValidator<Ejecuta>
        {
            public ValidarEjecuta()
            {
                RuleFor(x => x.Nombres).NotEmpty();
                RuleFor(x => x.Apellidos).NotEmpty();
                RuleFor(x => x.Grado).NotEmpty();
            }
        }

        public class Manejador : IRequestHandler<Ejecuta>
        {
            private readonly IInstructor instructor;

            public Manejador(IInstructor _instructor) {
                instructor = _instructor;
            }

            public async Task<Unit> Handle(Ejecuta request, CancellationToken cancellationToken)
            {
                var resultado = await instructor.Nuevo(request.Nombres, request.Apellidos, request.Grado);
                if (resultado > 0)
                {
                    return Unit.Value;
                }
                throw new Exception("No se pudo insertar el instructor");
            }
        }

    }
}
