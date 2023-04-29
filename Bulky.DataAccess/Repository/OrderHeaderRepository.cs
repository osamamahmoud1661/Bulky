using Bulky.DataAccess.Data;
using Bulky.DataAccess.Repository.IRepository;
using Bulky.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bulky.DataAccess.Repository
{
    public class OrderHeaderRepository : BaseRepository<OrderHeader>, IOrderHeaderRepository
    {
        private readonly ApplicationDBContext _db;
        public OrderHeaderRepository(ApplicationDBContext db) : base(db)
        {
            _db = db;
        }

        public void Update(OrderHeader obj)
        {
            _db.OrderHeaders.Update(obj);
        }

        public void UpdateStatus(int Id, string orderStatus, string? paymentStatus = null)
        {
            var orderFromDB = _db.OrderHeaders.Where(u=>u.Id==Id).FirstOrDefault();
            if (orderFromDB != null)
            {
                orderFromDB.OrderStatus = orderStatus;
            }
            if (!string.IsNullOrEmpty(paymentStatus))
            {
                orderFromDB.PaymentStatus = paymentStatus;
            }
        }

        public void UpdateStripePaymentID(int Id, string SessionId, string PaymentIntentId)
        {
            var orderFromDB = _db.OrderHeaders.Where(u => u.Id == Id).FirstOrDefault();
            if (!string.IsNullOrEmpty(SessionId))
            {
                orderFromDB.SessionId = SessionId;
            }
            if (!string.IsNullOrEmpty(PaymentIntentId))
            {
                orderFromDB.PaymentIntentId = PaymentIntentId;
                orderFromDB.PaymentDate = DateTime.Now;
            }
        }
    }
}
