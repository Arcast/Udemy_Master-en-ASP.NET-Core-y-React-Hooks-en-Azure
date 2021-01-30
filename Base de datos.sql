
create database CursosOnline
go
use CursosOnline
go

CREATE TABLE Curso(
	[CursoId] [int] IDENTITY(1,1),
	[Titulo] [nvarchar](500),
	[Descripcion] [nvarchar](1000),
	[FechaPublicacion] [datetime],
	[FotoPortada] [varbinary](max),
	)
go
create table Precio(
PrecioId int identity primary key,
PrecioActual money,
Promocion money,
CursoId int,
Constraint fk_PrecioCurso foreign key (CursoId) references Curso(CursoId)
)
go

create table Comentario(
ComentarioId int identity primary key,
Alumno nvarchar(500),
Puntaje int,
ComentarioTexto nvarchar(max),
CursoId int,
Constraint ComentarioCurso foreign key (CursoId) references Curso(CursoId)
)
go
create table Instructor(
InstructorId int identity primary key,
Nombres nvarchar(500),
Apellidos nvarchar(500),
Grado nvarchar(500),
FotoPerfil varbinary(max)
)

create table Curso_Instructor(
CursoId int,
InstructorId int,
Constraint Curso_CI foreign key (CursoId) references Curso(CursoId),
Constraint Instructor_CI foreign key (InstructorId) references Instructor(InstructorId),
primary key (CursoId,InstructorId)
)