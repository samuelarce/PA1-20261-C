using System.Collections.Generic;
using ModuloDespachoDomain.Models;

namespace ModuloDespachoAplic.Services
{
    public interface ICustomerService
    {
        List<Customer> GetAll();
    }
}
