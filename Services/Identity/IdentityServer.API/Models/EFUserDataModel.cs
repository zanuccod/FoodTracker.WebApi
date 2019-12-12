using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using IdentityServer.API.Domains;

namespace IdentityServer.API.Models
{
    public class EFUserDataModel : EFDataContext, IUserDataModel
    {
        private readonly DbContextOptions options;

        #region Constructors

        public EFUserDataModel()
        {
            options = new DbContextOptionsBuilder().Options;
        }

        public EFUserDataModel(DbContextOptions options)
        {
            this.options = options;
        }

        #endregion

        #region Public Users Methods

        public async Task<List<User>> GetUsersList()
        {
            using var db = new EFDataContext(options);
            return await db.Users.ToListAsync().ConfigureAwait(true);
        }

        public async Task<User> GetUser(string username)
        {
            using var db = new EFDataContext(options);
            return await db.Users.FindAsync(username).ConfigureAwait(true);
        }

        public async Task InsertUser(User item)
        {
            using var db = new EFDataContext(options);
            await db.Users.AddAsync(item);
            await db.SaveChangesAsync().ConfigureAwait(true);
        }

        public async Task UpdateUser(User item)
        {
            using var db = new EFDataContext(options);
            db.Users.Update(item);
            await db.SaveChangesAsync().ConfigureAwait(true);
        }

        public async Task DeleteUser(string username)
        {
            using var db = new EFDataContext(options);
            var item = await db.Users.FindAsync(username);
            db.Users.Remove(item);
            await db.SaveChangesAsync().ConfigureAwait(true);
        }

        #endregion
    }
}
