﻿using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Ankieter.Models;
using Ankieter.Mongo;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Ankieter.Models.Forms;

namespace Ankieter.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        private readonly IMongoDatabase _database = null;

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IOptions<MongoSettings> settings)
            : base(options)
        {
            var client = new MongoClient(settings.Value.ConnectionString);
            if (client != null)
                _database = client.GetDatabase(settings.Value.Database);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }

        public DbSet<Ankieter.Models.Forms.CreatedForm> CreatedForm { get; set; }

        public IMongoCollection<Question> Questions
        {
            get
            {
                return _database.GetCollection<Question>("Question");
            }
        }

        public IMongoCollection<Answer> Answers
        {
            get
            {
                return _database.GetCollection<Answer>("Answer");
            }
        }
    }
}
