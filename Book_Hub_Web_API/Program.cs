using System.Diagnostics;
using System.Text;
using System.Text.Encodings;
using System.Text.Json.Serialization;
using Book_Hub_Web_API.Data;
using Book_Hub_Web_API.Data.Enums;
using Book_Hub_Web_API.Repositories;
using Book_Hub_Web_API.Services;
using log4net.Config;
using log4net;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;


namespace Book_Hub_Web_API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                var builder = WebApplication.CreateBuilder(args);

                // Add services to the container.

                builder.Services.AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
                });
                // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
                builder.Services.AddEndpointsApiExplorer();
                builder.Services.AddSwaggerGen();


                // JWT Authentication Code Begins
                builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,

                        ValidIssuer = builder.Configuration["Jwt:Issuer"],
                        ValidAudience = builder.Configuration["Jwt:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
                    };
                });
                builder.Services.AddAuthorization();
                // JWT Authentication Code Ends


                builder.Services.AddDbContext<BookHubDBContext>();

                builder.Services.AddScoped<IEmailService, EmailService>();
                builder.Services.AddScoped<INotificationService, NotificationService>();

                builder.Services.AddScoped<ICommonRepository, CommonRepository>();
                builder.Services.AddScoped<IAdminRepository, AdminRepository>();
                builder.Services.AddScoped<IUserRepository, UserRepository>();

                builder.Services.AddCors(options =>
                {
                    options.AddPolicy("AngularAppPolicy", policy =>
                    {
                        policy.WithOrigins("http://localhost:4200")
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                    });
                });

                XmlConfigurator.Configure(new FileInfo("log4net.config"));
                builder.Services.AddSingleton(LogManager.GetLogger(typeof(Program)));

                var app = builder.Build();

                // Configure the HTTP request pipeline.
                if (app.Environment.IsDevelopment())
                {
                    app.UseSwagger();
                    app.UseSwaggerUI();
                }

                app.UseCors("AngularAppPolicy");

                app.UseHttpsRedirection();
                app.UseAuthentication();
                app.UseAuthorization();

                app.MapControllers();

                app.Run();
            }
            catch(Exception ex)
            {
                Debug.WriteLine("\n\nError captured in Main() as follows:");
                Debug.WriteLine(ex.Message, "\n\n");
            }
        }
    }
}
