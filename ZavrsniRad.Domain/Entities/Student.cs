using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZavrsniRad.Domain.Entities
{
    public class Student
    {
        // Primarni ključ = Id od IdentityUser-a
        public string Id { get; set; } = default!;

        public string? Jmbag { get; set; }
        public string? StudyProgram { get; set; }

        public string MentorId { get; set; } = default!;
        public Professor Mentor { get; set; } = default!;

        public ICollection<Thesis> Theses { get; set; } = new List<Thesis>();
    }
}
