using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Aplicacion.Instructores;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Persistencia.DapperConexion.Instructor;

namespace WebApi.Controllers
{
    [Authorize(Roles = "admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class InstructorController : MiControllerBase
    {
        
        [HttpGet]
        public async Task<ActionResult<List<InstructorModel>>> ObtenerInstructores()
        {
            return await mediator.Send(new Consulta.ListaInstructor());           
        }
        [HttpPost]
        public async Task<ActionResult<Unit>> Crear(Nuevo.Ejecuta data)
        {
            return await mediator.Send(data);
        }
        [HttpPut("{id}")]
        public async Task<ActionResult<Unit>> Actualizar(Guid id, Editar.Ejecuta data)
        {
            data.InstructorId = id;
            return await mediator.Send(data);
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult<Unit>> Eliminar(Guid id)
        {            
            return await mediator.Send(new Eliminar.Ejecuta {InstructorId = id});
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<InstructorModel>> ObtenerPorId(Guid id)
        {
            return await mediator.Send(new ConsultaId.Ejecuta { InstructorId = id });
        }

    }
}
