using System;
using System.Collections.Generic;
using System.Text;

namespace Aplicacion.Cursos
{
    public class InstructorDto
    {
        public Guid InstructorId { get; set; }
        public String Nombres { get; set; }
        public String Apellidos { get; set; }
        public String Grado { get; set; }
        public byte[] FotoPerfil { get; set; }
        public DateTime? FechaCreacion { get; set; }
    }
}
