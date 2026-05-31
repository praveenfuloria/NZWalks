using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace NZWalks.API.Data
{
    public class NZWalksAuthDbContext : IdentityDbContext
    {
        public NZWalksAuthDbContext( DbContextOptions<NZWalksAuthDbContext> dbContextOptions): base(dbContextOptions)
        {
                
        }

        override protected void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            var readGuid = "8f21bf82-386e-4924-ba51-b6e33adcd48c";
            var writerGuid = "64c4834c-b270-4904-90f9-b71edeb22960";

            var roles = new List<IdentityRole>()
            {
                new IdentityRole()
                {
                    Id = readGuid,
                    ConcurrencyStamp = readGuid,
                    Name = "Reader",
                    NormalizedName = "Reader".ToUpper()
                },
                new IdentityRole()
                {
                    Id = writerGuid,
                    Name = "Writer",
                    NormalizedName = "Writer".ToUpper(),
                    ConcurrencyStamp = writerGuid
                }
            };

            builder.Entity<IdentityRole>().HasData(roles);
        }
    }
}
