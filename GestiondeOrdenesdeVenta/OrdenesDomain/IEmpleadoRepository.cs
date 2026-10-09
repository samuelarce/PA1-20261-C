using System.Collections.Generic;

namespace OrdenesDomain
{
    public interface IEmpleadoRepository
    {
        IEnumerable<Empleado> GetAll();
        Empleado GetById(int id);
    }
}
