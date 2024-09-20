﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EducationalWebApplication.Models
{
    public class Instructor
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Enter the Instructor Name")]
        [StringLength(50, ErrorMessage = "The Instructor Name must be less than 50 characters")]
        public string Name { get; set; }
        public string ImageURL { get; set; }

        [Required(ErrorMessage = "Enter the Salary")]
        public double Salary { get; set; }

        [Required(ErrorMessage = "Enter the Address")]
        public string Address { get; set; }

        [ForeignKey(nameof(Department))]
        public int DepartmentID { get; set; }
        public Department Department { get; set; }

        [ForeignKey(nameof(Course))]
        public int CourseID { get; set; }
        public Course Course { get; set; }

    }
}