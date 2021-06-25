using Ductus.FluentDocker.Builders;
using Ductus.FluentDocker.Model.Common;
using Ductus.FluentDocker.Services;
using NUnit.Framework;
using School.Infra.Extensions;
using School.Infra.Helpers;
using School.Infra.Settings;
using System;
using System.Diagnostics;
using System.IO;
using System.Net.Http;

namespace School.Integration.Tests.Features
{
    [TestFixture]
    public class BaseIntegrationTest
    {
        protected string URL;
        protected HttpClient HttpClient;
        protected ICompositeService ContainerBuilder;
        protected string IdContainerWebAPI;

        [OneTimeSetUp]
        public void InitialSetup()
        {
            var configuration = EnvironmentHelper.GetBuilder().Build();
            var authSettings = configuration.LoadSettings<IntegrationTestsSettings>("IntegrationTestsSettings");

            this.URL = authSettings.AddressApi;

            this.HttpClient = new HttpClient { BaseAddress = new Uri(URL) };
        }

        /// <summary>
        /// É chamado uma vez ANTES da execução dos testes
        /// </summary>
        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            var pathCurrentDirectory = Directory.GetCurrentDirectory();
            var pathDockerCompose = Path.GetFullPath(Path.Combine(pathCurrentDirectory, @"..\\..\\..\\..\\..\\"));
            var file = Path.Combine(pathDockerCompose, (TemplateString)"docker-compose-integration.yml");

            ContainerBuilder = new Builder()
                                    .UseContainer()
                                    .UseCompose()
                                    .FromFile(file)
                                    .RemoveOrphans()
                                    .RemoveAllImages()
                                    .Build().Start();
        }

        /// <summary>
        /// É chamado uma vez ANTES da conclusão dos testes
        /// </summary>
        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            ContainerBuilder.Dispose();
        }

        public static void DropDockerDanglingImages()
        {
            var processInfo = new ProcessStartInfo()
            {
                FileName = "CMD", // O comando `Arguments` será executado pelo arquivo especificado no `FileName`, nesse caso utilizaremos o `CMD` como fonte
                Arguments = $"/C docker image prune --filter \"dangling = true\"",
                RedirectStandardError = true, // Habilita a resposta de erro, caso ocorra
                UseShellExecute = false // Como o comando está sendo executado direto no `Arguments` não é necessário utilizar o Shell, essa propriedade só será utilizado para abrir arquivos ou programas
            };

            try
            {
                using (var process = Process.Start(processInfo))
                {
                    var teste = process;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
