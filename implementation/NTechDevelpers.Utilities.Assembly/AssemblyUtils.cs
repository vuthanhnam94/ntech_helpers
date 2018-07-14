namespace NTechDevelpers.Utilities.Assembly
{
    using Common.Logging;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;

    /// <summary>
    /// The AssemblyUtils class
    /// </summary>
    public static class AssemblyUtils
    {
        /// <summary>
        /// The logger instance
        /// </summary>
        private static readonly ILog Logger = LogManager.GetLogger(typeof(AssemblyUtils));

        /// <summary>
        /// Define assembly pattern to scan
        /// </summary>
        public static Func<string, bool> AssemblyPatternFunc = s => !string.IsNullOrEmpty(s) && s.StartsWith(VersionScanningPattern);

        /// <summary>
        /// Version scanning pattern
        /// </summary>
        private const string VersionScanningPattern = "NTechDevelopers.";

        /// <summary>
        /// Getfolders the version.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        /// <returns>Version of folder</returns>
        public static string GetfolderVersion(string filePath)
        {
            var extension = new List<string>() { "*.dll", "*.exe" };
            var version = string.Empty;
            var listVersion = new List<Version>();
            foreach (var item in extension)
            {
                FileUtils.SafeAccess(
                    () =>
                    {
                        var files = new DirectoryInfo(filePath).GetFiles(item, SearchOption.AllDirectories);
                        foreach (var file in files)
                        {
                            var name = file.FullName;
                            var versionStr = FileVersionInfo.GetVersionInfo(name).FileVersion;
                            if (!string.IsNullOrEmpty(versionStr))
                            {
                                var versionInfo = new Version(versionStr);
                                listVersion.Add(versionInfo);
                            }
                        }
                    },
                    filePath);
            }

            if (listVersion.Count > 0)
            {
                version = listVersion.Max().ToString();
            }

            return version;
        }

        /// <summary>
        /// Get version of file
        /// </summary>
        /// <param name="filePath">path of directory</param>
        /// <returns>System.String.</returns>
        public static string GetMaxFileVersion(string filePath)
        {
            var extension = new List<string>() { "*.dll", "*.exe" };
            var version = string.Empty;
            var listVersion = new List<Version>();
            var assemblyVersion = new Version();
            foreach (var item in extension)
            {
                FileUtils.SafeAccess(
                    () =>
                    {
                        var files = new DirectoryInfo(filePath).GetFiles(item, SearchOption.AllDirectories);
                        foreach (var file in files)
                        {
                            var name = file.FullName;
                            if (file.Name.StartsWith(VersionScanningPattern))
                            {
                                var versionStr = FileVersionInfo.GetVersionInfo(name).FileVersion;
                                if (!string.IsNullOrEmpty(versionStr))
                                {
                                    var versionInfo = new Version(versionStr);
                                    listVersion.Add(versionInfo);
                                }
                            }
                        }
                    },
                    filePath);
            }

            if (listVersion.Count > 0)
            {
                version = listVersion.Max().ToString();
            }

            return version;
        }

    }
}
