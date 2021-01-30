using Dominio;
using System;
using System.Collections.Generic;
using System.Text;

namespace Aplicacion.Cursos
{
    public class CursoDto
    {
        public Guid CursoId { get; set; }
        public String Titulo { get; set; }
        public String Descripcion { get; set; }
        public DateTime? FechaPublicacion { get; set; }
        public byte[] FotoPortada { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public ICollection<InstructorDto> Instructores { get; set; }
        public Precio Precio { get; set; }
        public ICollection<ComentarioDto> comentarios { get; set; }
    }
}
