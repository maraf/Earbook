using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            var guidToString = new GuidToStringConverter();

            modelBuilder
                .Entity<EarModel>()
                .Property(e => e.Id)
                .HasConversion(guidToString);

            modelBuilder
                .Entity<QuizModel>()
                .Property(e => e.Id)
                .HasConversion(guidToString);

            modelBuilder
                .Entity<QuizOptionModel>()
                .Property(e => e.Id)
                .HasConversion(guidToString);
        }

        public DbSet<AccountModel> Accounts { get; private set; }
        public DbSet<EarModel> Ears { get; private set; }
        public DbSet<QuizModel> Quizzes { get; private set; }

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

        public Task<QuizModel> FindPendingQuizAsync(string username)
            => Quizzes.Where(q => q.Player.Username == username && q.IsSuccess == null).Include("Options.Option").FirstOrDefaultAsync();

        public async Task<QuizModel> EnsurePendingQuizAsync(string username)
            => (await FindPendingQuizAsync(username)) ?? await CreateQuizAsync(username);

        public async Task<QuizModel> CreateQuizAsync(string username)
        {
            AccountModel player = await Accounts.FirstAsync(a => a.Username == username);

            List<EarModel> ears = await GetRandomOptionsAsync();
            if (ears.Count < 5)
                return null;

            EarModel answer = ears.OrderBy(o => Guid.NewGuid()).First();
            List<QuizOptionModel> options = ears.Select(e => new QuizOptionModel(e)).ToList();

            QuizModel model = new QuizModel()
            {
                Player = player,
                When = DateTime.Now,
                Answer = answer,
                Options = options
            };

            await Quizzes.AddAsync(model);
            return model;
        }

        private Task<List<EarModel>> GetRandomOptionsAsync()
            => Ears.OrderBy(o => Guid.NewGuid()).Take(5).ToListAsync();
    }
}
