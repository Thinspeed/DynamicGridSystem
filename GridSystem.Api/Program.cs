using AppDefinition.Extensions;
using GridSystem.Api.AppDefinitions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddControllers();

builder.AddAppDefinitions();

var app = builder.Build();

app.InitAppDefinitions();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(CorsDefinition.CorsPolicyName);
app.UseRouting();
app.MapControllers();
app.Run();