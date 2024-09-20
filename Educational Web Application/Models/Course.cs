using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EducationalWebApplication.Models
{
    public class Course
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Enter the Course Name")]
        [StringLength(50, ErrorMessage = "The Course Name must be less than 50 characters")]
        public string Name { get; set; }
        [Required]
        public int Degree { get; set; }
        [Required]
        public int MinDegree { get; set; }
        [Required]
        public int Credits { get; set; }

        [Range(1, 1000,ErrorMessage = "Must Select Department")]
        [ForeignKey(nameof(Department))]
        public int DepartmentID { get; set; }
        public Department Department { get; set; }

        public IEnumerable<CourseResult> CourseResults { get; set; }
        public IEnumerable<Instructor> Instructors { get; set; }
    }
}