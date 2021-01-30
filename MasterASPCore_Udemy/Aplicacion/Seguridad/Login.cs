using Aplicacion.Contratos;
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
    public class Login
    {
        public class Ejecuta : IRequest<UsuarioData>
        {
            public string Email { get; set; }
            public string Password { get; set; }
        }
        public class EjecutaValidacion : AbstractValidator<Ejecuta>
        {
            public EjecutaValidacion()
            {
                RuleFor(x => x.Email).NotEmpty();
                RuleFor(x => x.Password).NotEmpty();
            }
        }
        public class Manejador : IRequestHandler<Ejecuta, UsuarioData>
        {
            private readonly UserManager<Usuario> _userManager;
            private readonly SignInManager<Usuario> _signInManager;
            private readonly IJwtGenerador _IJwtGenerador;
            public Manejador(UserManager<Usuario> userManager, SignInManager<Usuario> signInManager, IJwtGenerador jwtGenerador)
            {
                _userManager = userManager;
                _signInManager = signInManager;
                _IJwtGenerador = jwtGenerador;
            }
            public async Task<UsuarioData> Handle(Ejecuta request, CancellationToken cancellationToken)
            {
                var _Usuario = await _userManager.FindByEmailAsync(request.Email);
                if (_Usuario == null)
                {
                    throw new ManejadorException(HttpStatusCode.Unauthorized);
                }
                var resultado = await _signInManager.CheckPasswordSignInAsync(_Usuario, request.Password, false);
                var resultadoRoles = await _userManager.GetRolesAsync(_Usuario);
                var listaRoles = new List<string>(resultadoRoles);
                if (resultado.Succeeded)
                {
                    return new UsuarioData { 
                        NombreCompleto = _Usuario.NombreCompleto,
                        Token = _IJwtGenerador.CrearToken(_Usuario, listaRoles),
                        UserName = _Usuario.UserName,
                        Email = _Usuario.Email,
                        Imagen = null
                    };
                }
                throw new ManejadorException(HttpStatusCode.Unauthorized);
            }
        }

    }
}
