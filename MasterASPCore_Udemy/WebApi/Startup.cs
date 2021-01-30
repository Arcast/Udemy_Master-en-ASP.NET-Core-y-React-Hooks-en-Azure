using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Aplicacion.Cursos;
using Dominio;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Persistencia;
using WebApi.Controllers;
using WebApi.Middleware;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.AspNetCore.Authentication;
using Aplicacion.Contratos;
using Seguridad.TokenSeguridad;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using AutoMapper;
using Persistencia.DapperConexion;
using Persistencia.DapperConexion.Instructor;
using Microsoft.OpenApi.Models;
using Persistencia.DapperConexion.Paginacion;

namespace WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<CursosOnlineContext>(opt => {
                opt.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
            });

            //Agregar el Dapper
            services.AddOptions();
            services.Configure<ConexionConfiguracion>(Configuration.GetSection("connectionStrings"));

            services.AddMediatR(typeof(Consulta.Manejador).Assembly);
            //services.AddControllers(); asi estaba pero ahora se le agrega el fluentvalidation ¡h
            //opt => ... es para validar la Autorizacion de el token para los metodos
            services.AddControllers(opt => {
                var policy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
                opt.Filters.Add(new AuthorizeFilter(policy));
            })
                .AddFluentValidation( cfv => cfv.RegisterValidatorsFromAssemblyContaining<Nuevo>());
            //Configuracion IdentityCore
            var builder = services.AddIdentityCore<Usuario>();
            var IdentityBuilder = new IdentityBuilder(builder.UserType, builder.Services);

            //Agregar seguridad Roles
            IdentityBuilder.AddRoles<IdentityRole>();
            IdentityBuilder.AddClaimsPrincipalFactory<UserClaimsPrincipalFactory<Usuario,IdentityRole>>();

            //instanciar entity
            IdentityBuilder.AddEntityFrameworkStores<CursosOnlineContext>();
            IdentityBuilder.AddSignInManager<SignInManager<Usuario>>();

            services.TryAddSingleton<ISystemClock, SystemClock>();

            //Agregar seguridad

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("Mi Palabra Secreta"));

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(opt => {
                opt.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = key,
                    ValidateAudience = false,
                    ValidateIssuer = false
                };
            });

            //Inyectar Jwtoken
            services.AddScoped<IJwtGenerador, JwtGenerador>();

            // Para retornar el usuario log
            services.AddScoped<IUsuarioSesion, UsuarioSesion>();

            //Automaper
            services.AddAutoMapper(typeof(Consulta.Manejador));

            //Dapper 
            services.AddTransient<IFactoryConnection, FactoryConnection>();
            services.AddScoped<IInstructor, InstructorRepository>();

            //Paginacion
            services.AddScoped<IPaginacion, PaginacionRepositorio>();

            //Swager
            services.AddSwaggerGen( c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                        {
                            Title = "Services para mantenimiento de cursos",
                            Version = "v1"
                        }               
                   
                );

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme (Example: 'Bearer 12345abcdef')",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    Array.Empty<string>()
                }
            });

                c.CustomSchemaIds(c => c.FullName);
            } );
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //Se comento app.UseDeveloperExceptionPage(); para usar la configuracion de errores usando Middleware
            
            app.UseMiddleware<ManejadorErrorMiddleware>();

            if (env.IsDevelopment())
            {
                //app.UseDeveloperExceptionPage();
                
            }

            app.UseAuthentication();

            //Se comento para mientras esa linea segun el video tutorial
            //app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            //Swager
            app.UseSwagger();
            app.UseSwaggerUI(c => {
                c.SwaggerEndpoint("/swagger/v1/swagger.json","Cursos Online V1");
            });
        }
    }
} 
