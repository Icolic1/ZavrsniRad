using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZavrsniRad.Domain.Entities
{
    public class Professor
    {
        // Primarni ključ = Id od IdentityUser-a
        public string Id { get; set; } = default!;

        public string FirstName { get; set; } = default!;
        public string LastName { get; set; } = default!;
        public string? AcademicTitle { get; set; }
        public string? Department { get; set; }

        // Navigacije
        public ICollection<Student> Students { get; set; } = new List<Student>();
        public ICollection<Thesis> Theses { get; set; } = new List<Thesis>();
    }
}
