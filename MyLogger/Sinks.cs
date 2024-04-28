using MongoDB.Bson;
using MongoDB.Driver;
using System.Configuration;

namespace MyLogger
{
    abstract class ISinks
    {
        public abstract void ProcessMessage(string message);
    }

    sealed class FileSink : ISinks
    {
        string filePath;

        private static readonly FileSink instance = new FileSink();

        static FileSink()
        {

        }

        private FileSink() 
        {
            filePath = "E:\\log.txt"; //ConfigurationManager.AppSettings["FilePath"];
        }

        public override async void ProcessMessage(string message)
        {
            using (StreamWriter writer = new StreamWriter(filePath, true))
            {
                await Task.Run(async () => await writer.WriteLineAsync(message));
                //writer.WriteLine(message);
            }
        }

        public static FileSink GetInstance()
        {
            return instance;
        }
    }

    sealed class ConsoleSink:ISinks
    {
        private static readonly ConsoleSink instance = new ConsoleSink();

        static ConsoleSink()
        {

        }

        private ConsoleSink()
        {

        }
        public override void ProcessMessage(string message)
        {
            Console.WriteLine(message);
            Console.Out.Flush();
        }

        public static ConsoleSink GetInstance()
        {
            return instance;
        }
    }

    sealed class DBSink:ISinks
    {
        private IMongoDatabase database;
        private IMongoCollection<BsonDocument> collection;

        string connectionString = "mongodb://localhost:27017";
        string databaseName = "MyDatabase";
        string collectionName = "MyCollection";

        private static readonly DBSink instance = new DBSink();

        static DBSink()
        {

        }

        public DBSink()
        {
            var client = new MongoClient(connectionString);
            database = client.GetDatabase(databaseName);
            collection = database.GetCollection<BsonDocument>(collectionName);
        }

        public override async void ProcessMessage(string message)
        {
            var document = new BsonDocument
            {
                { "message", message }
            };

            await Task.Run(async () => await collection.InsertOneAsync(document));
        }

        public static DBSink GetInstance()
        {
            return instance;
        }
    }
}
