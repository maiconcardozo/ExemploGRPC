using ExemploGRPC.Application.Mappings;
using ExemploGRPC.Application.Services.Implementation;
using ExemploGRPC.Application.Services.Interfaces;
using ExemploGRPC.Domain.Interfaces;
using ExemploGRPC.GRPC.Services;
using ExemploGRPC.Infrastructure.Data;
using ExemploGRPC.Infrastructure.Data.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddGrpc();

// Configure Database
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") 
    ?? "Server=localhost;Port=3306;Database=exemplograpc;User=root;Password=root;";

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySql(connectionString, 
        ServerVersion.AutoDetect(connectionString),
        b => b.MigrationsAssembly("ExemploGRPC.Infrastructure")));

// Register AutoMapper
builder.Services.AddAutoMapper(typeof(MappingProfile));

// Register Unit of Work
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// Register Application Services
builder.Services.AddScoped<IClienteService, ClienteService>();
builder.Services.AddScoped<ICargoService, CargoService>();
builder.Services.AddScoped<IClienteCargoService, ClienteCargoService>();

var app = builder.Build();

// Configure the HTTP request pipeline
app.MapGrpcService<GrpcClienteService>();
app.MapGrpcService<GrpcCargoService>();
app.MapGrpcService<GrpcClienteCargoService>();

app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();
