using Asticom_BackendExam.Context;
using Asticom_BackendExam.Models.DbModel;
using Microsoft.AspNetCore.Identity;
using System.Text.Json;

namespace Asticom_BackendExam.Models
{
    

    public class Seeder
    {
        private readonly AsticomContext _context;
        private readonly UserManager<AdminInfo> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IWebHostEnvironment _hosting;


        public Seeder(AsticomContext context,
            UserManager<AdminInfo> userManager,
            IWebHostEnvironment hosting, 
            RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _hosting = hosting;
            _roleManager = roleManager;
        }

        public async Task SeedAsync()
        {
            _context.Database.EnsureCreated();

            //create role
            bool roleExists = await _roleManager.RoleExistsAsync("Admin");
            if (!roleExists)
            {  
                await _roleManager.CreateAsync(new IdentityRole("Admin"));
            }

            AdminInfo adminAccount = await _userManager.FindByEmailAsync("kevin_admin@sample.com");
            
            //create an admin account to use for first setup
            if(adminAccount == null)
            {
                adminAccount = new AdminInfo()
                {
                    FirstName = "John Kevin",
                    LastName = "Asuncion",
                    Email = "kevin_admin@sample.com",
                    UserName = "kevin_admin@sample.com",
                    
                };

                var result = await _userManager.CreateAsync(adminAccount, "P@ssword!1");
                if(result != IdentityResult.Success)
                {
                    throw new InvalidOperationException("Could not create a new user in seeder");
                }
                //attach the role to the user
                 await _userManager.AddToRoleAsync(adminAccount, "Admin");
            }

            //also add a dummy user to show on initial setup
            if (!_context.UserInfo.Any())
            {
                var file = Path.Combine(_hosting.ContentRootPath, "Models/users.json");
                var json = File.ReadAllText(file);
                var users = JsonSerializer.Deserialize<IEnumerable<UserInfo>>(json);
                
                _context.UserInfo.AddRange(users);
                _context.SaveChanges();
            }
        }
    }
}
