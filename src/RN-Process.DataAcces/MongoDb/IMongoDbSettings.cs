namespace RN_Process.DataAccess.MongoDb
{
    public interface IMongoDbSettings
    {
        string ConnectionString { get; set; }
        string Database { get; set; }

    }
}