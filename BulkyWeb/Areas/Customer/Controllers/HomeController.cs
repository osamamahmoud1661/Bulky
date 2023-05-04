using Bulky.DataAccess.Repository.IRepository;
using Bulky.Models;
using Bulky.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;

namespace BulkyBookWeb.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUnitOfWork _unitOfWork;
        public HomeController(ILogger<HomeController> logger , IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            IEnumerable<Product> productlist = _unitOfWork.product.GetAll(includeProperties: "Category");
            return View(productlist);
        }
        [HttpGet]
        public IActionResult Details(int productId)
        {
            ShoppingCart shoppingCart = new()
            {
                Product = _unitOfWork.product.Get(a => a.Id == productId, includeProperties: "Category" ),
                ProductId = productId,
                Count = 1
            };
            return View(shoppingCart);
        }
        [HttpPost]
        [Authorize]
        public IActionResult Details(ShoppingCart shoppingCart)
        {
            var claimsIdenty = (ClaimsIdentity)User.Identity;
            var userId = claimsIdenty.FindFirst(ClaimTypes.NameIdentifier).Value;
            shoppingCart.ApplcationUserId = userId;

            
            var cartFromDB = _unitOfWork.shoppingCart.Get(u=>u.ApplcationUserId== userId && u.ProductId==shoppingCart.ProductId);

            if(cartFromDB != null)
            {
                cartFromDB.Count += shoppingCart.Count;
                _unitOfWork.shoppingCart.Update(cartFromDB);
                _unitOfWork.Save();

            }
            else
            {
                _unitOfWork.shoppingCart.Add(shoppingCart);
                _unitOfWork.Save();
                HttpContext.Session.SetInt32(SD.SessionCart, _unitOfWork.shoppingCart.Get(u => u.ApplcationUserId == userId).Count);
            }
            TempData["success"] = "Cart Updated successfully";

            return RedirectToAction(nameof(Index));
        }



        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}