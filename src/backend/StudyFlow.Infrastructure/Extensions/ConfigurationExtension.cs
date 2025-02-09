﻿using Microsoft.Extensions.Configuration;

namespace StudyFlow.Infrastructure.Extensions
{
    public static class ConfigurationExtension
    {
        public static string ConnectionString(this IConfiguration configuration)
        {
            return configuration.GetConnectionString("ConnectionMySQLServer")!;
        }
    }
}
