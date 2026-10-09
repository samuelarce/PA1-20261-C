using System.Collections.Generic;
using OrdenesDomain;

namespace OrdenesApplication
{
    public interface IEmpleadoService
    {
        IEnumerable<Empleado> GetAll();
        Empleado GetById(int id);
    }
}
