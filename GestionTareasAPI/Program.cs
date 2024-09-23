using GestionTareas_BusinessLayer.Exceptions;
using GestionTareas_BusinessLayer.Interfaces;
using GestionTareas_BusinessLayer.Services;
using GestionTareas_DataAccessLayer.Data;
using GestionTareas_DataAccessLayer.Interfaces;
using GestionTareas_DataAccessLayer.Models;
using GestionTareas_DataAccessLayer.Repositories;
using GestionTareasAPI;
using GestionTareasAPI.Infraestructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
//agregar context y configuracion para conectarse a sql server
builder.Services.AddDbContext<GestionTareasDbContext>(
    options => options.UseSqlServer(
        builder.Configuration.GetConnectionString("ConexionSql"))
);

//Añadir Identity al proyecto y configurar opciones de Identity
builder.Services.AddIdentityCore<Usuario>(
    options =>
    {
        options.User.RequireUniqueEmail = true;
        options.Password.RequiredLength = 6;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequireUppercase = false;
        options.Password.RequireDigit = false;
        options.SignIn.RequireConfirmedEmail = false;
    }
    )
    .AddRoles<IdentityRole<int>>()
    .AddRoleManager<RoleManager<IdentityRole<int>>>()
    .AddSignInManager<SignInManager<Usuario>>()
    .AddRoleValidator<RoleValidator<IdentityRole<int>>>()
    .AddEntityFrameworkStores<GestionTareasDbContext>();

//Declarar servicios
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddTransient<ICurrentUserService, CurrentUserService>();
builder.Services.AddScoped<ITareasRepository, TareasRepository>();
builder.Services.AddScoped<ITareasService, TareasServices>();


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(
    setup =>
    {
        setup.SwaggerDoc("v1", new OpenApiInfo { Title = "Gestion de Tareas App", Version = "v1" });
        setup.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            Name = "Authorization",
            In = ParameterLocation.Header,
            Type = SecuritySchemeType.ApiKey,
            Scheme = "Bearer",
            BearerFormat = "JWT",
            Description = "Ingrese su bearer token en este formato - Bearer {tu token} para acceder a esta API"
        });
        setup.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    },
                    Scheme = "Bearer",
                    Name = "Bearer",
                    In = ParameterLocation.Header
                },
                new string[] {}
            }
        });
    }
);


//JWT Configuration. AddAuthentication and AddJwtBearer
//AddAuthentication: Configura la autenticación para la aplicación. Esto incluye la configuración de esquemas de autenticación, como cookies, tokens JWT y otros esquemas personalizados.
//AddJwtBearer: Añade el esquema de autenticación JWT Bearer a la aplicación. Esto permite autenticar las solicitudes HTTP mediante tokens JWT.
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        //options.Authority = "https://localhost:5001";
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = false,
            ValidateIssuer = false,
            ValidateIssuerSigningKey = true,
            //ValidAudience = builder.Configuration["Jwt:Audience"],
            //ValidIssuer = builder.Configuration["Jwt:Issuer"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
        };
    });

var app = builder.Build();

//Using para recursos
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<GestionTareasDbContext>();

    //context.Database.EnsureCreated();
    context.Database.Migrate(); //aplica las migraciones pendientes
    //Cargar datos iniciales
    SeedUsersAndRoles.Initialize(services);
    //SeedData.Initialize(context);
    SeedData.Initialize(services);
}

// Configurar el middleware para manejar excepciones.
app.UseMiddleware<ExceptionHandlingMiddleware>();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//CORS
app.UseCors(
    x => x
    .AllowAnyHeader()
    .AllowAnyMethod()
    //.WithOrigins("https://mybeautifullpage.com")
    .AllowAnyOrigin()
    //.SetIsOriginAllowed(origin => true)
);

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
