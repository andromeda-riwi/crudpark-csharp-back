using Microsoft.EntityFrameworkCore;
using CrudPark.Api.Data;
using CrudPark.Api.Services;

var builder = WebApplication.CreateBuilder(args);

// --- CONFIGURACIÓN DE SERVICIOS ---

// 1. Base de Datos
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
var dbPassword = builder.Configuration["DbPassword"];
var fullConnectionString = $"{connectionString}Password={dbPassword}";
builder.Services.AddDbContext<AppDbContext>(options => options.UseNpgsql(fullConnectionString));

// 2. CORS
var corsPolicyName = "AllowVueApp";
var vueAppOrigin = builder.Configuration["CorsOrigins"];
if (string.IsNullOrEmpty(vueAppOrigin))
{
    throw new InvalidOperationException("Configuración 'CorsOrigins' no encontrada.");
}
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: corsPolicyName, policy => {
        policy.WithOrigins(vueAppOrigin).AllowAnyHeader().AllowAnyMethod();
    });
});

builder.Services.AddScoped<IOperadorService, OperadorService>();

// --- ¡LA SOLUCIÓN ES AÑADIR ESTAS LÍNEAS! ---
builder.Services.AddScoped<IMensualidadService, MensualidadService>();
builder.Services.AddScoped<ITarifaService, TarifaService>();
builder.Services.AddScoped<ITicketService, TicketService>();
// ... aquí irían los otros servicios (Mensualidad, Tarifa, etc.) ...

// 4. API Controllers y Serialización
// EN EL PROGRAM.CS DEL BACK-END
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        // ¡ESTA ES LA CONFIGURACIÓN ESTÁNDAR Y RECOMENDADA!
        // Convierte automáticamente NombreCliente en C# a nombreCliente en JSON
        options.JsonSerializerOptions.PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase;
    });

// 5. Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


// --- CONSTRUCCIÓN DE LA APP Y PIPELINE HTTP ---
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// COMENTAR ESTA LÍNEA PARA EVITAR ERRORES 404 EN DESARROLLO LOCAL
// app.UseHttpsRedirection(); 

app.UseCors(corsPolicyName);
app.UseAuthorization();
app.MapControllers();

app.Run();