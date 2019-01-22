using Microsoft.EntityFrameworkCore;
using Neptuo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Earbook.Models.Repositories
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options)
            : base(options)
        { }

        public DbSet<AccountModel> Accounts { get; private set; }
        public DbSet<EarModel> Ears { get; private set; }

        public async Task EnsureAccountAsync(string username)
        {
            if (await Accounts.AnyAsync(a => a.Username == username))
                return;

            await Accounts.AddAsync(new AccountModel(username));
        }

        public Task<AccountModel> FindAccountAsync(string username)
            => Accounts.FirstOrDefaultAsync(a => a.Username == username);

        public async Task<Guid> AddEarAsync(AccountModel owner, string name)
        {
            Ensure.NotNull(owner, "owner");
            Ensure.NotNullOrEmpty(name, "name");

            var result = await Ears.AddAsync(new EarModel()
            {
                Owner = owner,
                Name = name
            });

            return result.Entity.Id;
        }

        public Task<bool> IsExistingEarAsync(string name)
            => Ears.AnyAsync(e => e.Name == name);

        public async Task SetEarFileNameAsync(Guid earId, string fileName)
        {
            EarModel entity = await Ears.FindAsync(earId);
            entity.FileName = fileName;
        }
    }
}
