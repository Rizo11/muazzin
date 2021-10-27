using Microsoft.EntityFrameworkCore;

namespace bot.Entity
{
    public class BotDbContext: DbContext
    {
        public DbSet<BotUser> Users {get;set;} 
        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            base.OnConfiguring(options);
        } 
    }
}