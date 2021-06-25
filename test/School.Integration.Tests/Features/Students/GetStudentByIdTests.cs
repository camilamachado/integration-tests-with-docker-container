using FluentAssertions;
using Newtonsoft.Json;
using NUnit.Framework;
using School.Domain.Features.Students;
using System.Net;
using System.Threading.Tasks;

namespace School.Integration.Tests.Features.Students
{
    [TestFixture]
    public class GetStudentByIdTests : BaseIntegrationTest
    {
        private string Uri => "api/students";

        [Test]
        public async Task ObterEstudantePorId_Deve_RetornarSucesso()
        {
            //Arrange
            var expectedFistName = "Camila";
            var expectedLastName = "Melo Machado";

            var studentId = 1;

            //Action
            var response = await HttpClient.GetAsync($"/{Uri}/{studentId}");

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var jsonResponse = await response.Content.ReadAsStringAsync();
            var returnedStudent = JsonConvert.DeserializeObject<Student>(jsonResponse);

            returnedStudent.Id.Should().Be(studentId);
            returnedStudent.FirstName.Should().Be(expectedFistName);
            returnedStudent.LastName.Should().Be(expectedLastName);            
        }
    }
}
