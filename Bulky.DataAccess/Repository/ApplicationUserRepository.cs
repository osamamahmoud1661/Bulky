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
    public class ApplicationUserRepository : BaseRepository<ApplicationUser> , IApplicationUserRepository
    {
        public ApplicationUserRepository(ApplicationDBContext db) : base(db)
        {
            
        }
    }
}
