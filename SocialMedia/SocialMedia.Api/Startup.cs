using AutoMapper;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SocialMedia.Core.CustomEntities;
using SocialMedia.Core.Interfaces;
using SocialMedia.Core.Services;
using SocialMedia.Infrastructure.Extensions;
using SocialMedia.Infrastructure.Filters;
using SocialMedia.Infrastructure.Interfacaes;
using SocialMedia.Infrastructure.Interfaces;
using SocialMedia.Infrastructure.Repositories;
using SocialMedia.Infrastructure.Services;
using System;
using System.IO;
using System.Reflection;
using System.Text;
using PasswordOptions = SocialMedia.Infrastructure.Options.PasswordOptions;

namespace SocialMedia.Api
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
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            //Se utiliza este código para ignorar el error de referencia circular.
            services.AddControllers(options =>
            { 
                options.Filters.Add<GlobalExceptionFilter>();
            }).AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                //validar cuando una entidad viene null ejemplo el nos se definio
                //metadata para ese metodo entonces la ignore y no la muestre null
                options.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
            })
            //VALIDAR LAS OPCIONES DE COMPOTAMINETO DEL API MODELO. deja de validar el modelo
            .ConfigureApiBehaviorOptions(Option =>
             {
                //Option.SuppressModelStateInvalidFilter = true;
            });

            services.AddOptions(Configuration);
            //Crear contexto de conexion a bd utilizando sqlserver con la cadena de conexion que esta appsetting
            services.AddDbContexts(Configuration);

            //AddSingleton manejar una unica instancia para toda la aplicación, por que el servicio no maneja estado y
            //se le envia unos parametros y devuelve una salida no se necesita una instancia cada vez que
            //se haga una solicitud es como  basicamente traba AddTransient
            services.AddServices();

            //Para generar la documentación del api
            services.AddSwagger($"{Assembly.GetExecutingAssembly().GetName().Name}.xml");

            //Para esquema de authenticacion por token
            services.AddAuthentication(optins =>
            {
                optins.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                optins.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(Options =>
            {
                Options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = Configuration["Authentication:Issuer"],
                    ValidAudience = Configuration["Authentication:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Authentication:SecretKey"]))
                };

            });

            //registramos un filto de forma global.
            services.AddMvc(options =>
            {
                options.Filters.Add<ValidationFilter>();
            }).AddFluentValidation(options => {
                options.RegisterValidatorsFromAssemblies(AppDomain.CurrentDomain.GetAssemblies());
            });


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            //Inicializa el swaqqer para la documentación
            app.UseSwagger();
            app.UseSwaggerUI(Options =>
            {
                Options.SwaggerEndpoint("../swagger/v1/swagger.json","Social Media API");
                //Options.RoutePrefix = string.Empty; //para arrancar con swagger cuando inicia le api web
            });

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();          

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
