namespace Mandalium.API.Models
{
    public class MethodCallResponse<T>   where T : class 
    {
        public T entity { get; set; }
        public string Message { get; set; }

        public ReturnCodes? StatusCode {get; set;}

    }
}