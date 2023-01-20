using LetterboxNetCore.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace LetterboxNetCore.Repositories.Database
{
    public class UnitOfWork
    {
        private readonly ApplicationDbContext context;
        public UserRepository UserRepository { get; private set; }
        public MovieRepository MoviesRepository { get; private set; }
        public ReviewRepository ReviewsRepository { get; private set; }
        public MovieLikeRepository MovieLikesRepository { get; private set; }

        public UnitOfWork(ApplicationDbContext context, UserManager<User> userManager)
        {
            this.context = context;
            this.UserRepository = new UserRepository(this.context, userManager);
            this.MoviesRepository = new MovieRepository(this.context);
            this.ReviewsRepository = new ReviewRepository(this.context);
            this.MovieLikesRepository = new MovieLikeRepository(this.context);
        }

        public async Task SaveAsync()
        {
            AddTimestamps();
            await context.SaveChangesAsync();
        }

        public void Dispose()
        {
            context.Dispose();
        }

        private void AddTimestamps()
        {
            var entities = context.ChangeTracker
                .Entries()
                .Where(x => x.Entity is BaseEntity && (x.State == EntityState.Added || x.State == EntityState.Modified));
            foreach (var entity in entities)
            {
                if (entity.State == EntityState.Added)
                {
                    ((BaseEntity)entity.Entity).CreatedAt = DateTime.UtcNow;
                }
                ((BaseEntity)entity.Entity).UpdatedAt = DateTime.UtcNow;
            }
        }
    }
}