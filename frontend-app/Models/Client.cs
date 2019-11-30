using System;

namespace frontend_app.Models
{
    public class Client
    {
        public Uri URI { get; private set; }
        
        public Client()
        {
            URI = new Uri("http://localhost:52968/api/");
        }
    }
}