using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Aplicacion.Seguridad;
using Dominio;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    //[AllowAnonymous] es para liberar este controlador para que puedan consultar sin token
    [AllowAnonymous]
    [Route("api/[controller]")]
    [ApiController]   
    public class UsuarioController : MiControllerBase
    {
        // http://localhost:5000/api/Usuario/login
        [HttpPost("login")]
        public async Task<ActionResult<UsuarioData>> Login(Login.Ejecuta parametros)
        {
            return await mediator.Send(parametros);
        }

        // http://localhost:5000/api/Usuario/Registrar
        [HttpPost("Registrar")]
        public async Task<ActionResult<UsuarioData>> Registrar(Registrar.Ejecuta parametros)
        {
            return await mediator.Send(parametros);
        }

        // http://localhost:5000/api/Usuario
        [HttpGet]
        public async Task<ActionResult<UsuarioData>> DevolverUsuario()
        {
            return await mediator.Send(new UsuarioActual.Ejecutar());
        }

    }
}
 