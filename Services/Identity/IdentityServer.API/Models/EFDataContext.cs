using System.IO;
using Microsoft.EntityFrameworkCore;
using IdentityServer.API.Domains;
using System;

namespace IdentityServer.API.Models
{
    public class EFDataContext : DbContext
    {
        private const string databaseName = "dbEFData.db";

        #region Constructors

        public EFDataContext()
        {
            Init();
        }

        public EFDataContext(DbContextOptions options)
            : base(options)
        {
            Init();
        }

        #endregion

        #region Public Propeties

        public DbSet<User> Users { get; set; }

        #endregion

        #region Public Methods

        public override void Dispose()
        {
            base.Dispose();

            Users = null;

            GC.SuppressFinalize(this);
        }

        #endregion

        #region Private Methods

        private void Init()
        {
            Database.EnsureCreated();
        }

        #endregion

        #region OnConfiguring

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (optionsBuilder != null && !optionsBuilder.IsConfigured)
            {
                // create data directory to store database file if not exist
                var dataDirPath = Path.Combine(Directory.GetCurrentDirectory(), "Data");
                Directory.CreateDirectory(dataDirPath);

                // Specify that we will use sqlite and the path of the database here
                optionsBuilder.UseSqlite($"Filename={Path.Combine(dataDirPath, databaseName)}");
            }
        }

        #endregion
    }
}
