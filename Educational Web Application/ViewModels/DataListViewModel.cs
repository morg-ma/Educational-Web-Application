using EducationalWebApplication.Models;

namespace EducationalWebApplication.ViewModels
{
    public class DataListViewModel<TModel> where TModel : class
    {
        public IQueryable<TModel> ModelList { get; set; }
        public string Search { get; set; }
        public string SortOrder { get; set; }
        public PaginatedList<TModel> Page { get; set; }
    }
}
