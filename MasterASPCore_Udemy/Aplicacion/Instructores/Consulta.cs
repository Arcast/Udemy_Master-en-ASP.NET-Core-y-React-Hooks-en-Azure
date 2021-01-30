using MediatR;
using Persistencia.DapperConexion.Instructor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Aplicacion.Instructores
{
    public class Consulta
    {
        public class ListaInstructor : IRequest<List<InstructorModel>> { }

        public class Manejador : IRequestHandler<ListaInstructor, List<InstructorModel>>
        {
            private readonly IInstructor instructorRepository;
            public Manejador(IInstructor _instructorRepository)
            {
                instructorRepository = _instructorRepository;
            }
            public async Task<List<InstructorModel>> Handle(ListaInstructor request, CancellationToken cancellationToken)
            {
                var resultado = await instructorRepository.obtenerLista();
                return resultado.ToList();
            }
        }
    }
}
