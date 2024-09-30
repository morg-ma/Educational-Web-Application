using EducationalWebApplication.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace EducationalWebApplication.ViewModels
{
    public class InstructorsViewModel
    {
        public IQueryable<Instructor> Instructors { get; set; }
        public string Search { get; set; }
        public string SortOrder { get; set; }
        public PaginatedList<Instructor> Page { get; set; }
    }
}
