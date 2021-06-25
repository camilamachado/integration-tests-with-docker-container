using NUnit.Framework;
using School.Domain.Features.Students;
using System.Threading.Tasks;
using School.Infra.Extensions;
using FluentAssertions;
using System.Net;

namespace School.Integration.Tests.Features.Students
{
    [TestFixture]
    public class CreateStudentTests : BaseIntegrationTest
    {
        private string Uri => "api/students";

        [Test]
        public async Task CriarEstudante_Deve_RetornarSucesso()
        {
            //Arrange
            Student student = new Student()
            {
                FirstName = "Camila",
                LastName = "Melo Machado"
            };

            //Action

            var response = await HttpClient.PostAsync($"/{Uri}", student);

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }
    }
}