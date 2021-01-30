using Aplicacion.ManejadorError;
using AutoMapper;
using Dominio;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistencia;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Aplicacion.Cursos
{
    public class ConsultaId
    {
        public class CursoUnico : IRequest<CursoDto>
        {
            public Guid _Id { get; set; }
        }
        public class Manejador : IRequestHandler<CursoUnico, CursoDto>
        {
            private readonly CursosOnlineContext context;
            private readonly IMapper mapper;
            public Manejador(CursosOnlineContext _context, IMapper _mapper)
            {
                context = _context;
                mapper = _mapper;
            }
            public async Task<CursoDto> Handle(CursoUnico request, CancellationToken cancellationToken)
            {
                var _Curso = await context.Curso
                     .Include(x => x.ComentarioLista)
                    .Include(x => x.PrecioPromocion)
                        .Include(x => x.InstructoresLink)
                        .ThenInclude(y => y.Instructor)
                        .FirstAsync(z => z.CursoId == request._Id);

                if (_Curso == null)
                {
                    //throw new Exception("Curso no existe..!");
                    throw new ManejadorException(HttpStatusCode.NotFound, new { Mensaje = "No se encontro el curso" });
                }
                var cursoDto = mapper.Map<Curso,CursoDto>(_Curso); 
                return cursoDto;
            }
        }
    }
} 
