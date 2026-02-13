using System.Text.RegularExpressions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using TenEightVideo.Web.Configuration;

namespace TenEightVideo.Web.Updates
{
    public class UpdateChecker : IUpdateChecker
    {
        private static readonly Regex VersionPattern = new(@"v([\d.]+)\.zip$", RegexOptions.IgnoreCase | RegexOptions.Compiled);

        private readonly UpdateSettings _settings;
        private readonly ILogger<UpdateChecker> _logger;

        public UpdateChecker(IOptions<UpdateSettings> settings, ILogger<UpdateChecker> logger)
        {
            _settings = settings.Value;
            _logger = logger;
        }

        public string GetLatestVersion(string appKey)
        {
            if (!_settings.Applications.TryGetValue(appKey, out var config))
            {
                throw new KeyNotFoundException($"Unknown application key: {appKey}");
            }

            if (string.IsNullOrEmpty(config.ProgramUpdatesFolder) || string.IsNullOrEmpty(config.FilePrefix))
            {
                throw new InvalidOperationException($"Application {appKey} is not fully configured.");
            }

            if (!Directory.Exists(config.ProgramUpdatesFolder))
            {
                _logger.LogWarning("Folder not found for app {AppKey}: {FolderPath}", appKey, config.ProgramUpdatesFolder);
                return "0.0.0";
            }

            var searchPattern = $"{config.FilePrefix}*.zip";
            var files = Directory.GetFiles(config.ProgramUpdatesFolder, searchPattern);

            if (files.Length == 0)
            {
                _logger.LogInformation("No matching ZIP files found for app {AppKey} in {FolderPath}", appKey, config.ProgramUpdatesFolder);
                return "0.0.0";
            }

            var latestVersion = new Version(0, 0, 0);
            var latestVersionString = "0.0.0";

            foreach (var file in files)
            {
                var filename = Path.GetFileName(file);
                var match = VersionPattern.Match(filename);
                if (!match.Success)
                    continue;

                var versionString = match.Groups[1].Value;
                if (TryParseVersion(versionString, out var version) && version > latestVersion)
                {
                    latestVersion = version;
                    latestVersionString = versionString;
                }
            }

            return latestVersionString;
        }

        private static bool TryParseVersion(string versionString, out Version version)
        {
            // Version.TryParse requires at least major.minor (2 components).
            // Pad single-component strings (e.g., "2") so they parse correctly.
            var dotCount = versionString.Count(c => c == '.');
            var padded = dotCount switch
            {
                0 => versionString + ".0.0",
                1 => versionString + ".0",
                _ => versionString
            };

            return Version.TryParse(padded, out version!);
        }
    }
}
