using Aplicacion.ManejadorError;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Aplicacion.Seguridad
{
    public class RolNuevo
    {
        public class Ejecuta : IRequest
        {
            public String Nombre { get; set; }
        }

        public class ValidaEjecuta : AbstractValidator<Ejecuta>
        {
            public ValidaEjecuta()
            {
                RuleFor(x => x.Nombre).NotEmpty();
            }
        }

        public class Manejador : IRequestHandler<Ejecuta>
        {
            private readonly RoleManager<IdentityRole> rolManager;
            public Manejador(RoleManager<IdentityRole> _rolManager)
            {
                rolManager = _rolManager;
            }
            public async Task<Unit> Handle(Ejecuta request, CancellationToken cancellationToken)
            {
                var rol = await rolManager.FindByNameAsync(request.Nombre);
                if (rol != null)
                {
                    throw new ManejadorException(HttpStatusCode.BadRequest, new { Mensaje = "Ya existe el Rol" });
                }
                var resultado = await rolManager.CreateAsync(new IdentityRole(request.Nombre));
                if (resultado.Succeeded)
                {
                    return Unit.Value;
                }
                throw new Exception("No se pudo crear el Rol");
            }
        }

    }
}
