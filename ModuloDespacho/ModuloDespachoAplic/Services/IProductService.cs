using System.Collections.Generic;
using ModuloDespachoDomain.Models;

namespace ModuloDespachoAplic.Services
{
    public interface IProductService
    {
        List<Product> GetAll();
    }
}
