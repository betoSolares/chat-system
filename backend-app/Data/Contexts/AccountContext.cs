using backend_app.Domain.Contexts;

namespace backend_app.Data.Contexts
{
    public class AccountContext : IAccountContext
    {
        public string CollectionName { get; set; }
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }
}