using System.Collections.Generic;
using OrdenesDomain;

namespace OrdenesApplication
{
    public interface IClienteService
    {
        IEnumerable<Cliente> GetAll();
        Cliente GetById(string id);
    }
}
