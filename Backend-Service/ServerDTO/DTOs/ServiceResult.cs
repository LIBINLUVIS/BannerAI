namespace ServerDTO;

public class ServiceResult<T>
{
    public T Data { get; set; }
    public bool Success { get; set; }
    public string ErrorMessage { get; set; }
    public string Message { get; set; }
    public int StatusCode { get; set; }
}