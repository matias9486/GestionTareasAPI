using GestionTareas_DataAccessLayer.Data;
using GestionTareas_DataAccessLayer.Models;

public class SeedData
{
    public static void Initialize(GestionTareasDbContext context) {
        if (!context.Usuario.Any())
        {

            // Agregar datos de usuarios
            var usuarios = new[]
            {
                new Usuario {
                    Name = "Matias",
                    Username = "Argento",
                    Email = "matias@gmail.com",
                    Password = "1234"
                },
                new Usuario {
                    Name = "Fernando",
                    Username = "Nando",
                    Email = "nando@gmail.com",
                    Password = "1234",
                }
            };
            context.Usuario.AddRange(usuarios);
            context.SaveChanges();
        }

        if (!context.Tarea.Any())
        {
            // Agregar datos de tareas
            var tareas = new[]
            {
                new Tarea {
                    Descripcion = "Crear entidades",
                    FechaCreacion = DateTime.Now,
                    IsTerminada = false,
                    UsuarioId = 1
                },
                new Tarea {
                    Descripcion = "Crear context",
                    FechaCreacion = DateTime.Now,
                    IsTerminada = false,
                    UsuarioId = 1
                },
                new Tarea {
                    Descripcion = "Crear Dto",
                    FechaCreacion = DateTime.Now,
                    IsTerminada = false,
                    UsuarioId = 1
                },
                new Tarea {
                    Descripcion = "Crear Servicios",
                    FechaCreacion = DateTime.Now,
                    IsTerminada = false,
                    UsuarioId = 1
                },
                new Tarea {
                    Descripcion = "Subir cambios al repo :v",
                    FechaCreacion = DateTime.Now,
                    IsTerminada = false,
                    UsuarioId = 2
                },
                new Tarea {
                    Descripcion = "Nota mental.. el niño sabe demasiado... :v",
                    FechaCreacion = DateTime.Now,
                    IsTerminada = false,
                    UsuarioId = 2
                },
                new Tarea {
                    Descripcion = "Crear Controllers",
                    FechaCreacion = DateTime.Now,
                    IsTerminada = false,
                    UsuarioId = 1
                }
            };
            context.Tarea.AddRange(tareas);
            context.SaveChanges();                       
        }
    }
}