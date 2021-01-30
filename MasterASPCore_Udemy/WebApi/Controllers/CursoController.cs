using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Aplicacion.Cursos;
using Dominio;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Persistencia.DapperConexion.Paginacion;
using static Aplicacion.Cursos.Nuevo;

namespace WebApi.Controllers
{
    // http://localhost:5000/api/Curso
    [Route("api/[controller]")]
    [ApiController] 
    public class CursoController : MiControllerBase
    {
        //Ya no se utiliza esto, en vez de heredar de ControllerBase
        //Ahora se hereda desde MiControllerBase
        //----
        //private readonly IMediator mediator;
        //public CursoController(IMediator _mediator)
        //{
        //    mediator = _mediator;
        //}

        [HttpGet]        
        public async Task<ActionResult<List<CursoDto>>> Get()
        {
            return await mediator.Send(new Consulta.ListaCursos());
        }

        // http://localhost:5000/api/Curso/id
        [HttpGet("{Id}")]
        public async Task<ActionResult<CursoDto>> Detalle(Guid id)
        {
            return await mediator.Send(new ConsultaId.CursoUnico { _Id = id });
        }

        [HttpPost]
        public async Task<ActionResult<Unit>> Crear(Nuevo.Ejecuta data)
        {
            return await mediator.Send(data);
        }

        [HttpPut("{Id}")]
        public async Task<ActionResult<Unit>> Editar(Guid id, Editar.Ejecuta data)
        {
            data.CursoId = id;
            return await mediator.Send(data);
        }

        [HttpDelete("{Id}")]
        public async Task<ActionResult<Unit>> Eliminar(Guid Id)
        {
            return await mediator.Send(new Eliminar.Ejecuta { CursoId = Id });
        }

        [HttpPost("report")]
        public async Task<ActionResult<PaginacionModel>> Report(PaginacionCurso.Ejecuta data)
        {
            return await mediator.Send(data);
        }
    }
}
