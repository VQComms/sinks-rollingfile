using System;
using Serilog;
using Serilog.Sinks.RollingFileAlternate;

namespace ExampleApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var logger = new LoggerConfiguration().MinimumLevel.Verbose()
                .WriteTo.RollingFileAlternate(@"C:\logs\serilogtest\", fileSizeLimitBytes: 4096, retainedFileCountLimit: 5)
                .WriteTo.ColoredConsole()
                .CreateLogger();

            var loggerWithPrefix = new LoggerConfiguration().MinimumLevel.Verbose()
                .WriteTo.RollingFileAlternate(@"C:\logs\serilogtest\", fileSizeLimitBytes: 4096, retainedFileCountLimit: 5, logFilePrefix:"sample")
                .WriteTo.ColoredConsole()
                .CreateLogger();

            int messageCount = 0;
            while (true)
            {
                for (int i = 0; i < 100; i++)
                {
                    logger.Information("Message: {messageCount}", messageCount);

                    if (i % 5 == 0)
                    {
                        loggerWithPrefix.Information("Message: {messageCount}", messageCount);    
                    }
                    
                    messageCount++;
                }
                Console.WriteLine("Enter to log 100 logs...");
                Console.ReadLine();
            }
        }
    }
}
