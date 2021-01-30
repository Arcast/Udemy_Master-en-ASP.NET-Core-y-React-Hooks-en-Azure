using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace Persistencia.DapperConexion.Instructor
{
    public class InstructorRepository : IInstructor
    {
        private readonly IFactoryConnection factoryConnection;
        public InstructorRepository(IFactoryConnection _factoryConnection)
        {
            factoryConnection = _factoryConnection;
        }     

        public async Task<IEnumerable<InstructorModel>> obtenerLista()
        {
            IEnumerable<InstructorModel> instructorList = null;
            var storeProcedure = "usp_Obtener_Instructores";
            try
            {
                var connection = factoryConnection.GetConnection();
                instructorList = await connection.QueryAsync<InstructorModel>(storeProcedure,null,commandType : CommandType.StoredProcedure);
            }
            catch (Exception e)
            {
                throw new Exception("Error en la consulta de datos", e);
            }
            finally
            {
                factoryConnection.CloseConnection(); 
            }
            return instructorList; 
        }

        public async Task<InstructorModel> ObtenerPorId(Guid id)
        {
            var storeProcedure = "usp_instructor_Instructor_por_id";
            InstructorModel instructor = null;
            try
            {
                var connection = factoryConnection.GetConnection();
                instructor = await connection.QueryFirstAsync<InstructorModel>
                             (storeProcedure, new
                             {
                                 InstructorId = id
                             },
                                 commandType: CommandType.StoredProcedure
                               );

                factoryConnection.CloseConnection();
                return instructor;
            }
            catch (Exception e)
            {
                throw new Exception("No se logro obtener el instructor", e); ;
            }
        }

        public async Task<int> Actualizar(Guid InstructorId, string _Nombres, string _Apellidos, string _Grado)
        {
            var storeProcedure = "usp_instructor_editar";
            try
            {
                var connection = factoryConnection.GetConnection();
                var resultado = await connection.ExecuteAsync
                             (storeProcedure, new
                             {
                                 InstructorId = Guid.NewGuid(),
                                 Nombres = _Nombres,
                                 Apellidos = _Apellidos,
                                 Grado = _Grado
                             },
                                 commandType: CommandType.StoredProcedure
                               );

                factoryConnection.CloseConnection();
                return resultado;
            }
            catch (Exception e)
            {
                throw new Exception("No se logro editar el instructor", e); ;
            }
        }

        public async Task<int> Eliminar(Guid id)
        {
            var storeProcedure = "usp_instructor_elimina";
            try
            {
                var connection = factoryConnection.GetConnection();
                var resultado = await connection.ExecuteAsync
                             (storeProcedure, new
                             {
                                 InstructorId = id
                             },
                                 commandType: CommandType.StoredProcedure
                               );

                factoryConnection.CloseConnection();
                return resultado;
            }
            catch (Exception e)
            {
                throw new Exception("No se logro eliminar el instructor", e); ;
            }           
        }

        public async Task<int> Nuevo(string _Nombres, string _Apellidos, string _Grado)
        {
            var storeProcedure = "usp_instructor_nuevo";
            try
            {
               var connection = factoryConnection.GetConnection();
               var resultado = await connection.ExecuteAsync
                            (storeProcedure, new
                                {
                                    InstructorId = Guid.NewGuid(),
                                    Nombres = _Nombres,
                                    Apellidos = _Apellidos,
                                    Grado = _Grado
                                }, 
                                commandType: CommandType.StoredProcedure
                              ); 

                factoryConnection.CloseConnection();
                return resultado;
            }
            catch (Exception e)
            {
                throw new Exception("No se logro guardar el nuevo instructor", e); ;
            }           
        }
    }
}
