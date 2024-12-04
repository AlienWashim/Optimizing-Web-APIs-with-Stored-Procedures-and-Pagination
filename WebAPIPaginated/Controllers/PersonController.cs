using Microsoft.AspNetCore.Mvc;
using WebAPI.Models;
using WebAPI.Services;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonController : ControllerBase
    {
        private readonly DatabaseServices _databaseServices;

        public PersonController(DatabaseServices databaseServices)
        {
            _databaseServices = databaseServices;
        }


        // Get Paginated Persons
        [HttpGet]
        public ActionResult<IEnumerable<Person>> GetPaginatedPersons(int pageNumber = 1, int pageSize = 10)
        {
            var persons = _databaseServices.GetPaginatedPersons(pageNumber, pageSize);
            return Ok(persons);
        }

        [HttpGet("{id}")]
        public ActionResult<Person> GetPersonById(int id)
        {
            var person = _databaseServices.GetPersonById(id);
            if (person == null)
            {
                return NotFound();
            }
            return Ok(person);
        }

        [HttpPost]
        public ActionResult PostPerson([FromBody] Person person)
        {
            _databaseServices.AddPerson(person);

            return CreatedAtAction(nameof(GetPaginatedPersons), new { Id = person.Id }, person);
        }

        [HttpPut("{id}")]
        public ActionResult PutPerson(int id, [FromBody] Person person)
        {
            var oldPerson = _databaseServices.GetPersonById(id);
            if (oldPerson == null)
            {
                return NotFound();
            }

            person.Id = id;

            _databaseServices.UpdatePerson(person);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public ActionResult DeletePerson(int id)
        {
            Person oldPerson = _databaseServices.GetPersonById(id);
            if(oldPerson == null)
            {
                return NotFound();
            }

            _databaseServices.DeletePerson(id);
            return NoContent();
        }
    }
}
