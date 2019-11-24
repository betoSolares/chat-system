namespace backend_app.Domain.Contexts
{
    interface IAccountContext
    {
        string CollectionName { get; set; }
        string ConnectionString { get; set; }
        string DatabaseName { get; set; }
    }
}