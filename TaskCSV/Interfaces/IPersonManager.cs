using TaskCSV.DTOs;

namespace TaskCSV.Interfaces
{
    public interface IPersonManager
    {
        Task<List<PersonDTO>> GetAll();
        Task AddPersons(List<PersonDTO> person);
        Task Delete(string phone);
        Task Change(PersonDTO changedPerson);
    }
}
