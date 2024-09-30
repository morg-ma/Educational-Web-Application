using EducationalWebApplication.Models;

namespace EducationalWebApplication.ViewModels
{
    public class CoursesViewModel
    {
        public IQueryable<Course> Courses { get; set; }
        public string Search { get; set; }
        public string SortOrder { get; set; }
        public PaginatedList<Course> Page { get; set; }
    }
}
