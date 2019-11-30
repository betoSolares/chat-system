namespace backend_app.Application.Resources
{
    public class MessageResource
    {
        public string From { get; set; }
        public string Content { get; set; }
        public bool IsFile { get; set; }
        public string Path { get; set; }
    }
}