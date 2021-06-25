using Ductus.FluentDocker;
using Ductus.FluentDocker.Builders;
using Ductus.FluentDocker.Commands;
using Ductus.FluentDocker.Model.Common;
using Ductus.FluentDocker.MsTest;
using Ductus.FluentDocker.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Linq;
using System.Net.Http;

namespace School.Integration.MsTest.Tests.Base
{
    public class BaseIntegrationTest : FluentDockerTestBase
    {
        protected string URL;
        protected HttpClient HttpClient;
        protected ContainerBuilder ContainerBuilder;

        [TestInitialize]
        public void InitialSetup()
        {
            this.URL = "http://localhost:5000/";

            this.HttpClient = new HttpClient { BaseAddress = new Uri(URL) };
        }

        // Inciando o container
        protected override ContainerBuilder Build()
        {
            var pathCurrentDirectory = Directory.GetCurrentDirectory();
            var pathDockerCompose = Path.GetFullPath(Path.Combine(pathCurrentDirectory, @"..\\..\\..\\"));
            var file = Path.Combine(pathDockerCompose, (TemplateString)"docker-compose-integration.yml");

            ContainerBuilder = Fd.UseContainer();

            ContainerBuilder.UseCompose()
                            .FromFile(file)
                            .RemoveOrphans()
                            .WaitForPort("integrationtestswithdockercontainer_web", "5000/tcp", 30000 /*30s*/)
                            .Build().Start();

            return new Builder().UseContainer()
                                .UseImage("integrationtestswithdockercontainer_web")
                                .ExposePort(5000)
                                .RemoveVolumesOnDispose()
                                .WaitForPort("5000/tcp", 30000 /*30s*/);
        }

        // Depois do container ser criado e iniciado
        protected override void OnContainerInitialized()
        {

        }

        // Antes do container ser encerrado
        protected override void OnContainerTearDown()
        {
            var hosts = new Hosts().Discover();
            var _docker = hosts.FirstOrDefault(x => x.IsNative) ?? hosts.FirstOrDefault(x => x.Name == "default");

            var id = _docker.Host.Run("integrationtestswithdockercontainer_web", null, _docker.Certificates).Data;
            var ps = _docker.Host.Ps(null, _docker.Certificates).Data;

            _docker.Host.RemoveContainer(id, true, true, null, _docker.Certificates);
            _docker.Host.RemoveContainer(ps[0], true, true, null, _docker.Certificates);
            _docker.Host.RemoveContainer(ps[1], true, true, null, _docker.Certificates);
            _docker.Host.RemoveContainer(ps[2], true, true, null, _docker.Certificates);
        }
    }
}
