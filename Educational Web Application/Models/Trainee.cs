using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EducationalWebApplication.Models
{
    public class Trainee
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Enter the Trainee Name")]
        [StringLength(50, ErrorMessage = "The Trainee Name must be less than 50 characters")]
        public string Name { get; set; }
        public string ImageURL { get; set; }

        [Required(ErrorMessage = "Enter the Grade")]
        [Range(1,5)]
        public int Grade { get; set; }

        [Required(ErrorMessage = "Enter the Address")]
        public string Address { get; set; }

        public IEnumerable<CourseResult> CourseResults { get; set; }

        [Required(ErrorMessage = "Please, Select The Department")]
        [ForeignKey(nameof(Department))]
        public int DepartmentID { get; set; }
        public Department Department { get; set; }
    }
}
