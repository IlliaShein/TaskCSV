using TaskCSV.Interfaces;
using DbLib.Models.EntityFramework;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using TaskCSV.DTOs;
using TaskCSV.DB;

namespace TaskCSV.Managers
{
    public class PersonManager : IPersonManager
    {
        private readonly MyDbContext _dbContext;
        private readonly IMapper _mapper;

        public PersonManager(MyDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<List<PersonDTO>> GetAll()
        {
            var people = await _dbContext.Person.ToListAsync();
            return _mapper.Map<List<PersonDTO>>(people);
        }

        public async Task AddPersons(List<PersonDTO> personDtos)
        {
            var people = _mapper.Map<List<Person>>(personDtos);
            await _dbContext.Person.AddRangeAsync(people);
            await _dbContext.SaveChangesAsync();
        }

        public async Task Delete(string phone)
        {
            var person = await _dbContext.Person.FindAsync(phone);
            if (person == null)
            {
                throw new InvalidOperationException($"Person with phone \"{phone}\" not found");
            }

            _dbContext.Person.Remove(person);
            await _dbContext.SaveChangesAsync();
        }

        public async Task Change(PersonDTO changedPersonDto)
        {
            var existingPerson = await _dbContext.Person.FindAsync(changedPersonDto.Phone);
            if (existingPerson == null)
            {
                throw new InvalidOperationException($"Person with phone \"{changedPersonDto.Phone}\" not found");
            }

            _mapper.Map(changedPersonDto, existingPerson);
            await _dbContext.SaveChangesAsync();
        }
    }
}
