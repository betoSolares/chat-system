using backend_app.Domain.Contexts;

namespace backend_app.Data.Contexts
{
    public class DatabaseContext : IDatabaseContext
    {
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }
}