namespace internship.ModelView
{
    public class ApiResult<T>
    {
        public int StatusCode { get; set; }

        public string Message { get; set; } = string.Empty;

        public T? Data { get; set; }

        public ApiResult(T? resultData)
        {
            Data = resultData;
        }
        public ApiResult(int status, string message, T data)
        {
            this.StatusCode = status;
            this.Message = message;
            this.Data = data;
        }
    }
}
