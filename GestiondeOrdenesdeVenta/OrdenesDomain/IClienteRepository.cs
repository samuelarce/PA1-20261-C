using System.Collections.Generic;

namespace OrdenesDomain
{
    public interface IClienteRepository
    {
        IEnumerable<Cliente> GetAll();
        Cliente GetById(string id);
    }
}
