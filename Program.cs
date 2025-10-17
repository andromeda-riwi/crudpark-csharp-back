// --- USINGS ESENCIALES ---
using Microsoft.EntityFrameworkCore;
// TODO: Reemplaza "CrudPark.Api" si tu proyecto tiene un nombre base diferente en las siguientes líneas.
using CrudPark.Api.Data; 
using CrudPark.Api.Services;

// --- INICIO DE LA CONFIGURACIÓN ---
var builder = WebApplication.CreateBuilder(args);

// --- 1. CONFIGURACIÓN DE SERVICIOS (EL "BUILDER" O CONTENEDOR DE SERVICIOS) ---

// 1.1 Configuración de la Base de Datos (PostgreSQL)
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
var dbPassword = builder.Configuration["DbPassword"]; // Lee el secreto
var fullConnectionString = $"{connectionString}Password={dbPassword}";

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(fullConnectionString));

// 1.2 Configuración de CORS (Cross-Origin Resource Sharing) - VERSIÓN SEGURA
var corsPolicyName = "AllowVueApp";
var vueAppOrigin = builder.Configuration["CorsOrigins"];

// Verificación para evitar el warning CS8604 y asegurar que la configuración existe.
if (string.IsNullOrEmpty(vueAppOrigin))
{
    throw new InvalidOperationException("La configuración 'CorsOrigins' no se encuentra en appsettings.json y es requerida.");
}

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: corsPolicyName,
                      policy =>
                      {
                          // Ahora el compilador sabe que vueAppOrigin no es nulo aquí.
                          policy.WithOrigins(vueAppOrigin)
                                .AllowAnyHeader()
                                .AllowAnyMethod();
                      });
});

// 1.3 Registro de Servicios de Lógica de Negocio
builder.Services.AddScoped<IOperadorService, OperadorService>();
builder.Services.AddScoped<IMensualidadService, MensualidadService>();
builder.Services.AddScoped<ITarifaService, TarifaService>();
builder.Services.AddScoped<ITicketService, TicketService>();

// 1.4 Otros Servicios Esenciales de la API
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


// --- 2. CONSTRUCCIÓN DE LA APLICACIÓN Y CONFIGURACIÓN DEL PIPELINE HTTP ---
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors(corsPolicyName);
app.UseAuthorization();
app.MapControllers();


// --- 3. EJECUCIÓN DE LA APLICACIÓN ---
app.Run();