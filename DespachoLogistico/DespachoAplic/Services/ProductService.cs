using DespachoAplic.Services;
using DespachoDomain.Models;
using DespachoInfraestructura.Repositories;
using System.Collections.Generic;
using System.Linq;

namespace DespachoAplic.Services
{
    public class ProductService : IProductService
    {
        private readonly ProductRepository _repo = new ProductRepository();

        public IEnumerable<Product> GetByCategory(int categoryId)
        {
            var dtos = _repo.GetProductsByCategory(categoryId);
            return dtos.Select(d => new Product
            {
                ProductID = d.ProductID,
                ProductName = d.ProductName,
                CategoryID = d.CategoryID,
                UnitPrice = d.UnitPrice,
                UnitsInStock = d.UnitsInStock
            });
        }

        public IEnumerable<Product> SearchByName(string namePart)
        {
            var dtos = _repo.SearchProductsByName(namePart);
            return dtos.Select(d => new Product
            {
                ProductID = d.ProductID,
                ProductName = d.ProductName,
                CategoryID = d.CategoryID,
                UnitPrice = d.UnitPrice,
                UnitsInStock = d.UnitsInStock
            });
        }
    }
}
