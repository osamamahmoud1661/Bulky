
using Bulky.DataAccess.Data;
using Bulky.DataAccess.Repository;
using Bulky.DataAccess.Repository.IRepository;
using Bulky.Models;
using Bulky.Models.VIewModels;
using Bulky.Utility;
using ecommerce.BL.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace BulkyWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class UserController : Controller
    {
        private readonly ApplicationDBContext _db;
        public UserController(ApplicationDBContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            return View();
        }
        
        #region Ajax methods
        [HttpGet]
        public IActionResult GetAll()
        {
            List<ApplicationUser> usersList = _db.applicationUsers.Include(u=>u.Company).ToList();
            var roles = _db.Roles.ToList();
            var userRoles = _db.UserRoles.ToList();
            foreach (var user in usersList)
            {
                var roleId = userRoles.FirstOrDefault(u => u.UserId == user.Id).RoleId;
                user.role = roles.FirstOrDefault(u=>u.Id==roleId).Name;
                if (user.Company==null)
                {
                    user.Company = new() { Name = "" };
                }
            }
            return Json(new { data = usersList });
        }
       
        #endregion
    }
}
