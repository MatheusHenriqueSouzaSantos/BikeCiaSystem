using ApiEstagioBicicletaria.Dtos.ClienteDtos;
using ApiEstagioBicicletaria.Entities.ClienteDomain;
using ApiEstagioBicicletaria.Entities.UsuarioDomain;
using ApiEstagioBicicletaria.Repositories;
using ApiEstagioBicicletaria.Repository.Repositorios;
using ApiEstagioBicicletaria.Seguranca;
using ApiEstagioBicicletaria.Services;
using ApiEstagioBicicletaria.Services.Interfaces;
using ApiEstagioBicicletaria.Services.LogServices;
using ApiEstagioBicicletaria.Services.LogServices.InterfacesLog;
using ApiEstagioBicicletaria.Services.ServicesLogs;
using ApiEstagioBicicletaria.Utils;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Runtime.ConstrainedExecution;
using System.Security.Claims;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;

namespace ApiEstagioBicicletaria
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddDbContext<ContextoDb>(options =>
            options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
            builder.Services.AddSwaggerGen();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddScoped<IServicoJwt,ServicoJwt>();
            builder.Services.AddScoped<IGeradorCodigoIdentificador,GeradorCodigoIndentificador>();
            builder.Services.AddScoped<IClienteService, ClienteService>();
            builder.Services.AddScoped<IProdutoService, ProdutoService>();
            builder.Services.AddScoped<IServicoService, ServicoService>();
            builder.Services.AddScoped<IUsuarioService, UsuarioService>();
            builder.Services.AddScoped<IVendaService, VendaService>();
            builder.Services.AddScoped<IVendedorService, VendedorService>();
            builder.Services.AddScoped<IFornecedorService, FornecedorService>();
            builder.Services.AddScoped<IEstoqueService, EstoqueService>();
            builder.Services.AddScoped<IEntradaEstoqueService, EntradaEstoqueService>();
            builder.Services.AddScoped<ISenhaService, SenhaService>();
            builder.Services.AddScoped<IUsuarioLogadoService,UsuarioLogadoService>();
            builder.Services.AddScoped(typeof(LogRepositorio<>));
            builder.Services.AddScoped<ClienteLogService>();
            builder.Services.AddScoped<EnderecoLogService>();
            builder.Services.AddScoped<IEstoqueLogService,EstoqueLogService>();
            builder.Services.AddScoped<IFornecedorLogService,FornecedorLogService>();
            builder.Services.AddScoped<ProdutoLogService>();
            builder.Services.AddScoped<ServicoLogService>();
            builder.Services.AddScoped<IVendedorLogService,VendedorLogService>();
            builder.Services.AddScoped<VendaLogService>();
            builder.Services.AddScoped<ItemVendaLogService>();
            builder.Services.AddScoped<ServicoVendaLogService>();
            builder.Services.AddScoped<ParcelaLogService>();
            builder.Services.AddScoped<TransacaoLogService>();
            builder.Services.AddScoped<IUsuarioLogService, UsuarioLogService>();
            builder.Services.AddScoped<IEntradaEstoqueLogService,EntradaEstoqueLogService>();
            builder.Services.AddScoped<IItemEntradaEstoqueLogService,ItemEntradaEstoqueLogService>();
            builder.Services.AddHttpContextAccessor();

            builder.Services.AddCors(options =>
            {
                //mudar quando rodar o sistema
                options.AddPolicy("PermitirTudo", policy =>
                {
                    policy.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();

                });
            });
            builder.Services.AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.WriteIndented = true;
                    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;

                    options.JsonSerializerOptions.TypeInfoResolver = new DefaultJsonTypeInfoResolver
                    {
                        Modifiers =
                        {
                            ti =>
                            {
                                if (ti.Type == typeof(Cliente))
                                {
                                    ti.PolymorphismOptions = new JsonPolymorphismOptions
                                    {
                                        TypeDiscriminatorPropertyName = "$type",
                                        IgnoreUnrecognizedTypeDiscriminators = true,
                                        DerivedTypes =
                                        {
                                            new JsonDerivedType(typeof(ClienteFisico), "fisico"),
                                            new JsonDerivedType(typeof(ClienteJuridico), "juridico")
                                        }
                                    };
                                }

                                if (ti.Type == typeof(ClienteDtoOutPut))
                                {
                                    ti.PolymorphismOptions = new JsonPolymorphismOptions
                                    {
                                        TypeDiscriminatorPropertyName = "$type",
                                        IgnoreUnrecognizedTypeDiscriminators = true,
                                        DerivedTypes =
                                        {
                                            new JsonDerivedType(typeof(ClienteFisicoDtoOutPut), "fisico"),
                                            new JsonDerivedType(typeof(ClienteJuridicoDtoOutPut), "juridico")
                                        }
                                    };
                                }
                            }
                        }
                    };
                });


            var jwtKey = builder.Configuration["Jwt:Key"];


            var bytesJwtKey=Encoding.UTF8.GetBytes(jwtKey);


            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                    {
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(bytesJwtKey),
                        NameClaimType=ClaimTypes.Name,
                        RoleClaimType=ClaimTypes.Role,
                        ClockSkew=TimeSpan.Zero
                    };
                });

            builder.Services.AddAuthorization();

            builder.Services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = context =>
                {
                    var mensagensDeErro = context.ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage)
                        .ToList();

                    return new BadRequestObjectResult(mensagensDeErro);
                };
            });

            var app = builder.Build();

            app.UseCors("PermitirTudo");

            app.UseAuthentication();
            app.UseAuthorization();
  
            app.UseSwagger();
            app.UseSwaggerUI();

            app.MapControllers();

            using(var scope = app.Services.CreateScope())
            {
                var contexto = scope.ServiceProvider.GetRequiredService<ContextoDb>();
                var senhaService = scope.ServiceProvider.GetRequiredService<ISenhaService>();

                var usuarioAdminCadastrado = contexto.Usuarios.FirstOrDefault(u => u.Email == builder.Configuration["user:email"]);
                var usuarioUtilizadoParaDemonstracao = contexto.Usuarios.FirstOrDefault(u => u.Email == "demo@bikecia.com");
                if (usuarioAdminCadastrado==null)
                {
                    Usuario usuarioAdmin = new Usuario(builder.Configuration["user:codigoUser"], builder.Configuration["user:nome"],
                        builder.Configuration["user:email"], senhaService.GerarHashDaSenha(builder.Configuration["user:senha"]),PerfilUsuario.Admin);

                    contexto.Usuarios.Add(usuarioAdmin);
                    contexto.SaveChanges();
                }
                if (usuarioUtilizadoParaDemonstracao == null)
                {
                    Usuario usuarioDemo = new Usuario("efgh","usuario demo",
                        "demo@bikecia.com", senhaService.GerarHashDaSenha("demo123"), PerfilUsuario.User);
                    contexto.Usuarios.Add(usuarioDemo);
                    contexto.SaveChanges();
                }
            }

            var port = Environment.GetEnvironmentVariable("PORT") ?? "10000";
            app.Run($"http://0.0.0.0:{port}");

        }
    }
}
