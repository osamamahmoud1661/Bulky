
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
    //[Authorize(Roles = SD.Role_Admin)]
    public class CompanyController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public CompanyController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            var s = new string[5];
            List<Company> Companys = _unitOfWork.company.GetAll().ToList();
            return View(Companys);
        }
        [HttpGet]
        public IActionResult Upsert(int? Id)
        {
            
            if (Id == null || Id ==0)
            {
                //create
                return View(new Company());
            }
            else
            {
                //update
                Company company = _unitOfWork.company.Get(u => u.Id==Id);
                return View(company);
            }
        }
        [HttpPost]
        public IActionResult Upsert(Company obj)
        {
            if (ModelState.IsValid)
            {

            
                if (obj.Id==0)
                {
                    _unitOfWork.company.Add(obj);
                    TempData["success"] = "Company Created Successfully";

                }
                else
                {
                    _unitOfWork.company.Update(obj);
                    TempData["success"] = "Company Updated Successfully";
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
        //    var Company = _unitOfWork.Company.Get(u => u.Id == Id);
        //    if (Company == null)
        //    {
        //        return NotFound();
        //    }
        //    return View(Company);
        //}
        //[HttpPost]
        //public IActionResult Delete(Company obj)
        //{
        //    FileUploader.DeleteFile("/wwwroot/Images/Company/", obj.ImageUrl);
        //    _unitOfWork.Company.Remove(obj);
        //    _unitOfWork.Save();
        //    TempData["success"] = "Company Deleted Successfully";
        //    return RedirectToAction("Index");
        //}
        #region Ajax methods
        [HttpGet]
        public IActionResult GetAll()
        {
            List<Company> Companys = _unitOfWork.company.GetAll().ToList();
            return Json(new { data = Companys });
        }
        [HttpDelete]
        public IActionResult Delete(int? Id)
        {
            var Company = _unitOfWork.company.Get(u => u.Id == Id);
            if (Company==null)
            {
                return Json(new { success = "Error while deleting Company!" });
            }
            
            _unitOfWork.company.Remove(Company);
            _unitOfWork.Save();
            TempData["success"] = "Company Deleted Successfully";
            return Json(new { success = "Deleted Successfuly!" });

        }
        #endregion
    }
}
