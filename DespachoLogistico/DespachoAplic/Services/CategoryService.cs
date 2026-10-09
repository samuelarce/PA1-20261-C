using DespachoAplic.Services;
using DespachoDomain.Models;
using DespachoInfraestructura.Repositories;
using System.Collections.Generic;
using System.Linq;

namespace DespachoAplic.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly CategoryRepository _repo = new CategoryRepository();

        public IEnumerable<Category> GetAll()
        {
            var dtos = _repo.GetAll();
            return dtos.Select(d => new Category { CategoryID = d.CategoryID, CategoryName = d.CategoryName });
        }
    }
}
