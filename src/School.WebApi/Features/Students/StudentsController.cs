using Microsoft.AspNetCore.Mvc;
using School.Domain.Features.Students;

namespace School.WebApi.Features.Students
{
    [Route("api/[controller]")]
    public class StudentsController
    {
        [HttpPost]
        public Student Create([FromBody] Student command)
        {
            return new Student
            {
                FirstName = command.FirstName,
                LastName = command.LastName
            };
        }

        [HttpGet("{id}")]
        public Student Get()
        {
            return new Student
            {
                Id = 1,
                FirstName = "Camila",
                LastName = "Melo Machado"
            };
        }
    }
}
