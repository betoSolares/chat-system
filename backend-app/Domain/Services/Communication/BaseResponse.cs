namespace backend_app.Domain.Services.Communication
{
    public abstract class BaseResponse
    {
        public bool Succes { get; set; }
        public string Message { get; set; }

        public BaseResponse(bool succes, string message)
        {
            Succes = succes;
            Message = message;
        }
    }
}
