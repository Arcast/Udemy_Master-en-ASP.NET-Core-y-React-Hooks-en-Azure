using Dominio;
using FluentValidation;
using MediatR;
using Persistencia;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Aplicacion.Cursos
{
   public class Nuevo
    {
        public class Ejecuta : IRequest
        {
            public String Titulo { get; set; }
            public String Descripcion { get; set; }
            public DateTime? FechaPublicacion { get; set; }
            public List<Guid> ListaInstructor { get; set; }
            public decimal precio { get; set; }
            public decimal promocion { get; set; }
        }

        public class EjecutaValidacion : AbstractValidator<Ejecuta>
        {
            public EjecutaValidacion()
            {
                RuleFor(x => x.Titulo).NotEmpty();
                RuleFor(x => x.Descripcion).NotEmpty();
                RuleFor(x => x.FechaPublicacion).NotEmpty();
            }
        }

        public class Manejador : IRequestHandler<Ejecuta>
        {
            private readonly CursosOnlineContext context;

            public Manejador(CursosOnlineContext _context)
            {
                context = _context;
            }

            public async Task<Unit> Handle(Ejecuta request, CancellationToken cancellationToken)
            {
                Guid _CursoId = Guid.NewGuid();
                var curso = new Curso
                {
                    CursoId = _CursoId,
                    Titulo = request.Titulo,
                    Descripcion = request.Descripcion,
                    FechaPublicacion = request.FechaPublicacion,
                    FechaCreacion = DateTime.UtcNow

                };
                context.Curso.Add(curso);

                if (request.ListaInstructor == null)
                {                    
                    foreach (var id in request.ListaInstructor)
                    {
                       var _cursoInstructor = new Curso_Instructor
                        {
                            CursoId = _CursoId,
                            InstructorId = id
                        };
                        context.Curso_Instructor.Add(_cursoInstructor);
                    } 
                }

                var PrecioEntidad = new Precio
                {
                    CursoId = _CursoId,
                    PrecioActual = request.precio,
                    Promocion = request.promocion,
                    PrecioId = Guid.NewGuid()
                };
                context.Precio.Add(PrecioEntidad);

                var valor = await context.SaveChangesAsync();
                if (valor > 0)
                {
                    return Unit.Value;
                }
                throw new Exception("Error al ingresar el curso");
            }
        } 
    }
}
