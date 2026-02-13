
using System.Threading.RateLimiting;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using TenEightVideo.Web.Configuration;
using TenEightVideo.Web.Data;
using TenEightVideo.Web.Mail;
using TenEightVideo.Web.Updates;
using TenEightVideo.Web.Warranty;


namespace TenEightVideo.Web.Services
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.Configure<ApiSettings>(builder.Configuration.GetSection(ApiSettings.SECTION_NAME));
            builder.Services.Configure<UpdateSettings>(builder.Configuration.GetSection(UpdateSettings.SECTION_NAME));

            // Add services to the container.
            builder.Services.AddLogging(loggingBuilder => 
                loggingBuilder.AddEventLog(eventLogBuilder => eventLogBuilder.SourceName = "10-8Video.com")                
                    );
            builder.Services.AddControllers();

            // Define CORS policies
            var webApiConsumersDev = "WebApiConsumersDev";
            builder.Services.AddCors(options =>
            {
                options.AddPolicy(name: webApiConsumersDev,
                    policy =>
                    {
                        policy.WithOrigins("https://develop.10-8video.com", "http://localhost:3000")
                              .AllowAnyHeader()
                              .AllowAnyMethod();
                    });
            });

            var webApiConsumersProd = "WebApiConsumersProd";
            builder.Services.AddCors(options =>
            {
                options.AddPolicy(name: webApiConsumersProd,
                    policy =>
                    {
                        policy.WithOrigins("https://www.10-8video.com", "https://10-8video.com")
                              .AllowAnyHeader()
                              .AllowAnyMethod();
                    });
            });


            builder.Services.AddEndpointsApiExplorer();
            //builder.Services.AddSwaggerGen();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();

            builder.Services.AddScoped<IMailManager, GMailManager>(serviceProvider =>
            {
                var settings = serviceProvider.GetRequiredService<IOptions<ApiSettings>>().Value;
                var logger = serviceProvider.GetRequiredService<ILogger<MailManager>>();
                var contentRootPath = builder.Environment.ContentRootPath;                
                return new GMailManager(
                    settings.GMailGoogleUniqueId!,
                    settings.GMailGoogleUser!,
                    settings.GMailGoogleCertificateFileName!,
                    settings.GMailGoogleCertificatePassword!,
                    contentRootPath, 
                    settings.MailTransformPath!, 
                    logger);
            });

            builder.Services.AddScoped<IWarrantyRequestManager, WarrantyRequestManager>();
            builder.Services.AddScoped<IUpdateChecker, UpdateChecker>();

            // Rate limiting: 10 requests per 60 seconds per IP for update checks
            builder.Services.AddRateLimiter(options =>
            {
                options.AddFixedWindowLimiter("updates", limiterOptions =>
                {
                    limiterOptions.PermitLimit = 10;
                    limiterOptions.Window = TimeSpan.FromSeconds(60);
                    limiterOptions.QueueLimit = 0;
                });
                options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
            });

            var connectionString = builder.Configuration.GetConnectionString("TenEightVideo");
            builder.Services.AddDbContext<TenEightVideoDbContext>(options => options.UseSqlServer(connectionString));

            var app = builder.Build();
            
            app.UseHttpsRedirection();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
                //app.UseSwagger();
                app.UseSwaggerUI(options => options.SwaggerEndpoint("/openapi/v1.json", "Open API"));

                // Apply the CORS policy
                app.UseCors(webApiConsumersDev);
            }
            else
            {
                // Apply the CORS policy
                app.UseCors(webApiConsumersProd);
            }

            app.UseRateLimiter();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
