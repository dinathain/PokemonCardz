using PokemonCardz.Config;

// Setup services
var builder = WebApplication.CreateBuilder(args);
builder.Services
    .SetupIoc()
    .SetupApp();

// Setup web application
var app = builder.Build();
app.SetupApp()
    .SetupRouting()
    .SetupAuth();

app.Run();