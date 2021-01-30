using Aplicacion.ManejadorError;
using Dominio;
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
    public class Editar
    {
        public class Ejecuta : IRequest
        {
            public Guid CursoId { get; set; }
            public String Titulo { get; set; }
            public String Descripcion { get; set; }
            public DateTime? FechaPublicacion { get; set; }
            public List<Guid> ListaInstructor { get; set; }
            public decimal? Precio { get; set; }
            public decimal? Promocion { get; set; }
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
                var curso = await context.Curso.FindAsync(request.CursoId);
                if (curso == null)
                {
                    {
                        //throw new Exception("Curso no existe..!");
                        throw new ManejadorException(HttpStatusCode.NotFound, new { Mensaje = "No se encontro el curso" });
                    }
                }
                curso.Titulo = request.Titulo ?? curso.Titulo;
                curso.Descripcion = request.Descripcion ?? curso.Descripcion;
                curso.FechaPublicacion = request.FechaPublicacion ?? curso.FechaPublicacion;
                curso.FechaPublicacion = DateTime.UtcNow;

                var precioEntidad = context.Precio.Where(x => x.CursoId == curso.CursoId).FirstOrDefault();
                if (precioEntidad != null)
                {
                    precioEntidad.Promocion = request.Promocion ?? precioEntidad.Promocion ;
                    precioEntidad.PrecioActual = request.Precio ?? precioEntidad.PrecioActual;
                }
                else
                {
                    precioEntidad = new Precio
                    {
                        PrecioId = Guid.NewGuid(),
                        PrecioActual = request.Precio ?? 0,
                        Promocion = request.Promocion ?? 0,
                        CursoId = curso.CursoId
                    };
                    await context.Precio.AddAsync(precioEntidad);
                }

                if (request.ListaInstructor != null)
                {
                    if (request.ListaInstructor.Count > 0)
                    {
                        /*Eliminar los instructores actuales de la bd*/
                        var instructoresbd = context.Curso_Instructor
                            .Where(x => x.CursoId == request.CursoId).ToList();
                        foreach (var InstructorEliminar in instructoresbd)
                        {
                            context.Curso_Instructor.Remove(InstructorEliminar);
                        }
                        /*Fin Eliminados*/

                        /*Agregar instructores que provienen del cliente*/
                        foreach (var ids in request.ListaInstructor)
                        {
                            var NuevoInstructor = new Curso_Instructor
                            {
                                CursoId = request.CursoId,
                                InstructorId = ids
                            };
                            await context.Curso_Instructor.AddAsync(NuevoInstructor);
                        }
                    }

                }

                var resultado = await context.SaveChangesAsync();
                if (resultado > 0)
                {
                    return Unit.Value;
                }
                throw new Exception("Error al editar el curso");

            }
        }
    }
}
