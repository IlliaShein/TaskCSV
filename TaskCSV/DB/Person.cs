using System.ComponentModel.DataAnnotations;

namespace TaskCSV.DB
{
    public class Person
    {
        [Key]
        [Required]
        public string Phone { get; set; }
        public string Name { get; set; }
        public DateTime DateOfBirth { get; set; }
        public bool Married { get; set; }
        public decimal Salary { get; set; }

        public Person() { }

        public Person(int id, string name, DateTime dateOfBirth, bool married, string phone, decimal salary)
        {
            Name = name;
            DateOfBirth = dateOfBirth;
            Married = married;
            Phone = phone;
            Salary = salary;
        }
    }
}
