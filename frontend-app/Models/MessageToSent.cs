namespace frontend_app.Models
{
    public class MessageToSent
    {
        public string From { get; set; }
        public string To { get; set; }
        public string Content { get; set; }
        public string NewContent { get; set; }
    }
}