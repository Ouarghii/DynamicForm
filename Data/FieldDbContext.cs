// FieldDbContext.cs
using MongoDB.Driver;
using FieldAPI.Models;

namespace FieldAPI.Data
{
    public class FieldDbContext
    {
        private readonly IMongoDatabase _database;

        // Define a property for the Fields collection
        public IMongoCollection<Field> Fields { get; }

        // Define a property for the Forms collection
        public IMongoCollection<FormModel> Forms { get; }

        public FieldDbContext()
        {
            var client = new MongoClient("mongodb://localhost:27017");
            _database = client.GetDatabase("Formdynamic"); // Use the name of your database

            // Initialize the Fields collection
            Fields = _database.GetCollection<Field>("Fields");

            // Initialize the Forms collection
            Forms = _database.GetCollection<FormModel>("Forms");
        }
    }
}
