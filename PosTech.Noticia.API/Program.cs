using FluentValidation;
using MediatR;
using PosTech.Noticia.API.Commands.Noticia;
using PosTech.Noticia.API.Data;
using PosTech.Noticia.API.Noticias.Update;
using PosTech.Noticia.API.Noticias;
using PosTech.Noticia.API.Repositories;
using PosTech_Fase2.Application.Queries.Noticias.GetById;
using System.Reflection;
using PosTech_Fase.Queries.Login;
using PosTech.Noticia.API.Noticias.Delete;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PosTech.Usuario.API.Commands;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddMediatR(Assembly.GetExecutingAssembly());
builder.Services.AddDbContext<DbContextClass>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("WebApiDatabase"))
);

builder.Configuration.AddJsonFile("appsettings.json", true, true)
                .AddEnvironmentVariables();

builder.Services.AddRazorPages();

builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>().Reverse();
builder.Services.AddScoped<INoticiaRepository, NoticiaRepository>().Reverse();

builder.Services.AddTransient<IValidator<GetUsuarioQuery>, GetUsuarioValidator>();
builder.Services.AddTransient<IValidator<GetNoticiaByIdQuery>, GetNoticiaByIdValidator>();
builder.Services.AddTransient<IValidator<UpdateNoticiaCommand>, UpdateNoticiaValidator>();
builder.Services.AddTransient<IValidator<DeleteNoticiaCommand>, DeleteNoticiaValidator>();
builder.Services.AddTransient<IValidator<CreateNoticiaCommand>, CreateNoticiaValidator>();

builder.Services.AddTransient<IValidator<CreateUsurarioCommand>, CreateUsurarioValidator>();


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

app.UseAuthorization();

app.MapControllers();

app.Run();

namespace PosTech.Noticia.API
{
    public partial class Program { }
}