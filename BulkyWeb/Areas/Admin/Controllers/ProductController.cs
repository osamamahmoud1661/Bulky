
using Bulky.DataAccess.Data;
using Bulky.DataAccess.Repository;
using Bulky.DataAccess.Repository.IRepository;
using Bulky.Models;
using Bulky.Models.VIewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BulkyWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            List<Product> Products = _unitOfWork.product.GetAll().ToList();
            return View(Products);
        }
        [HttpGet]
        public IActionResult Upsert(int? Id)
        {
            IEnumerable<SelectListItem> CaptegoryList = _unitOfWork.category.GetAll().Select(u => new SelectListItem
            {
                Text = u.Name,
                Value = u.Id.ToString()

            });
            ProductVM productVM = new()
            {
                CategoryList = CaptegoryList,
                Product = new Product()
            };
            if (Id == null || Id ==0)
            {
                //create
                return View(productVM);
            }
            else
            {
                //update
                productVM.Product = _unitOfWork.product.Get(u => u.Id==Id);
                return View(productVM);
            }
        }
        [HttpPost]
        public IActionResult Upsert(ProductVM obj , IFormFile? file)
        {
            if (ModelState.IsValid)
            {
                string wwwRootPath = _webHostEnvironment.WebRootPath;
                if (file != null)
                {
                    string FileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    string ProductPath = Path.Combine(wwwRootPath, @"images\prouct");
                    using (var fileStream = new FileStream(Path.Combine(wwwRootPath, FileName), FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }
                    obj.Product.ImageUrl = @"\images\product\" + FileName;

                }
                _unitOfWork.product.Add(obj.Product);
                _unitOfWork.Save();
                TempData["success"] = "Product Created Successfully";
                return RedirectToAction("Index");
            }
            return View(obj);

        }       
        [HttpGet]
        public IActionResult Delete(int Id)
        {
            if (Id == null || Id == 0)
            {
                return NotFound();
            }
            var product = _unitOfWork.product.Get(u => u.Id == Id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }
        [HttpPost]
        public IActionResult Delete(Product obj)
        {

            _unitOfWork.product.Remove(obj);
            _unitOfWork.Save();
            TempData["success"] = "Product Deleted Successfully";
            return RedirectToAction("Index");
        }
    }
}
