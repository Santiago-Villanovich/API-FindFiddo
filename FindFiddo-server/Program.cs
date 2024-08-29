using FindFiddo.Application;
using FindFiddo.DataAccess;
using FindFiddo_server.DataAccess;
using FindFiddo_server.Repository;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IDigitoVerificadorService, DVApp>();

builder.Services.AddScoped<ITranslatorContext, TranslatorContext>();
builder.Services.AddScoped<ITranslatorRepository, TranslatorRepository>();

builder.Services.AddScoped<IDVContext, DVContext>();

builder.Services.AddScoped<IUsuarioApp, UsuarioApp>();
builder.Services.AddScoped<IUserContext, UserContext>();

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
