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
    public class CompanyRepository : BaseRepository<Company>, ICompanyRepository
    {
        private readonly ApplicationDBContext _db;

        public CompanyRepository(ApplicationDBContext db) : base(db)
        {
            this._db = db;
        }
        public void Update(Company entity)
        {
            _db.Companies.Update(entity);
        }
    }
}
