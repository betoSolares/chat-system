namespace backend_app.Domain.Models
{
    public class Response<T>
    {
        public bool Succes { get; set; }
        public string Message { get; set; }
        public int StatusCode { get; set; }

        public T Resource { get; set; }

        /// <summary>Create a success response</summary>
        /// <param name="resource">The new resource of the response</param>
        public Response(T resource)
        {
            Succes = true;
            Message = string.Empty;
            StatusCode = 0;
            Resource = resource;
        }

        /// <summary>Create a failed response</summary>
        /// <param name="message">The error message</param>
        /// <param name="statuscode">The status code of the response</param>
        public Response(string message, int statuscode)
        {
            Succes = false;
            Message = message;
            StatusCode = statuscode;
            Resource = default;
        }
    }
}