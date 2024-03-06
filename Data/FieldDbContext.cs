    // FieldDbContext.cs
    using MongoDB.Driver;
    using FieldAPI.Models;
using Microsoft.EntityFrameworkCore;

    namespace FieldAPI.Data
    {
        public class FieldDbContext
        {
            private readonly IMongoDatabase _database;

            public FieldDbContext()
            {
                var client = new MongoClient("mongodb://localhost:27017");
                _database = client.GetDatabase("Formdynamic"); // Use the name of your database
            }

            public IMongoCollection<Field> Fields => _database.GetCollection<Field>("Formdynamic"); // Use the name of your collection
        public DbSet<FormModel> FormModels { get; set; }
    }
    }
