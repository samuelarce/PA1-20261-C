using System.Collections.Generic;
using ModuloDespachoDomain.Models;
using ModuloDespachoAplic.Services;
using ModuloDepachoInfraestructura.Repositories;

namespace ModuloDespachoAplic.Services
{
    public class CustomerService : ICustomerService
    {
        public List<Customer> GetAll()
        {
            var repo = new CustomerRepository();
            return repo.GetAll();
        }
    }
}
