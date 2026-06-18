var builder = WebApplication.CreateBuilder(args);

// Agregamos gRPC y el servicio de Reflexión (vital para Postman)
builder.Services.AddGrpc();
builder.Services.AddGrpcReflection();

var app = builder.Build();

// Mapear tu servicio de Trivia
app.MapGrpcService<GrpcTest.Services.TriviaService>();

app.MapGrpcReflectionService();
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client.");

app.Run();
