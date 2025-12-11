using System.ComponentModel.DataAnnotations;

namespace ZavrsniRad.web.Models.Admin
{
    public class CreateProfessorViewModel
    {
        [Required]
        [Display(Name = "Ime")]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Prezime")]
        public string LastName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [Display(Name = "E-mail")]
        public string Email { get; set; } = string.Empty;

        [Display(Name = "Akademska titula")]
        public string? AcademicTitle { get; set; }

        [Display(Name = "Odsjek / katedra")]
        public string? Department { get; set; }
    }
}
