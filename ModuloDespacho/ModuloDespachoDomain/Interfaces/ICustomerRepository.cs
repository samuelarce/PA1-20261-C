using System.Collections.Generic;
using ModuloDespachoDomain.Models;

namespace ModuloDespachoDomain.Interfaces
{
    public interface ICustomerRepository
    {
        List<Customer> GetAll();
    }
}
