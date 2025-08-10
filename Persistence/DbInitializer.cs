using Domain;
using Microsoft.AspNetCore.Identity;

namespace Persistence
{
    public class DbInitializer
    {
        public static async Task SeedData(AppDbContext context, UserManager<UserApplication> userManager)
        {
            if (!context.Users.Any())
            {
                var users = new List<UserApplication>() {
                    new UserApplication { FullName="Emad Ismail Mohammed",UserName="emad@gmail.com",Email="emad@gmail.com",Geneder=true, Bio="Software developer, react developer (Fullstack developer)"},
                    new UserApplication { FullName="Farida Emad Ismail ",UserName="farida@gmail.com",Email="farida@gmail.com",Geneder=false, Bio="Kg one  two and half 😂😂❤️❤️"},
                };
                foreach (var user in users)
                {
                    await userManager.CreateAsync(user, "12456");
                }
            }
            if (!context.activities.Any())
            {
                List<Activity> activities = new List<Activity> {
                new Activity {
                    Title = "Past Activity 1",
                    Date = DateTime.Now.AddMonths(-2),
                    Description = "Activity 2 months ago",
                    Category = "drinks",
                    City = "London",
                    Venue = "Pub",
                    InsertionDate = DateTime.Now,
                    InsertionUserID= userManager.Users.First().Id,
                    },
                 new Activity
                {
                    Title = "Past Activity 2",
                    Date = DateTime.Now.AddMonths(-1),
                    Description = "Activity 1 month ago",
                    Category = "culture",
                    City = "Paris",
                    Venue = "Louvre",
                    InsertionDate = DateTime.Now,
                    InsertionUserID= userManager.Users.First().Id,
                },
                new Activity
                {
                    Title = "Future Activity 1",
                    Date = DateTime.Now.AddMonths(1),
                    Description = "Activity 1 month in future",
                    Category = "culture",
                    City = "London",
                    Venue = "Natural History Museum",
                    InsertionDate = DateTime.Now,
                    InsertionUserID= userManager.Users.First().Id,
                },
            };
                await context.activities.AddRangeAsync(activities);
                await context.SaveChangesAsync();
            }
            return;
        }
    }
}
