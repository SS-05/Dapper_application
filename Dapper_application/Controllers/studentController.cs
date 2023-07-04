using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;

namespace Dapper_application.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class studentController : ControllerBase
    {
        private readonly IConfiguration _config;
        public studentController(IConfiguration config)
        {
            _config = config;
        }
        private static async Task<IEnumerable<student_assingment>> SelectAllStudents(SqlConnection connection)
        {
            return await connection.QueryAsync<student_assingment>("select * from student_assingment");
        }

        //get all students using get method
        [HttpGet]
        public async Task<ActionResult<List<student_assingment>>> GetStudentsDets()
        {
            using var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            IEnumerable<student_assingment> students = await SelectAllStudents(connection);
            return Ok(students);
        }

   
        //get details of a single student
        [HttpGet ("{naam}")]
        public async Task<ActionResult<student_assingment>> GetStudent(string naam)
        {
            using var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            var student = await connection.QueryFirstAsync<student_assingment>("select * from student_assingment where name=@namee",
               new {namee=naam} );
            return Ok(student);
        }

        //get dets using post method
        [HttpPost]
        public async Task<ActionResult<student_assingment>> PostStudent(student_assingment student)
        {
            using var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            await connection.ExecuteAsync("insert into student_assingment (name,yop,course,number)values(@Name,@Yop,@Course,@Number)", student);
            return Ok(await SelectAllStudents(connection));

        }

        [HttpPut]
        public async Task<ActionResult<student_assingment>> UpdateStudent(student_assingment student)
        {
            using var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            await connection.ExecuteAsync("update student_assingment set name=@Name,yop=@Yop,course=@Course,number=@Number where name=@Name", student);
            return Ok(await SelectAllStudents(connection));

        }
        [HttpDelete("{naam}")]
        public async Task<ActionResult<student_assingment>> delStudent(string naam)
        {
            using var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            await connection.ExecuteAsync("DELETE FROM student_assingment WHERE name = @namee", new { namee = naam });
            return Ok(await SelectAllStudents(connection));
        }

    }
}
        