using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;
using System.Linq;

namespace Persistencia.DapperConexion.Paginacion
{
    public class PaginacionRepositorio : IPaginacion
    {
        private readonly IFactoryConnection factoryConnection;
        public PaginacionRepositorio(IFactoryConnection _factoryConnection)
        {
            factoryConnection = _factoryConnection;
        }
        public async Task<PaginacionModel> devolverPaginacion(string storeProcedure, int numeroPagina, int cantidadElementos, IDictionary<string, object> parametrosFiltro, string ordenamientoColumna)
        {
            PaginacionModel paginacionModel = new PaginacionModel();
            List<IDictionary<string, object>> listaReporte = null;
            int totalRecords = 0;
            int totalPaginas = 0;
            try
            {
                var connection = factoryConnection.GetConnection();
                
                DynamicParameters parametros = new DynamicParameters();

                //Para los parametros de filtro
                foreach (var item in parametrosFiltro)
                {
                    parametros.Add("@" + item.Key, item.Value);
                }

                //Parametros de entrada
                parametros.Add("@NumeroPagina", numeroPagina);
                parametros.Add("@CantidadElementos", cantidadElementos);
                parametros.Add("@Ordenamiento", ordenamientoColumna);

                //Parametros de salida
                parametros.Add("@TotalRecords", totalRecords, DbType.Int32, ParameterDirection.Output);
                parametros.Add("@TotalPaginas", totalPaginas, DbType.Int32, ParameterDirection.Output);

                var result = await connection.QueryAsync(storeProcedure, parametros, commandType : CommandType.StoredProcedure );
                listaReporte = result.Select(x => (IDictionary<string,object>) x).ToList();
                paginacionModel.ListaRecords = listaReporte;
                paginacionModel.NumeroPaginas = parametros.Get<int>("@TotalPaginas");
                paginacionModel.TotalRecords = parametros.Get<int>("@TotalRecords");

            }
            catch (Exception e)
            {
                throw new Exception("No se pudo ejecutar el procedimiento almacenado ", e);
            }
            finally
            {
                factoryConnection.CloseConnection();
            }
            return paginacionModel;
        }
    }
}
