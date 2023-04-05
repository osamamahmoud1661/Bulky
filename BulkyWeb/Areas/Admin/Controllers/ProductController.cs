
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
using System.Data;

namespace BulkyWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
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
            var s = new string[5];
            List<Product> Products = _unitOfWork.product.GetAll(new[] {"Category"} ).ToList();
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
                
                if (file != null)
                {
                    if (!string.IsNullOrEmpty(obj.Product.ImageUrl))
                    {
                        FileUploader.DeleteFile("/wwwroot/Images/product/", obj.Product.ImageUrl);
                    }
                    obj.Product.ImageUrl= FileUploader.UploadFile("/wwwroot/Images/product", file);
                }
                if (obj.Product.Id==0)
                {
                    _unitOfWork.product.Add(obj.Product);
                    TempData["success"] = "Product Created Successfully";

                }
                else
                {
                    _unitOfWork.product.Update(obj.Product);
                    TempData["success"] = "Product Updated Successfully";
                }
                _unitOfWork.Save();
             
                return RedirectToAction("Index");
            }
            return View(obj);

        }       
        //[HttpGet]
        //public IActionResult Delete(int Id)
        //{
        //    if (Id == null || Id == 0)
        //    {
        //        return NotFound();
        //    }
        //    var product = _unitOfWork.product.Get(u => u.Id == Id);
        //    if (product == null)
        //    {
        //        return NotFound();
        //    }
        //    return View(product);
        //}
        //[HttpPost]
        //public IActionResult Delete(Product obj)
        //{
        //    FileUploader.DeleteFile("/wwwroot/Images/product/", obj.ImageUrl);
        //    _unitOfWork.product.Remove(obj);
        //    _unitOfWork.Save();
        //    TempData["success"] = "Product Deleted Successfully";
        //    return RedirectToAction("Index");
        //}
        #region Ajax methods
        [HttpGet]
        public IActionResult GetAll()
        {
            List<Product> Products = _unitOfWork.product.GetAll(new[] { "Category" }).ToList();
            return Json(new { data = Products });
        }
        [HttpDelete]
        public IActionResult Delete(int? Id)
        {
            var product = _unitOfWork.product.Get(u => u.Id == Id);
            if (product==null)
            {
                return Json(new { success = "Error while deleting product!" });
            }
            FileUploader.DeleteFile("/wwwroot/Images/product/", product.ImageUrl);
            _unitOfWork.product.Remove(product);
            _unitOfWork.Save();
            TempData["success"] = "Product Deleted Successfully";
            return Json(new { success = "Deleted Successfuly!" });

        }
        #endregion
    }
}
