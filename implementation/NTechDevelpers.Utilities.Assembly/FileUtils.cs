namespace NTechDevelpers.Utilities.Assembly
{
    using Common.Logging;
    using System;
    using System.IO;

    /// <summary>
    /// The FileUtils class.
    /// </summary>
    public static class FileUtils
    {
        /// <summary>
        /// The logger instance
        /// </summary>
        private static readonly ILog Logger = LogManager.GetLogger(typeof(FileUtils));

        /// <summary>
        /// Execute read & write IO files
        /// </summary>
        /// <param name="action"></param>
        /// <param name="filePath"></param>
        public static void SafeAccess(Action action, string filePath)
        {
            try
            {
                action.Invoke();
            }
            catch (UnauthorizedAccessException ex)
            {
                var message = $"Access to the path '{filePath}' is denied";
                Logger.Error(message, ex);
                new Exception(message);
            }
            catch (PathTooLongException ex)
            {
                var message = $"Path or file [{filePath}] is longer than supported";
                Logger.Error(message, ex);
                new Exception(message);
            }
            catch (DirectoryNotFoundException ex)
            {
                var message = $"Part of a file or directory [{filePath}] cannot be found";
                Logger.Error(message, ex);
                new Exception(message);
            }
            catch (FileNotFoundException)
            {
                var message = $"Path or file [{filePath}] does not exist";
                Logger.Error(message);
                new Exception(message);
            }
            catch (IOException ex)
            {
                var message = $"General IO exception [{ex.Message}]";
                Logger.Error(message, ex);
                throw new Exception(message);
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message, ex);
                throw new Exception(ex.Message, ex);
            }
        }
    }
}
