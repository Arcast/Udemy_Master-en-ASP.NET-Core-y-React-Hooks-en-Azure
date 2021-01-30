using System;
using System.Collections.Generic;
using System.Text;

namespace Dominio
{
    public class Comentario
    {
        public Guid ComentarioId { get; set; }
        public String Alumno { get; set; }
        public int Puntaje { get; set; }
        public String ComentarioTexto { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public Guid CursoId { get; set; }
        public Curso Curso { get; set; }
    }
}
