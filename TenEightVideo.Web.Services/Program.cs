
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using TenEightVideo.Web.Configuration;
using TenEightVideo.Web.Mail;


namespace TenEightVideo.Web.Services
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.Configure<ApiSettings>(builder.Configuration.GetSection(ApiSettings.SECTION_NAME));

            // Add services to the container.
            builder.Services.AddLogging(loggingBuilder => 
                loggingBuilder.AddEventLog(eventLogBuilder => eventLogBuilder.SourceName = "10-8Video.com")                
                    );
            builder.Services.AddControllers();
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

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
                //app.UseSwagger();
                app.UseSwaggerUI(options => options.SwaggerEndpoint("/openapi/v1.json", "Open API" ));
            }

            app.UseHttpsRedirection();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
