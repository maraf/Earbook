using Microsoft.EntityFrameworkCore;
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

        public async Task EnsureAccountAsync(string username)
        {
            if (await Accounts.AnyAsync(a => a.Username == username))
                return;

            await Accounts.AddAsync(new AccountModel(username));
        }
    }
}
