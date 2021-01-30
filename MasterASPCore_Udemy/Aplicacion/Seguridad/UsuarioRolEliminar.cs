using Aplicacion.ManejadorError;
using Dominio;
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
    public class UsuarioRolEliminar
    {
        public class Ejecuta : IRequest
        {
            public string userName { get; set; }
            public string rolName { get; set; }
        }
        public class ValidarEjecuta : AbstractValidator<Ejecuta>
        {
            public ValidarEjecuta()
            {
                RuleFor(x => x.userName).NotEmpty();
                RuleFor(x => x.rolName).NotEmpty();
            }
        }
        public class Manejador : IRequestHandler<Ejecuta>
        {
            private readonly UserManager<Usuario> userManager;
            private readonly RoleManager<IdentityRole> roleManager;

            public Manejador(UserManager<Usuario> _userManager, RoleManager<IdentityRole> _roleManager)
            {
                userManager = _userManager;
                roleManager = _roleManager;
            }
                      

            public async Task<Unit> Handle(Ejecuta request, CancellationToken cancellationToken)
            {
                var role = await roleManager.FindByNameAsync(request.rolName);
                if (role == null)
                {
                    throw new ManejadorException(HttpStatusCode.NotFound, new { Mensaje = "No se encontro el Rol" });
                }
                var usuarioIden = await userManager.FindByNameAsync(request.userName);
                if (usuarioIden == null)
                {
                    throw new ManejadorException(HttpStatusCode.NotFound, new { Mensaje = "No se encontro el Usuario" });
                }
                var result = await userManager.RemoveFromRoleAsync(usuarioIden, request.rolName);
                if (result.Succeeded)
                {
                    return Unit.Value;
                }
                throw new Exception("No se logro eliminar el rol al usuario");
            }
        }
    }
}
