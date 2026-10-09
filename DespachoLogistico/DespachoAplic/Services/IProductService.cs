using DespachoDomain.Models;
using System.Collections.Generic;

namespace DespachoAplic.Services
{
    public interface IProductService
    {
        IEnumerable<Product> GetByCategory(int categoryId);
        IEnumerable<Product> SearchByName(string namePart);
    }
}
