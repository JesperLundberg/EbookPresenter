namespace EBookPresenter.Models
{
    public class PaginationFilter
    {
        public PaginationFilter(string sortOrder = "", int pageNumber = 1, int pageSize = 10)
        {
            SortOrder = string.IsNullOrEmpty(sortOrder) ? "creation" : sortOrder;
            PageSize = pageSize < 1 ? 1 : pageSize;
            PageNumber = pageNumber < 1 ? 10 : pageNumber;
        }

        public string SortOrder { get; }
        public int PageSize { get; }
        public int PageNumber { get; }
    }
}