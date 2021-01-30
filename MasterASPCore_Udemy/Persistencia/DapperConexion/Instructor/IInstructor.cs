using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Persistencia.DapperConexion.Instructor
{
    public interface IInstructor
    {
        Task<IEnumerable<InstructorModel>> obtenerLista();
        Task<InstructorModel> ObtenerPorId(Guid id);
        Task<int> Nuevo(string Nombres, string Apellidos, string Grado);
        Task<int> Actualizar(Guid InstructorId, string _Nombres, string _Apellidos, string _Grado);
        Task<int> Eliminar(Guid id);
    }
}
