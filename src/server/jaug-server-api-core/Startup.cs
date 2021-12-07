using System;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;

using FluentValidation.AspNetCore;
using Newtonsoft.Json.Serialization;

using jaug_server_api_core.Data.Contexts;
using jaug_server_api_core.Data.Repositories;
using jaug_server_api_core.Middleware;
using jaug_server_api_core.Controllers;


namespace jaug_server_api_core
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
            services.AddDbContext<CoreContext>(options => options.UseSqlServer(Configuration.GetConnectionString("CommandConStr"), options => options.EnableRetryOnFailure(
                 maxRetryCount: 4,
                 maxRetryDelay: TimeSpan.FromSeconds(1),
                 errorNumbersToAdd: new int[] { }
             )));
            services.AddHttpContextAccessor();
            services.AddSingleton<IUriService>(o =>
            {
                var accessor = o.GetRequiredService<IHttpContextAccessor>();
                var request = accessor.HttpContext.Request;
                var uri = string.Concat(request.Scheme, "://", request.Host.ToUriComponent());
                return new UriService(uri);
            });
            // Autho - https://auth0.com/blog/aspnet-web-api-authorization/
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.Authority = $"https://{Configuration["Auth0:Domain"]}/";
                options.Audience = $"https://{Configuration["Auth0:Audience"]}/";
            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "jaug_server_api_core", Version = "v1" });

                // Autho - https://auth0.com/blog/aspnet-web-api-authorization/
                var securitySchema = new OpenApiSecurityScheme
                {
                    Description = "Using the Authorization header with the Bearer scheme.",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                };

                c.AddSecurityDefinition("Bearer", securitySchema);
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    { securitySchema, new[] { "Bearer" } }
                });
            });


            services.AddApiVersioning(config =>
            {
                config.DefaultApiVersion = new ApiVersion(1, 0);
                config.AssumeDefaultVersionWhenUnspecified = true;
                config.ReportApiVersions = true;
            });

            services.AddControllers(setupAction =>
            {
            //    setupAction.ReturnHttpNotAcceptable = true;
            })
                .AddNewtonsoftJson(s =>
                {
                    // use for PATCH API call (PartialUpdate) utilizing JsonPatchDocument 
                    s.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                })
            //    .AddXmlDataContractSerializerFormatters() // support for XML, but JSON is default (https://docs.microsoft.com/en-us/aspnet/core/web-api/advanced/formatting?view=aspnetcore-6.0#content-negotiation) 
                .AddFluentValidation(fv =>
                {
                    fv.DisableDataAnnotationsValidation = true;
                    fv.RegisterValidatorsFromAssemblyContaining<Startup>();
                });
            
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            // AddSingleton - same for every request
            // AddScoped - same within a request, but different across client requests
            // AddTransient - always different
            services.AddScoped<IToolsRepository, ToolsRepository>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseMiddleware<ExceptionHandling>();
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage(); // override use of CustomExceptionHandlingMiddleware (above) when in dev
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "jaug_server_api_core v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication(); // needed because responsible for creating security context under which the code is running 
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
