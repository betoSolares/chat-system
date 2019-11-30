namespace frontend_app.Models
{
    public class Message
    {
        public string From { get; set; }
        public string Content { get; set; }
        public bool IsFile { get; set; }
        public string Path { get; set; }
        public bool IsFromMe { get; set; }
    }
}