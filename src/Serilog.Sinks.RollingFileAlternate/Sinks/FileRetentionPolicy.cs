using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Serilog.Debugging;

namespace Serilog.Sinks.RollingFileAlternate.Sinks
{
    public class FileRetentionPolicy
    {
        private readonly string logDirectory;
        private readonly int? retainedFileCountLimit;
        public FileRetentionPolicy(string logDirectory, int? retainedFileCountLimit)
        {
            this.logDirectory = logDirectory;
            this.retainedFileCountLimit = retainedFileCountLimit;
        }

        public void Apply(string currentFilename)
        {
            if (this.retainedFileCountLimit == null) return;

            var filename = currentFilename.Substring(0, currentFilename.LastIndexOf("-"));
            var regex = new Regex($"/.*/{filename}-[0-9]{{5}}.log");

            var newestFirst = Directory.GetFiles(this.logDirectory)
                .Where(f => regex.IsMatch(f))
                .Select(m => new FileInfo(m))
                .OrderByDescending(m => m.CreationTime)
                .Select(m => m.Name);

            var toRemove = newestFirst
                .Where(n => StringComparer.OrdinalIgnoreCase.Compare(currentFilename, n) != 0)
                .Skip(this.retainedFileCountLimit.Value - 1)
                .ToList();

            foreach (var obsolete in toRemove)
            {
                var fullPath = Path.Combine(this.logDirectory, obsolete);
                try
                {
                    File.Delete(fullPath);
                }
                catch (Exception ex)
                {
                    SelfLog.WriteLine("Error {0} while removing obsolete file {1}", ex, fullPath);
                }
            }
        }
    }
}