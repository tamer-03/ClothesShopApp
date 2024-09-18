namespace ClothesShop.Api.Response
{
    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public T? Data { get; set; }

        public ApiResponse(bool succes, string message, T? data)
        {
            Success = succes;
            Message = message;
            Data = data;
        }
    }
}
