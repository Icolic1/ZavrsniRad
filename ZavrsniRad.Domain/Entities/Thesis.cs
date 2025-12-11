using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZavrsniRad.Domain.Entities
{
    public class Thesis
    {
        public Guid Id { get; set; }

        public string TitleHr { get; set; } = default!;
        public string TitleEn { get; set; } = default!;
        public string? Description { get; set; }

        public string StudentId { get; set; } = default!;
        public Student Student { get; set; } = default!;

        public string MentorId { get; set; } = default!;
        public Professor Mentor { get; set; } = default!;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? LastUpdatedAt { get; set; }
    }
}
