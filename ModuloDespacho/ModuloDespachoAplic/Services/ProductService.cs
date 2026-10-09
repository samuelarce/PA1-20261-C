using System.Collections.Generic;
using ModuloDespachoDomain.Models;
using ModuloDespachoAplic.Services;
using ModuloDepachoInfraestructura.Repositories;

namespace ModuloDespachoAplic.Services
{
    public class ProductService : IProductService
    {
        public List<Product> GetAll()
        {
            var repo = new ProductRepository();
            return repo.GetAll();
        }
    }
}
