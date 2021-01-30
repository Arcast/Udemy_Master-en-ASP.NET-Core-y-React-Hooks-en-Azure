using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using Dominio;
using System.Threading.Tasks;
using System.Threading;
using Persistencia;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace Aplicacion.Cursos
{ 
    public class Consulta
    {
        public class ListaCursos : IRequest<List<CursoDto>> {}

        public class Manejador : IRequestHandler<ListaCursos, List<CursoDto>>
        {
            private readonly CursosOnlineContext context;
            private readonly IMapper mapper;
           public Manejador(CursosOnlineContext _context, IMapper _mapper)
            {
                context = _context;
                mapper = _mapper;
            }
            public async Task<List<CursoDto>> Handle(ListaCursos request, CancellationToken cancellationToken)
            {
                var cursos = await context.Curso
                    .Include(x => x.ComentarioLista)
                    .Include(x => x.PrecioPromocion)
                    .Include(x => x.InstructoresLink)
                    .ThenInclude(x => x.Instructor)
                    .ToListAsync();

                var cursoDto = mapper.Map<List<Curso>, List<CursoDto>>(cursos);

                return cursoDto;
            }
        }



    }
}
