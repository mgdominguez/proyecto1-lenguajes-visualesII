using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using WebApiCodeFirst.Data;
using WebApiCodeFirst.Mappers;
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

//Agregar los repositorios al container
builder.Services.AddScoped<ICategoriaRepositorio, CategoriaRepositorio>();

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
builder.Services.AddSwaggerGen();

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