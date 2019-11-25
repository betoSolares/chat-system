namespace backend_app.Domain.Services.Communication
{
    public abstract class BaseResponse
    {
        public bool Succes { get; set; }
        public string Message { get; set; }
        public int StatusCode { get; set; }

        public BaseResponse(bool succes, string message, int statuscode)
        {
            Succes = succes;
            Message = message;
            StatusCode = statuscode;
        }
    }
}
