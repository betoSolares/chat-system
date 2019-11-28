namespace frontend_app.Models
{
    public class Contact
    {
        public string Username { get; set; }
        public string GivenName { get; set; }
        public string FamilyName { get; set; }
        public bool SentRequest { get; set; }
    }
}