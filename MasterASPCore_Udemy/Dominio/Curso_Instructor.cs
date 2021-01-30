using System;
using System.Collections.Generic;
using System.Text;

namespace Dominio
{
    public class Curso_Instructor
    {
        public Guid CursoId { get; set; }
        public Guid InstructorId { get; set; }
        public Curso Curso { get; set; }
        public Instructor Instructor { get; set; }
    }
}
