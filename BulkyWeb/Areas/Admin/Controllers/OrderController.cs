using Bulky.DataAccess.Repository.IRepository;
using Bulky.Models;
using Bulky.Models.VIewModels;
using Bulky.Utility;
using ecommerce.BL.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;

namespace BulkyWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class OrderController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public OrderController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            return View();
        } 
        public IActionResult Details(int orderId)
        {
            OrderVM orderVM = new()
            {
                OrderHeader = _unitOfWork.OrderHeader.Get(u => u.Id == orderId,includeProperties:"applicationUser"),
                OrderDetail = _unitOfWork.OrderDetail.GetAll(u => u.OrderHeaderId == orderId, includeProperties: "Product")
            };  
            return View(orderVM);
        }
        [HttpPost]
        [Authorize(Roles =SD.Role_Admin+","+SD.Role_Employee)]
        public IActionResult UpdateOrderDetails(OrderVM orderVM)
        {
            var oldOrderHeaderFromDB = _unitOfWork.OrderHeader.Get(u => u.Id == orderVM.OrderHeader.Id);
            oldOrderHeaderFromDB.Name = orderVM.OrderHeader.Name;
            oldOrderHeaderFromDB.PhoneNumber = orderVM.OrderHeader.PhoneNumber;
            oldOrderHeaderFromDB.City = orderVM.OrderHeader.City;
            oldOrderHeaderFromDB.State = orderVM.OrderHeader.State;
            oldOrderHeaderFromDB.PostalCode = orderVM.OrderHeader.PostalCode;
            oldOrderHeaderFromDB.StreetAddress = orderVM.OrderHeader.StreetAddress;
            if (!string.IsNullOrEmpty(orderVM.OrderHeader.Carrier))
            {
                oldOrderHeaderFromDB.Carrier = orderVM.OrderHeader.Carrier;
            }
            if (!string.IsNullOrEmpty(orderVM.OrderHeader.TrackingNumber))
            {
                oldOrderHeaderFromDB.TrackingNumber = orderVM.OrderHeader.TrackingNumber;
            }
            _unitOfWork.OrderHeader.Update(oldOrderHeaderFromDB);
            _unitOfWork.Save();
            TempData["success"] = "Order Details Updated successfully";
            return RedirectToAction(nameof(Details), new { orderId = oldOrderHeaderFromDB.Id});
        }


        #region Ajax methods
        [HttpGet]
        public IActionResult GetAll(string status)
        {
            IEnumerable<OrderHeader> objOrderHeaders;

            if (User.IsInRole(SD.Role_Admin)||User.IsInRole(SD.Role_Employee))
            {
                objOrderHeaders = _unitOfWork.OrderHeader.GetAll(includeProperties: "applicationUser").ToList();
            }
            else
            {
                var claimsIdentity = (ClaimsIdentity)User.Identity;
                var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

                objOrderHeaders = _unitOfWork.OrderHeader.GetAll(u=>u.ApplicationUserId==userId,includeProperties: "applicationUser").ToList();
            }
            
            switch (status)
            {
                case "pending":
                    objOrderHeaders = objOrderHeaders.Where(u => u.PaymentStatus == SD.PaymentStatusDelayedPayment);
                    break;
                case "inprocess":
                    objOrderHeaders = objOrderHeaders.Where(u => u.OrderStatus == SD.StatusInProcess);
                    break;
                case "completed":
                    objOrderHeaders = objOrderHeaders.Where(u => u.OrderStatus == SD.StatusShipped);
                    break;
                case "approved":
                    objOrderHeaders = objOrderHeaders.Where(u => u.OrderStatus == SD.StatusApproved);
                    break;
                default:
                    break;

            }

            return Json(new { data = objOrderHeaders });
        }
        
        #endregion
    }
}
