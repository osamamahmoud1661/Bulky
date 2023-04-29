using Bulky.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bulky.DataAccess.Repository.IRepository
{
    public interface IOrderHeaderRepository : IBaseRepository<OrderHeader>
    {
        void Update(OrderHeader obj);
        void UpdateStatus(int Id ,string orderStatus,string? paymentStatus = null);
        void UpdateStripePaymentID(int Id, string SessionId, string PaymentIntent );
    }
}
