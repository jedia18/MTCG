using Microsoft.EntityFrameworkCore;

namespace MTCG_Web_API.Authentication
{
    public class AppDataContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseNpgsql("ConnString");
        }
    }
}
