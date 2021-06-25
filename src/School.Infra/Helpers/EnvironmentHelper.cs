using Microsoft.Extensions.Configuration;
using System;
using System.IO;

namespace School.Infra.Helpers
{
    public static class EnvironmentHelper
    {
        public static IConfigurationBuilder GetBuilder()
        {
            var configEnvironment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            // Tivemos que alterar a lógica da leitura dos appsettings devido a uma issue relacionada ao .net core 3.0
            // https://github.com/dotnet/sdk/issues/3871
            // No .net core 3.0 durante o build os appsettings dos projetos referenciados estão sendo copiados isso resulta na sobrescrita dos 
            // appsettings dos tests fazendo com eles quebrem de uma forma imprevisível.
            var builder = new ConfigurationBuilder()
                .SetBasePath(Path.GetFullPath(Path.Combine("..", "..", "..")))
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false)
                .AddJsonFile($"appsettings.{configEnvironment}.json", optional: true, reloadOnChange: false);

            return builder;
        }
    }
}
