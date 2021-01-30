using Aplicacion.ManejadorError;
using MediatR;
using Persistencia;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Aplicacion.Comentarios
{
    public class Eliminar
    {
        public class Ejecuta : IRequest
        {
            public Guid id { get; set; }
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
                var comentario = await context.Comentario.FindAsync(request.id);
                if (comentario == null)
                {
                    throw new ManejadorException(HttpStatusCode.NotFound, new {Mensaje = "No se encontro el comentario" });
                }
                context.Remove(comentario);
                var resp = await context.SaveChangesAsync();
                if (resp > 0)
                {
                    return Unit.Value;
                }

                throw new Exception("No se logro eliminar el comentario.");
            }
        }
    }
}
