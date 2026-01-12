using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Org.BouncyCastle.Cms;
using System.CommandLine;
using System.Net.Mail;
using TenEightVideo.JobRunner.WarrantyReports;
using TenEightVideo.Web.Data;
using TenEightVideo.Web.Mail;

namespace TenEightVideo.JobRunner
{
    internal class Program
    {
        public const string JOB_NAME_MONTHLY_WARRANTY_REPORT = "MonthlyWarrantyReport";
        public const string JOB_NAME_YEARLY_WARRANTY_REPORT = "YearlyWarrantyReport";
        private static ILogger<Program>? _logger;
        private static ILoggerFactory? _loggerFactory;
        private static IConfigurationRoot? _configuration;

        static int Main(string[] args)
        {
            ConfigureLogging();
            _logger = _loggerFactory?.CreateLogger<Program>();
            _configuration = ConfigureSettings();

            //Parse command line args
            var parser = GetRootCommand();
            var parseResult = parser.Parse(args);
            if (!parseResult.Errors.Any())
            {
                var jobName = parseResult.GetValue<string>("-job");
                ProcessJob(jobName);
                return JobStatus.Success;
            }
            else
            {
                foreach (var error in parseResult.Errors)
                {
                    Console.WriteLine(error.Message);
                }
                return JobStatus.Failure;
            }
        }

        private static IConfigurationRoot ConfigureSettings()
        {
            //Read configuration
            return new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();
        }

        private static void ConfigureLogging()
        {
            // 1. Create a LoggerFactory instance
            _loggerFactory = LoggerFactory.Create(builder =>
            {
                // Add the console logging provider
                builder.AddConsole();
                // Configure minimum log level (optional, defaults to Information)
                builder.SetMinimumLevel(LogLevel.Information);
            });                       
        }

        private static void ProcessJob(string? jobName)
        {
            switch (jobName)
            {
                case JOB_NAME_MONTHLY_WARRANTY_REPORT:
                    {
                        DoMonthlyWarrantyReportJob();                        
                        break;
                    }
                case JOB_NAME_YEARLY_WARRANTY_REPORT:
                    {
                        DoYearlyWarrantyReportJob();                        
                        break;
                    }
            }
        }

        private static JobStatus DoYearlyWarrantyReportJob()
        {
            try
            {
                var settings = _configuration!.GetSection("AppSettings").Get<AppSettings>();
                if (settings == null)
                    throw new Exception("AppSettings section is missing from configuration.");

                settings.Validate();
                
                WarrantyScheduleProcessor processor = GetWarrantyProcessor(settings);

                var emailSender = new MailAddress(settings.ServerEmailAddress!);
                var recipient = new MailAddress(settings.ServiceEmailAddress!);

                MailAddress? bcc = null;
                if (!string.IsNullOrWhiteSpace(settings.AdministratorEmailAddress))
                    bcc = new MailAddress(settings.AdministratorEmailAddress);

                var sent = processor.ProcessYearlyWarrantyReport(emailSender, recipient, bcc);
                if (sent)
                {
                    Console.WriteLine($"Yearly Warranty Part Request Report sent to {settings.ServiceEmailAddress}.");
                    _logger?.LogInformation($"Yearly Warranty Part Request Report sent to {settings.ServiceEmailAddress}.");
                }
                else
                {
                    Console.WriteLine("Yearly Warranty Part Request Report was not sent. Not enough days passed since last run.");
                    _logger?.LogInformation("Yearly Warranty Part Request Report was not sent. Not enough days passed since last run.");
                }
                return JobStatus.Success;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error processing yearly warranty report: {ex.Message}. See event log for more details.");
                _logger?.LogError(ex, "Error processing yearly warranty report.");
                return JobStatus.Failure;
            }

        }

        private static WarrantyScheduleProcessor GetWarrantyProcessor(AppSettings settings)
        {
            var connectionString = _configuration!.GetConnectionString("TenEightVideoDatabase");
            var mailLogger = _loggerFactory!.CreateLogger<GMailManager>();
            var builder = new DbContextOptionsBuilder<TenEightVideoDbContext>();
            builder.UseSqlServer(connectionString);

            var processScheduleRepository = new EFProcessScheduleRepository(builder.Options);
            var warrantyRequestPartRepository = new EFWarrantyRequestPartRepository(builder.Options);
            var mailManager = new GMailManager(
                settings.GMailGoogleUniqueId!,
                settings.GMailGoogleUser!,
                settings.GMailGoogleCertificateFileName!,
                settings.GMailGoogleCertificatePassword!,
                settings.ContentRootPath!,
                settings.MailTransformPath!,
                mailLogger
                );
            var processor = new WarrantyScheduleProcessor(
                processScheduleRepository,
                warrantyRequestPartRepository,
                mailManager);
            return processor;
        }

        private static JobStatus DoMonthlyWarrantyReportJob()
        {
            try
            {
                var settings = _configuration!.GetSection("AppSettings").Get<AppSettings>();
                if (settings == null)
                    throw new Exception("AppSettings section is missing from configuration.");

                settings.Validate();

                WarrantyScheduleProcessor processor = GetWarrantyProcessor(settings);

                var emailSender = new MailAddress(settings.ServerEmailAddress!);
                var recipient = new MailAddress(settings.ServiceEmailAddress!);

                MailAddress? bcc = null;
                if (!string.IsNullOrWhiteSpace(settings.AdministratorEmailAddress))
                    bcc = new MailAddress(settings.AdministratorEmailAddress);

                var sent = processor.ProcessMonthlyWarrantyReport(emailSender, recipient, bcc);
                if (sent)
                {
                    Console.WriteLine($"Monthly Warranty Part Request Report sent to {settings.ServiceEmailAddress}.");
                    _logger?.LogInformation($"Monthly Warranty Part Request Report sent to {settings.ServiceEmailAddress}.");
                }
                else
                {
                    Console.WriteLine("Monthly Warranty Part Request Report was not sent. Not enough days passed since last run.");
                    _logger?.LogInformation("Monthly Warranty Part Request Report was not sent. Not enough days passed since last run.");
                }
                return JobStatus.Success;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error processing Monthly warranty report: {ex.Message}. See event log for more details.");
                _logger?.LogError(ex, "Error processing Monthly warranty report.");
                return JobStatus.Failure;
            }
        }

        private static RootCommand GetRootCommand()
        {
            var rootCommand = new RootCommand("TenEightVideo Job Runner");
            rootCommand.Options.Add(new Option<string>("-job")
            {
                Required = true,
            }.AcceptOnlyFromAmong(
                JOB_NAME_MONTHLY_WARRANTY_REPORT, 
                JOB_NAME_YEARLY_WARRANTY_REPORT));
            return rootCommand;
        }
    }
}
