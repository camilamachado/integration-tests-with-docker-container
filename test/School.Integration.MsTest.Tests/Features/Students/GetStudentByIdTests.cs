using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using School.Integration.MsTest.Tests.Base;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace School.Integration.MsTest.Tests.Features.Students
{
    [TestClass]
    public class GetStudentByIdTests : BaseIntegrationTest
    {
        private string Uri => "api/students";

        [TestMethod]
        public async Task TestMethod1()
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
            //var returnedStudent = JsonConvert.DeserializeObject<Student>(jsonResponse);

            //returnedStudent.Id.Should().Be(studentId);
            //returnedStudent.FirstName.Should().Be(expectedFistName);
            //returnedStudent.LastName.Should().Be(expectedLastName);
        }

        [TestMethod]
        public async Task TestMethod2()
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
            //var returnedStudent = JsonConvert.DeserializeObject<Student>(jsonResponse);

            //returnedStudent.Id.Should().Be(studentId);
            //returnedStudent.FirstName.Should().Be(expectedFistName);
            //returnedStudent.LastName.Should().Be(expectedLastName);
        }
    }
}
