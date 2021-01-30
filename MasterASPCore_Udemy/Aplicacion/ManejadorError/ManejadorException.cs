using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Aplicacion.ManejadorError
{
    public class ManejadorException : Exception
    {
        public HttpStatusCode codigo{ get; }
        public object errores { get; }
        public ManejadorException(HttpStatusCode _codigo, object _errores = null)
        {
            codigo = _codigo;
            errores = _errores;
        }
    }
}
