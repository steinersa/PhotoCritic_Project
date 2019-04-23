namespace PhotoCritic.Migrations
{
    using PhotoCritic.Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<PhotoCritic.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(PhotoCritic.Models.ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.

            //context.Ages.AddOrUpdate(
            //      a => a.Name,
            //      new Age { Name = "18 to 24" },
            //      new Age { Name = "25 to 34" },
            //      new Age { Name = "35 to 44" },
            //      new Age { Name = "45 to 54" },
            //      new Age { Name = "55 to 64" },
            //      new Age { Name = "65 to 74" },
            //      new Age { Name = "75+" }
            //    );

            //context.Sexes.AddOrUpdate(
            //    s => s.Name,
            //    new Sex { Name = "Male" },
            //    new Sex { Name = "Female" }
            //   );

            //context.Races.AddOrUpdate(
            //     r => r.Name,
            //     new Race { Name = "American Indian or Native American" },
            //     new Race { Name = "Asian or Pacific Islander" },
            //     new Race { Name = "Black or African American" },
            //     new Race { Name = "Hispanic or Latino" },
            //     new Race { Name = "White" },
            //     new Race { Name = "Other" }
            //    );

            //context.Locations.AddOrUpdate(
            //     l => l.Name,
            //     new Location { Name = "City" },
            //     new Location { Name = "Suburb" },
            //     new Location { Name = "Rural" }
            //    );

            //context.Educations.AddOrUpdate(
            //     e => e.Name,
            //     new Education { Name = "No schooling completed" },
            //     new Education { Name = "Some high school, no diploma" },
            //     new Education { Name = "High school degree or equivalent" },
            //     new Education { Name = "Some college, no degree" },
            //     new Education { Name = "Trade/technical/vocational training" },
            //     new Education { Name = "Associate's degree" },
            //     new Education { Name = "Bachelor's degree" },
            //     new Education { Name = "Master's degree" },
            //     new Education { Name = "Doctorate degree" }
            //    );

            //context.Professions.AddOrUpdate(
            //     p => p.Name,
            //     new Profession { Name = "Accounting or finance" },
            //     new Profession { Name = "Admin or clerical" },
            //     new Profession { Name = "Business" },
            //     new Profession { Name = "Customer service" },
            //     new Profession { Name = "Engineering" },
            //     new Profession { Name = "Government" },
            //     new Profession { Name = "Health care" },
            //     new Profession { Name = "Human resources" },
            //     new Profession { Name = "Information technology" },
            //     new Profession { Name = "Manufacturing" },
            //     new Profession { Name = "Sales and marketing" },
            //     new Profession { Name = "Science and biotech" },
            //     new Profession { Name = "Other" }
            //    );

            //context.MaritalStatuses.AddOrUpdate(
            //     m => m.Name,
            //     new MaritalStatus { Name = "Single, never married" },
            //     new MaritalStatus { Name = "Married or in a domestic partnership" },
            //     new MaritalStatus { Name = "Widowed" },
            //     new MaritalStatus { Name = "Divorced" },
            //     new MaritalStatus { Name = "Separated" }
            //    );

            //context.IncomeLevels.AddOrUpdate(
            //     i => i.Name,
            //     new IncomeLevel { Name = "Less than $20,000" },
            //     new IncomeLevel { Name = "$20,000 to $34,999" },
            //     new IncomeLevel { Name = "$35,000 to $49,999" },
            //     new IncomeLevel { Name = "$50,000 to $74,999" },
            //     new IncomeLevel { Name = "$75,000 to $99,999" },
            //     new IncomeLevel { Name = "Over $100,000" }
            //    );

            //context.Categories.AddOrUpdate(
            //     c => c.Name,
            //     new Category { Name = "Logos" },
            //     new Category { Name = "Food" },
            //     new Category { Name = "People" },
            //     new Category { Name = "Locations" },
            //     new Category { Name = "Miscellaneous" }
            //    );
        }
    }
}
