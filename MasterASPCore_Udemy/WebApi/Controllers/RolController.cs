using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Aplicacion.Seguridad;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolController : MiControllerBase
    {
        [HttpPost("Crear")]
        public async Task<ActionResult<Unit>> crear(RolNuevo.Ejecuta parametros)
        {
            return await mediator.Send(parametros);
        }

        [HttpDelete("Eliminar")]
        public async Task<ActionResult<Unit>> Eliminar(RolElimina.Ejecuta parametros)
        {
            return await mediator.Send(parametros);
        }

        [HttpGet("Lista")]
        public async Task<ActionResult<List<IdentityRole>>> Lista()
        {
            return await mediator.Send(new RolLista.Ejecuta());
        }

        [HttpPost("AgregarRolUsuario")]
        public async Task<ActionResult<Unit>> AgregarRolUsuario(UsuarioRolAgregar.Ejecuta parametros)
        {
            return await mediator.Send(parametros);
        }
        [HttpDelete("EliminarRolUsuario")]
        public async Task<ActionResult<Unit>> EliminarRolUsuario(UsuarioRolEliminar.Ejecuta parametros)
        {
            return await mediator.Send(parametros);
        }
        [HttpGet("{UserName}")]
        public async Task<ActionResult<List<string>>> ObtenerRolesUsuario(string username)
        {
            return await mediator.Send(new ObtenerRolesPorUsuario.Ejecuta {userName = username });
        }
    }
}
