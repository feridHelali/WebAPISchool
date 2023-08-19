using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Models;
using WebAPI.Models.DTOs;
using WebAPI.Repositories;

namespace WebAPI.Controllers
{
    [Route("/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        [HttpGet]
        [Route("all", Name = nameof(GetAllStudents))]
        public ActionResult<IEnumerable<Student>> GetAllStudents ()
        {
            return Ok(CollegeRpository.Students);
        }

        [HttpGet]
        [Route("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<Student> GetStudentById (Guid id)
        {
            var student= CollegeRpository.Students.Where(n => n.Id == id).FirstOrDefault();
            if (student == null) return NotFound($"Student with id {id} is not found"); 
            return Ok(student);
        }

        [HttpPost]
        [Route("Create")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<StudentResponseDTO> PostStudent ([FromBody]AddStudentRequestDTO request)
        {
            if (request == null) return BadRequest();
            Student student = MapDTO(request);
            CollegeRpository.Students.Add(student);
            var response = new StudentResponseDTO(student.Id, student.Name, student.Gender, student.Age);
            return Ok(CreatedAtRoute("GetStudentById", new { id = student.Id },response ));
        }

        private static Student MapDTO(AddStudentRequestDTO request)
        {
            return new Student()
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                Gender = request.Gender,
                Age = request.Age,
            };
        }

        [HttpPut]
        [Route("{id:guid}", Name = nameof(UpdateStudent))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult UpdateStudent(Guid id, UpdateStudentRequestDTO request)
        {
            if(id  == Guid.Empty) return BadRequest();

            var student=CollegeRpository.Students.Where<Student>(student=>student.Id==id).FirstOrDefault();
            
            if(student == null) 
            { 
                return NotFound(); 
            }
            else
            {
                student.Name = request.Name;
                student.Gender = request.Gender;
                student.Age = request.Age;
                return NoContent();
            }


        }


        [HttpPatch]
        [Route("{id:guid}/UpdatePatial", Name = nameof(UpdatePartialStudent))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult UpdatePartialStudent(Guid id, [FromBody] JsonPatchDocument<UpdateStudentRequestDTO> patchRequest)
        {
            if (id == Guid.Empty || patchRequest == null) return BadRequest();

            var student = CollegeRpository.Students.Where<Student>(student => student.Id == id).FirstOrDefault();

            if (student == null)
            {
                return NotFound();
            }
            
            var studentRequestDTO = new UpdateStudentRequestDTO(student.Name,student.Gender,student.Age);

            patchRequest.ApplyTo(studentRequestDTO,ModelState);

            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            student.Name = studentRequestDTO.Name;
            student.Gender = studentRequestDTO.Gender;
            student.Age = studentRequestDTO.Age;

            return NoContent();

        }

        /// <summary>
        /// DeleteStudent Action that handle PUT request
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>

        [HttpDelete("{id}",Name =nameof(DeleteStudent))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<bool> DeleteStudent(Guid id)
        {
            var student = CollegeRpository.Students.Where<Student>(_student=> _student.Id==id).FirstOrDefault();
            if(student == null) { 
                return NotFound($"Can't remove Student with Id {id}");
            }
            else
            {
                CollegeRpository.Students.Remove(student);
                return Ok(true);
            }
            
        }
    }
}
