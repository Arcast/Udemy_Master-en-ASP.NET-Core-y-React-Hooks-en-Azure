using Aplicacion.ManejadorError;
using Dominio;
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
    public class ObtenerRolesPorUsuario
    {
        public class Ejecuta : IRequest<List<String>>
        {
            public string userName { get; set; }
        }
        public class Manejador : IRequestHandler<Ejecuta, List<string>>
        {
            private readonly UserManager<Usuario> userManager;
            private readonly RoleManager<IdentityRole> roleManager;

            public Manejador(UserManager<Usuario> _userManager, RoleManager<IdentityRole> _roleManager)
            {
                userManager = _userManager;
                roleManager = _roleManager;
            }

            public async Task<List<string>> Handle(Ejecuta request, CancellationToken cancellationToken)
            {
                var usuarioIden = await userManager.FindByNameAsync(request.userName);
                if (usuarioIden == null)
                {
                    throw new ManejadorException(HttpStatusCode.NotFound, new { Mensaje = "No existe el Usuario" });
                }

                var resultado = await userManager.GetRolesAsync(usuarioIden);
                return new List<string>(resultado);

            }
        }
    }
}
