using TaskCSV.Interfaces;
using Microsoft.AspNetCore.Mvc;
using TaskCSV.DTOs;
using System.Formats.Asn1;
using System.Globalization;
using TaskCSV.DB;
using CsvHelper;

namespace ReactCRUD.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PersonController : ControllerBase
    {
        IPersonManager _personManager;

        public PersonController(IPersonManager personManager)
        {
            _personManager = personManager;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var people = await _personManager.GetAll();
            return Ok(people);
        }

        [HttpPost]
        public async Task<IActionResult> AddNewFromCSV(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("File is empty.");

            StreamReader streamReader = null;
            CsvReader csv = null;
            streamReader = new StreamReader(file.OpenReadStream());
            csv = new CsvReader(streamReader, CultureInfo.InvariantCulture);

            var personDTOs = csv.GetRecords<PersonDTO>().ToList();

            await _personManager.AddPersons(personDTOs); 
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string phone)
        {
            await _personManager.Delete(phone);
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] PersonDTO changedPerson)
        {
            await _personManager.Change(changedPerson);
            return Ok();
        }
    }
}
