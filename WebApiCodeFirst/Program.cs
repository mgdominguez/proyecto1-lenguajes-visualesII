using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using WebApiCodeFirst.Data;
using WebApiCodeFirst.Mappers;
using WebApiCodeFirst.Models;
using WebApiCodeFirst.Repositorios;
using WebApiCodeFirst.Repositorios.IRepositorios;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//Agregar la conexion a la base de datos al container
builder.Services.AddDbContext<ApplicationDbContext>(
    opciones => opciones.UseSqlServer(
        builder.Configuration.GetConnectionString("ConexionSql")
    )
);

//Agregar el servicio de Identity al container
builder.Services.AddIdentity<AppUsuario, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

//Agregar los repositorios al container
builder.Services.AddScoped<ICategoriaRepositorio, CategoriaRepositorio>();
builder.Services.AddScoped<IUsuarioRepositorio, UsuarioRepositorio>();

//Agregar el AutoMapper
builder.Services.AddAutoMapper(typeof(ApiMapper));

var key = builder.Configuration.GetValue<string>("ApiSettings:Secreta");

//Aquí se configura la Autenticación
builder.Services.AddAuthentication
    (
        x =>
        {
            x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }
    ).AddJwtBearer(x =>
    {
        x.RequireHttpsMetadata = false;
        x.SaveToken = true;
        x.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key)),
            ValidateIssuer = false,
            ValidateAudience = false
        };
    });

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

//Configuración de Swagger para JWT
builder.Services.AddSwaggerGen((options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description =
        "Autenticación JWT usando el esquema Bearer. \r\n\r\n " +
        "Ingresa la palabra 'Bearer' seguido de un [espacio] y después su token en el campo de abajo.\r\n\r\n" +
        "Ejemplo: \"Bearer tkljk125jhhk\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Scheme = "Bearer"
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement()
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                Scheme = "oauth2",
                Name = "Bearer",
                In = ParameterLocation.Header
            },
            new List<string>()
        }
    });
}));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();//Habilitar Autenticacion
app.UseAuthorization();

app.MapControllers();

app.Run();