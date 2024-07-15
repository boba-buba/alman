using Microsoft.Extensions.Logging;
//using business_log;
using Alman;

var loggerFactory = LoggerFactory.Create(builder => _ = builder
            .AddFilter("Microsoft", LogLevel.Warning)
            .AddFilter("System", LogLevel.Warning)
            .AddFilter("Program", LogLevel.Error));

//var logger = loggerFactory.CreateLogger<business_log.Program>();


