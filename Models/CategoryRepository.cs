using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BethanyPieShop.Models
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly AppDbContext _appDbContext;

        public CategoryRepository(AppDbContext _appDbContext)
        {
            this._appDbContext = _appDbContext;
        }
        public IEnumerable<Category> AllCategories => _appDbContext.Categories;
        
    }
}
