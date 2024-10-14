using EducationalWebApplication.ValidationAttributes;
using EducationalWebApplication.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
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
        [Range(50, 200, ErrorMessage = "The Course Degree must be between 50 and 200")]
        public int Degree { get; set; }
        [Required]
        [DisplayName("Minimum Degree")]
        [Range(10, 100, ErrorMessage = "The Course Minimum Degree must be between 10 and 100")]
        [Remote(
            action: "CheckDegree",
            controller: "Courses",
            AdditionalFields = "Degree",
            ErrorMessage = "The Minimum-Degree must be less than the Degree!"
        )]
        public int MinDegree { get; set; }
        [Required]
        [Range(1, 6, ErrorMessage = "The Course Credits must be between 1 and 6")]
        public int Credits { get; set; }

        [IsSelected]
        [DisplayName("Department")]
        [ForeignKey(nameof(Department))]
        public int DepartmentID { get; set; }
        public Department? Department { get; set; }

        public IEnumerable<CourseResult>? CourseResults { get; set; }
        public IEnumerable<Instructor>? Instructors { get; set; }
    }
}