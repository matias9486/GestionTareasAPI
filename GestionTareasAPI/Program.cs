using GestionTareas_BusinessLayer.Exceptions;
using GestionTareas_BusinessLayer.Interfaces;
using GestionTareas_BusinessLayer.Services;
using GestionTareas_DataAccessLayer.Data;
using GestionTareas_DataAccessLayer.Interfaces;
using GestionTareas_DataAccessLayer.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
//agregar context y configuracion para conectarse a sql server
builder.Services.AddDbContext<GestionTareasDbContext>(
    options => options.UseSqlServer(
        builder.Configuration.GetConnectionString("ConexionSql"))
);

//Declarar servicios
builder.Services.AddScoped<ITareasRepository, TareasRepository>();
builder.Services.AddScoped<ITareasService, TareasServices>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

//Using para recursos
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<GestionTareasDbContext>();

    //context.Database.EnsureCreated();
    context.Database.Migrate(); //aplica las migraciones pendientes
    //Cargar datos iniciales
    SeedData.Initialize(context);
}

// Configurar el middleware para manejar excepciones.
app.UseMiddleware<ExceptionHandlingMiddleware>();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
