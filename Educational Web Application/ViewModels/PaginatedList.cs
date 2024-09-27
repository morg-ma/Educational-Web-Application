using Microsoft.EntityFrameworkCore;

namespace EducationalWebApplication.ViewModels
{
    // This is a generic class called PaginatedList<T>. 
    // It is used to handle paginated data where 'T' is a class type.
    public class PaginatedList<T> where T : class
    {
        // List to hold the paginated items of type 'T'.
        public List<T> Items { get; set; }

        // The current page number.
        public int PageNo { get; set; }

        // Total number of pages calculated based on the total items and the page size.
        public int TotalPages { get; set; }

        // Constructor to initialize the PaginatedList with the list of items, 
        // current page number, total item count, and page size.
        public PaginatedList(List<T> items, int index, int count, int pageSize)
        {
            Items = items;
            PageNo = index;

            // Calculates the total number of pages by dividing the total count of items 
            // by the page size and rounding up to the next whole number (using Math.Ceiling).
            TotalPages = (int)Math.Ceiling(count / Convert.ToDouble(pageSize));
        }

        // Property that indicates whether there is a previous page.
        // Returns true if the current page number is greater than 1.
        public bool HasPrevPage => PageNo > 1;

        // Property that indicates whether there is a next page.
        // Returns true if the current page number is less than the total number of pages.
        public bool HasNextPage => PageNo < TotalPages;

        // Static asynchronous method to create a PaginatedList from an IQueryable source.
        // This will fetch a specific page of data asynchronously from the database.
        public async static Task<PaginatedList<T>> Create(IQueryable<T> source, int pageNo, int pageSize)
        {
            // Get the total count of items in the source asynchronously.
            var count = await source.CountAsync();

            // Skip the items from previous pages and take only the items for the current page,
            // then convert them to a List asynchronously.
            var items = await source.Skip((pageNo - 1) * pageSize).Take(pageSize).ToListAsync();

            // Create and return a new PaginatedList with the fetched items, current page number,
            // total count of items, and the page size.
            return new PaginatedList<T>(items, pageNo, count, pageSize);
        }
    }
}
