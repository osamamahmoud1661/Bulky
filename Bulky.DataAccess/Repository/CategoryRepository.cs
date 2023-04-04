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
    public class CategoryRepository : BaseRepository<Category>, ICategoryRepository
    {
        private readonly ApplicationDBContext _db;
        public CategoryRepository(ApplicationDBContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Category entity)
        {
            _db.Categories.Update(entity);
        }
    }
}
