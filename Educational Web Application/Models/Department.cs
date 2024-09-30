using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace EducationalWebApplication.Models
{
    public class Department
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Enter the Department Name")]
        [StringLength(50, ErrorMessage = "The Department Name must be less than 50 characters")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Enter the Manager Name")]
        [StringLength(50, ErrorMessage = "The Manager Name must be less than 50 characters")]
        [DisplayName("Manager Name")]
        public string ManagerName { get; set; }

        public IEnumerable<Course>? Courses { get; set; }
        public IEnumerable<Instructor>? Instructors { get; set; }
        public IEnumerable<Trainee>? Trainees { get; set; }
    }
}
