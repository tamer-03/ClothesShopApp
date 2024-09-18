namespace ClothesShop.Api.Response
{
    
    public class PagedResponse<T> : ApiResponse<T>
    {
        public int TotalItems { get; set; }
        public int TotalPages { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }

        public PagedResponse(bool success, string message, T data, int totalItems, int totalPages, int page, int pageSize)
            : base(success, message, data)
        {
            TotalItems = totalItems;
            TotalPages = totalPages;
            Page = page;
            PageSize = pageSize;
        }
    }

    
}
