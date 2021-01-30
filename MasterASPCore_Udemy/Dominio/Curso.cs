using System;
using System.Collections.Generic;
using System.Text;

namespace Dominio
{
    public class Curso
    {
        public Guid CursoId { get; set; }
        public String Titulo { get; set; }
        public String Descripcion { get; set; }
        public DateTime? FechaPublicacion { get; set; }
        public byte[] FotoPortada { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public Precio PrecioPromocion { get; set; }
        public ICollection<Comentario> ComentarioLista { get; set; }

        public ICollection<Curso_Instructor> InstructoresLink { get; set; }

    }
}
