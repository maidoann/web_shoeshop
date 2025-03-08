namespace WEB_MQDD_SHOESHOP_API.Models
{
    public class PaginatedList<T>
    {
        public List<T> Items { get; }
        public int TotalItems { get; }
        public int CurrentPage { get; }
        public int PageSize { get; }
        public int TotalPages => (int)Math.Ceiling(TotalItems / (double)PageSize);
        public bool HasPreviousPage => CurrentPage > 1;
        public bool HasNextPage => CurrentPage < TotalPages;

        public PaginatedList(List<T> items, int totalItems, int currentPage, int pageSize)
        {
            Items = items;
            TotalItems = totalItems;
            CurrentPage = currentPage;
            PageSize = pageSize;
        }
    }
}