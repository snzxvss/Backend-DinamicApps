namespace Models.Response
{
    public class ResponseModel<T>
    {
        public string Message { get; set; }
        public List<T> Content { get; set; }
        public int StatusCode { get; set; }
        public string? Error { get; set; }
    }
}
