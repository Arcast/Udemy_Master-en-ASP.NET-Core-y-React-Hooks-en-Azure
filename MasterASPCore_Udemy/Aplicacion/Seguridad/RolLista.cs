using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Persistencia;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Aplicacion.Seguridad
{
    public class RolLista
    {
        public class Ejecuta : IRequest<List<IdentityRole>>
        {
        }

        public class Manejador : IRequestHandler<Ejecuta, List<IdentityRole>>
        {
            private readonly CursosOnlineContext context;

            public Manejador(CursosOnlineContext _context)
            {
                context = _context;
            }

            public async Task<List<IdentityRole>> Handle(Ejecuta request, CancellationToken cancellationToken)
            {
                var roles = await context.Roles.ToListAsync();

                return roles;
            }
        }
    }
}
