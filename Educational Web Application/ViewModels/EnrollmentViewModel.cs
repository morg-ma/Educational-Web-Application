using EducationalWebApplication.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace EducationalWebApplication.ViewModels
{
    [NotMapped]
    public class EnrollmentViewModel
    {
        public int TraineeId { get; set; }
        public string Name { get; set; }
        public string? ImageURL { get; set; }
        public int Grade { get; set; }
        public string Address { get; set; }
        public int DepartmentID { get; set; }
        public Department? Department { get; set; }
    }
}
