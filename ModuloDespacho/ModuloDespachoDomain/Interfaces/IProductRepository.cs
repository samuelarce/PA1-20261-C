using System.Collections.Generic;
using ModuloDespachoDomain.Models;

namespace ModuloDespachoDomain.Interfaces
{
    public interface IProductRepository
    {
        List<Product> GetAll();
    }
}
