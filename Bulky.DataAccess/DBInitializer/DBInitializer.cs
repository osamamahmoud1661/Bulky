using Bulky.DataAccess.Data;
using Bulky.Models;
using Bulky.Utility;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bulky.DataAccess.DBInitializer
{
    public class DBInitializer : IDBInitializer
    {
        private readonly ApplicationDBContext _db;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public DBInitializer(ApplicationDBContext db,UserManager<IdentityUser> userManager , RoleManager<IdentityRole> roleManager)
        {
            _db = db;
            _userManager = userManager;
            _roleManager = roleManager;
        }
        public void Initialize()
        {
            //migration if they not applied
            try
            {
                if (_db.Database.GetPendingMigrations().Count() > 0)
                {
                    _db.Database.Migrate();
                }
            }
            catch (Exception ex)
            {

                throw;
            }
            //Create Roles if they not Created
            if (!_roleManager.RoleExistsAsync(SD.Role_Customer).GetAwaiter().GetResult())
            {
                _roleManager.CreateAsync(new IdentityRole(SD.Role_Customer)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(SD.Role_Admin)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(SD.Role_Company)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(SD.Role_Employee)).GetAwaiter().GetResult();
                //if roles not created , then we will create admin user as well

                _userManager.CreateAsync(new ApplicationUser
                {
                    UserName = "Admin@gmail.com",
                    Email = "Admin@gmail.com",
                    Name = "Admin",
                    PhoneNumber = "1234567890",
                    StreatAddress = "asd",
                    City = "test",
                    State = "test",
                    PostCode = "565"


                }, "Aa@123321").GetAwaiter().GetResult();
                ApplicationUser user = _db.applicationUsers.FirstOrDefault(u=>u.Email == "Admin@gmail.com");
                _userManager.AddToRoleAsync(user, SD.Role_Admin).GetAwaiter().GetResult();
            }
            return;
        }
    }
}
