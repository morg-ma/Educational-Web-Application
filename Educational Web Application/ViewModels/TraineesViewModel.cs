using EducationalWebApplication.Models;

namespace EducationalWebApplication.ViewModels
{
    public class TraineesViewModel
    {
        public IQueryable<Trainee> Trainees { get; set; }
        public string Search { get; set; }
        public string SortOrder { get; set; }
        public PaginatedList<Trainee> Page { get; set; }
    }
}
