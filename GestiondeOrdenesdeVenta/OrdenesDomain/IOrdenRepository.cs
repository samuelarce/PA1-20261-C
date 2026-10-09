using System.Collections.Generic;

namespace OrdenesDomain
{
    public interface IOrdenRepository
    {
        IEnumerable<Orden> GetAll();
        Orden GetById(int id);
        int Add(Orden orden);
        bool Update(Orden orden);
        bool Delete(int id);
    }
}
