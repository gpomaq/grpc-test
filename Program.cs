using GrpcTest.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddGrpc();

// 1. Configurar CORS (Indispensable para el frontend web)
builder.Services.AddCors(o => o.AddPolicy("AllowAll", policy =>
{
    policy.AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader()
        // Estos headers expuestos son obligatorios para que gRPC-Web funcione
        .WithExposedHeaders("Grpc-Status", "Grpc-Message", "Grpc-Encoding", "Grpc-Accept-Encoding"); 
}));

var app = builder.Build();

app.UseCors("AllowAll");

// 2. Middleware de gRPC-Web (debe ir después de UseRouting y UseCors si los tuvieras explícitos)
app.UseGrpcWeb();

// 3. Mapear tu servicio indicando que soporta gRPC-Web
app.MapGrpcService<TriviaService>() // Reemplaza con el nombre de tu clase
    .EnableGrpcWeb(); 

// Opcional: Para devolver una respuesta si entran por la URL base desde un navegador normal
app.MapGet("/", () => "El servidor gRPC está funcionando (vía gRPC-Web).");

app.Run();