using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace PhotoCritic.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {

        public DbSet<Age> Ages { get; set; }
        public DbSet<Sex> Sexes { get; set; }
        public DbSet<Race> Races { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<Education> Educations { get; set; }
        public DbSet<Profession> Professions { get; set; }
        public DbSet<MaritalStatus> MaritalStatuses { get; set; }
        public DbSet<IncomeLevel> IncomeLevels { get; set; }
        public DbSet<OpinionatedIndividual> OpinionatedIndividuals { get; set; }
        public DbSet<OpinionatedIndividualPhoto> OpinionatedIndividualPhotos { get; set; }
        public DbSet<Photo> Photos { get; set; }

        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}