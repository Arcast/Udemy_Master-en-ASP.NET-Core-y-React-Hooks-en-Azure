using System;
using System.Collections.Generic;
using System.Text;

namespace Aplicacion.Cursos
{
    public class ComentarioDto
    {
        public Guid ComentarioId { get; set; }
        public String Alumno { get; set; }
        public int Puntaje { get; set; }
        public String ComentarioTexto { get; set; }
        public Guid CursoId { get; set; }
        public DateTime? FechaCreacion { get; set; }
    }
}
