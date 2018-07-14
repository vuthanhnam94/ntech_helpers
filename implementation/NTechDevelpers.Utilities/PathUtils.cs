namespace NTechDevelpers.Utilities
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Threading;

    /// <summary>
    ///  The PathUtils class
    /// </summary>
    public static class PathUtils
    {
        /// <summary>
        /// Ensure create folder
        /// </summary>
        /// <param name="path">The path to create</param>
        public static void EnsureCreateFolder(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                return;
            }

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            while (!Directory.Exists(path))
            {
                Thread.Sleep(50);
            }
        }

        /// <summary>
        /// Get absolute path by passing a relative path to application folder 
        /// </summary>
        /// <param name="relativePath">The relative path to application folder</param>
        /// <returns>The absolute path</returns>
        public static string GetExecutingPath(string relativePath)
        {
            var asmPath = Assembly.GetEntryAssembly().Location;
            var path = Directory.GetParent(asmPath).FullName;
            var absolutePath = Path.Combine(path, relativePath);
            return absolutePath;
        }

        /// <summary>
        /// Get absolute path by passing a relative path to application folder 
        /// </summary>
        /// <returns>The absolute path</returns>
        public static string GetExecutingPath()
        {
            var asmPath = Assembly.GetEntryAssembly().Location;
            return Directory.GetParent(asmPath).FullName;
        }

        /// <summary>
        /// Get absolute path by passing a relative path to application folder
        /// </summary>
        /// <param name="relativePath">The relative path to application folder</param>
        /// <returns>The absolute path</returns>
        public static string GetAppPath(string relativePath)
        {
            return GetRelativeAppDomainBasePath(relativePath);
        }

        /// <summary>
        /// Get absolute path by passing a relative path to app domain base folder
        /// </summary>
        /// <param name="relativePath">The relative path to app domain base folder</param>
        /// <returns>The absolute path</returns>
        /// <exception cref="ArgumentNullException">
        /// Input relativePath is null
        /// </exception>
        /// <exception cref="ArgumentException">
        /// relativePath contains special character(s)
        /// </exception>
        public static string GetRelativeAppDomainBasePath(string relativePath)
        {
            var basedir = AppDomain.CurrentDomain.BaseDirectory;
            var absolutePath = Path.Combine(basedir, relativePath);
            return absolutePath;
        }

        /// <summary>
        /// The get file name from path.
        /// </summary>
        /// <param name="path">
        /// The path.
        /// </param>
        /// <returns>
        /// The System.String.
        /// </returns>
        public static string GetFileNameFromPath(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                return string.Empty;
            }

            var lastDirectorySeparatorIndex = Math.Max(path.LastIndexOf('\\'), path.LastIndexOf('/'));
            var startIndexOfFileName = lastDirectorySeparatorIndex > -1 ? lastDirectorySeparatorIndex + 1 : 0;
            return path.Substring(startIndexOfFileName);
        }

        /// <summary>
        /// Get List Directories by Path,searchPattern and SearchOption
        /// </summary>
        /// <param name="path"></param>
        /// <param name="searchPattern"></param>
        /// <param name="searchOption"></param>
        /// <returns>List directories</returns>
        public static List<string> GetDirectories(string path, string searchPattern = "*",
            SearchOption searchOption = SearchOption.TopDirectoryOnly)
        {
            if (searchOption == SearchOption.TopDirectoryOnly)
            {
                return Directory.GetDirectories(path, searchPattern).ToList();
            }

            var directories = new List<string>(GetDirectories(path, searchPattern));
            for (var i = 0; i < directories.Count; i++)
            {
                directories.AddRange(GetDirectories(directories[i], searchPattern));
            }

            return directories;
        }


        /// <summary>
        /// Gets all file in dirctory.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns>List&lt;System.String&gt;.</returns>
        public static List<string> GetAllFileInDirectory(string path)
        {
            var listFiles = new List<string>();

            var directoryInfor = new DirectoryInfo(path);

            if (!directoryInfor.Exists)
            {
                return listFiles;
            }

            var listDirectory = directoryInfor.EnumerateDirectories().ToList();

            if (listDirectory != null && listDirectory.Any())
            {
                foreach (var directory in listDirectory)
                {
                    listFiles.AddRange(GetAllFileInDirectory(directory.FullName));
                }
            }

            var listFileInDirectory = Directory.EnumerateFiles(path);
            listFiles.AddRange(listFileInDirectory);
            return listFiles;
        }

        /// <summary>
        /// Gets the path.
        /// </summary>
        /// <param name="baseDirectory">The base directory.</param>
        /// <param name="input">The sub component.</param>
        /// <returns></returns>
        public static string GetPath(string baseDirectory, string input)
        {
            var subComponent = input;

            if (subComponent.StartsWith("\\"))
            {
                subComponent = subComponent.Substring(1);
            }

            var path = Path.Combine(baseDirectory, subComponent);
            return path;
        }

    }
}
