using EducationalWebApplication.Models;

namespace EducationalWebApplication.ViewModels
{
    public class DepartmentsViewModel
    {
        public IQueryable<Department> Departments { get; set; }
        public string Search { get; set; }
        public string SortOrder { get; set; }
        public PaginatedList<Department> Page { get; set; }
    }
}
