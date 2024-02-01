using System.Text.Json.Serialization;
using WebApi.Helpers;
using WebApi.Repositories;
using WebApi.Services;
using Microsoft.OpenApi.Models;
using System.Runtime.InteropServices;

var builder = WebApplication.CreateBuilder(args);

// add services to DI container
{
    var services = builder.Services;
    var env = builder.Environment;
 
    services.AddCors();
    services.AddControllers().AddJsonOptions(x =>
    {
        // serialize enums as strings in api responses (e.g. Role)
        x.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());

        // ignore omitted parameters on models to enable optional params (e.g. User update)
        x.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
    });
    services.AddEndpointsApiExplorer();
    var infoSdk = RuntimeInformation.FrameworkDescription;
    services.AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1",
            new OpenApiInfo
            {
                Title = $"API Tech Challenge - Pós-Tech SOAT - FIAP {infoSdk}",
                Description = $"API de AutoAtendimento de Lanchonete implementada com .NET 8 ({infoSdk})",
                Version = "v1",
                Contact = new OpenApiContact()
                {
                    Name = "Armando Costa",
                    Url = new Uri("https://github.com/armandofc1"),
                },
                License = new OpenApiLicense()
                {
                    Name = "MIT",
                    Url = new Uri("http://opensource.org/licenses/MIT"),
                }
            });
    });
    services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

    // configure strongly typed settings object
    services.Configure<DbSettings>(builder.Configuration.GetSection("DbSettings"));

    // configure DI for application services
    services.AddSingleton<DataContext>();
    services.AddScoped<IUserRepository, UserRepository>();
    services.AddScoped<IUserService, UserService>();
    services.AddScoped<IClienteRepository, ClienteRepository>();
    services.AddScoped<IClienteService, ClienteService>();
    services.AddScoped<IProdutoRepository, ProdutoRepository>();
    services.AddScoped<IProdutoService, ProdutoService>();
    services.AddScoped<IPedidoRepository, PedidoRepository>();
    services.AddScoped<IPedidoService, PedidoService>();
    services.AddScoped<IItemPedidoRepository, ItemPedidoRepository>();
}

var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI();

// ensure database and tables exist
{
    using var scope = app.Services.CreateScope();
    var context = scope.ServiceProvider.GetRequiredService<DataContext>();
    await context.Init();
}

// configure HTTP request pipeline
{
    // global cors policy
    app.UseCors(x => x
        .AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader());

    // global error handler
    app.UseMiddleware<ErrorHandlerMiddleware>();

    app.MapControllers();
}

app.Run("http://localhost:4000");