using AutoMapper;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SocialMedia.Core.CustomEntities;
using SocialMedia.Core.Interfaces;
using SocialMedia.Core.Services;
using SocialMedia.Infrastructure.Data;
using SocialMedia.Infrastructure.Filters;
using SocialMedia.Infrastructure.Interfacaes;
using SocialMedia.Infrastructure.Repositories;
using SocialMedia.Infrastructure.Services;
using System;

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

            services.Configure<PaginationOptions>(Configuration.GetSection("Pagination"));

            //Crear contexto de conexion a bd utilizando sqlserver con la cadena de conexion que esta appsetting
            services.AddDbContext<SocialMediaContext>(Option => 
            Option.UseSqlServer(Configuration.GetConnectionString("SocialMedia")));


            services.AddTransient<IPostService, PostService>();
            services.AddScoped(typeof(IRepository<>), typeof(BaseRepository<>));
            services.AddTransient<IUnitOfWork, UnitOfWork>();            
            services.AddSingleton<IUriServices>(provider =>
            {
                //obtiene el htpp context de nuestar aplicacion
                var accesor = provider.GetRequiredService<IHttpContextAccessor>();
                var request = accesor.HttpContext.Request;
                var absoluteUri = string.Concat(request.Scheme, "://",request.Host.ToUriComponent());
                return new UriServices(absoluteUri);
            });
            
            //AddSingleton manejar una unica instancia para toda la aplicación, por que el servicio no maneja estado y
            //se le envia unos parametros y devuelve una salida no se necesita una instancia cada vez que
            //se haga una solicitud es como  basicamente traba AddTransient

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

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
