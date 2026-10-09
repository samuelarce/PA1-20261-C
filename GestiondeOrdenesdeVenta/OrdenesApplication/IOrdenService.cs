using System.Collections.Generic;
using OrdenesDomain;

namespace OrdenesApplication
{
    public interface IOrdenService
    {
        IEnumerable<Orden> GetAll();
        Orden GetById(int id);
        OperationResult<int> Create(Orden orden);
        OperationResult Update(Orden orden);
        OperationResult Delete(int id);
    }
}
