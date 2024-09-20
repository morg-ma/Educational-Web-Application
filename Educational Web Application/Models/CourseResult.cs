using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EducationalWebApplication.Models
{
    public class CourseResult
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int Score { get; set; }

        [ForeignKey(nameof(Course))]
        public int CourseID { get; set; }
        public Course Course { get; set; }

        [ForeignKey(nameof(Trainee))]
        public int TraineeID { get; set; }
        public Trainee Trainee { get; set; }

    }
}
