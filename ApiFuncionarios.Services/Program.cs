using ApiFuncionarios.Infra.Data.Contexts;
using ApiFuncionarios.Infra.Data.Interfaces;
using ApiFuncionarios.Infra.Data.Repositories;
using ApiFuncionarios.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

SwaggerConfiguration.Register(builder);
MailConfiguration.Register(builder);

#region Configuração do Repositório

//lendo a connectionstring localizada no arquivo 'appsettings.json'
var connectionString = builder.Configuration.GetConnectionString("BDApiFuncionarios");

//configurando a classe 'SqlServerContext' do projeto Infra.Data
//para que o EntityFramework funcione corretamente
builder.Services.AddDbContext<SqlServerContext>
    (options => options.UseSqlServer(connectionString));

//injeção de dependência das interfaces / classes do repositório
builder.Services.AddTransient<IContatoRepository, ContatoRepository>();
builder.Services.AddTransient<IUsuarioRepository, UsuarioRepository>();

JwtConfiguration.Register(builder);

#endregion

#region Configuração do CORS

builder.Services.AddCors(s => s.AddPolicy("DefaultPolicy", builder =>
{
    builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
}));

#endregion

var app = builder.Build();

SwaggerConfiguration.Use(app);

app.UseAuthentication();
app.UseAuthorization();

app.UseCors("DefaultPolicy");

app.MapControllers();

app.Run();

public partial class Program { }