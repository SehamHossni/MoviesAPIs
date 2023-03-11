
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using MoviesAPI.Models;
using MoviesAPI.Services;

namespace MoviesAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddCors();
            builder.Services.AddDbContext<ApplicationDbContext>(Options =>
             Options.UseSqlServer(builder.Configuration.GetConnectionString(name:"CS")) );
            builder.Services.AddScoped<IGeneresService, GeneresService>();
            builder.Services.AddScoped<IMoviesService, MovieService>();
            builder.Services.AddAutoMapper(typeof(Program));

            builder.Services.AddSwaggerGen(options => 
            {
                //options.SwaggerDoc(name: "V1", info: new Microsoft.OpenApi.Models.OpenApiInfo
                //{
                //    Version= "v1",
                //    Description="My First Api",
                //    Contact = new Microsoft.OpenApi.Models.OpenApiContact
                //    { 
                //     Name = "Seham",
                //     Email = "seham.hossny@gmail.com",
                //     Url = new Uri(uriString:"https://www.google.com")
                //    },
                //    License = new Microsoft.OpenApi.Models.OpenApiLicense
                //    { 
                //    Name= "License",
                //    Url = new Uri(uriString: "https://www.google.com")
                //    }
                //});
                options.AddSecurityDefinition(name: "Bearer", securityScheme: new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = Microsoft.OpenApi.Models.ParameterLocation.Header,
                    Description = "Enter your JWT Key"
                });
                options.AddSecurityRequirement(securityRequirement: new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
                {
                    {
                    new OpenApiSecurityScheme
                    {
                    Reference = new OpenApiReference
                    {
                     Type = ReferenceType.SecurityScheme,
                     Id = "Bearer",

                    },
                    Name = "Bearer",
                    In = Microsoft.OpenApi.Models.ParameterLocation.Header,
                    },
                    new List<string>()
                    }
                });
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseCors(c => c.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}