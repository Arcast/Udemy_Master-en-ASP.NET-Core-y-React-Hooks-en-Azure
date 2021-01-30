using Aplicacion.ManejadorError;
using MediatR;
using Persistencia;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Aplicacion.Cursos
{
    public class Eliminar
    {
        public class Ejecuta : IRequest
        {
            public Guid CursoId { get; set; }
        }
        public class Manejador : IRequestHandler<Ejecuta>
        {
            public readonly CursosOnlineContext context;

            public Manejador(CursosOnlineContext _context)
            {
                context = _context;
            }
            public async Task<Unit> Handle(Ejecuta request, CancellationToken cancellationToken)
            {
                var instructoresBd = context.Curso_Instructor.Where(x => x.CursoId == request.CursoId).ToList();
                foreach (var Instructor in instructoresBd)
                {
                    context.Curso_Instructor.Remove(Instructor);
                }

                var comentariosBd = context.Comentario.Where(x => x.CursoId == request.CursoId);
                foreach (var comentario in comentariosBd)
                {
                    context.Comentario.Remove(comentario);
                }

                var precioDb = context.Precio.Where(x => x.CursoId == request.CursoId).FirstOrDefault();
                if (precioDb != null)
                {
                    context.Precio.Remove(precioDb);
                }

                var curso = await context.Curso.FindAsync(request.CursoId);
                if (curso == null)
                {
                    //throw new Exception("Curso no existe..!");
                    throw new ManejadorException(HttpStatusCode.NotFound, new {Mensaje = "No se encontro el curso"});
                }
                context.Remove(curso);
                var valor = await context.SaveChangesAsync();
                if (valor > 0)
                {
                    return Unit.Value;
                }
                throw new Exception("Error al eliminar el curso");

            }
        }
    }
}
