using Microsoft.OpenApi.Models;
using EcosaveAPI.Services;
using EcosaveAPI.Services.Interfaces;
using EcosaveAPI.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Adiciona serviços ao contêiner
builder.Services.AddControllers();

// Configura o DbContext com a string de conexão do appsettings.json
builder.Services.AddDbContext<EcosaveContext>(options =>
    options.UseOracle(builder.Configuration.GetConnectionString("DefaultConnection")));

// Registra os serviços
builder.Services.AddScoped<IGptService, GptService>();
builder.Services.AddHttpClient(); // Adiciona o HttpClient

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Ecosave API",
        Version = "v1",
        Description = "API para gestão de endereços, pontos e usuários"
    });

    options.EnableAnnotations();
});

var app = builder.Build();

// Configura o Swagger no middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Ecosave API v1");
        c.RoutePrefix = string.Empty; // Configura o Swagger para ser acessado diretamente na raiz
    });
}

app.UseHttpsRedirection();

// Mapeia os controladores
app.MapControllers();

app.Run();