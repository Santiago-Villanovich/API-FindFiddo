using FindFiddo.Application;
using FindFiddo.DataAccess;
using FindFiddo.Repository;
using FindFiddo_server.Application;
using FindFiddo_server.DataAccess;
using FindFiddo_server.Repository;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<ITranslatorApp, TranslatorApp>();
builder.Services.AddScoped<ITranslatorRepository, TranslatorRepository>();
builder.Services.AddScoped<ITranslatorContext, TranslatorContext>();

builder.Services.AddScoped<IUsuarioApp, UsuarioApp>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserContext, UserContext>();

builder.Services.AddScoped<IDVService, DVApp>();
builder.Services.AddScoped<IDVRepository, DVRepository>();
builder.Services.AddScoped<IDVContext, DVContext>();

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
