namespace TenEightVideo.Web.Configuration
{
    public class UpdateSettings
    {
        public const string SECTION_NAME = "UpdateSettings";

        public Dictionary<string, AppUpdateConfig> Applications { get; set; } = new();
    }

    public class AppUpdateConfig
    {
        public string? FilePrefix { get; set; }
        public string? ProgramUpdatesFolder { get; set; }
    }
}
