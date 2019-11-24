namespace backend_app.Domain.Contexts
{
    public interface IDatabaseContext
    {
        string ConnectionString { get; set; }
        string DatabaseName { get; set; }
    }
}