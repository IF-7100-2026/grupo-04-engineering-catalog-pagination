using Microsoft.EntityFrameworkCore;
using WaynePartsCatalog.Api.Services;
using WaynePartsCatalog.Api.Data;

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

var builder = WebApplication.CreateBuilder(args);

// Registra los controladores de la API.
builder.Services.AddControllers();

// Configura la conexion con PostgreSQL usando la cadena definida en appsettings.json.
builder.Services.AddDbContextPool<AppDbContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("DefaultConnection")));

// Registra el servicio que contiene la logica de paginacion.
builder.Services.AddScoped<PartService>();

// Habilita Swagger para probar los endpoints durante el desarrollo.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Swagger solo se muestra en ambiente de desarrollo.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Permite servir archivos estaticos desde wwwroot.
app.UseStaticFiles();

app.UseAuthorization();

app.MapControllers();

// Permite abrir el frontend directamente desde la raiz del sitio.
app.MapFallbackToFile("index.html");

app.Run();

