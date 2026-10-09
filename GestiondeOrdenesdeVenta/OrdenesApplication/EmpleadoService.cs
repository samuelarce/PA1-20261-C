using System.Collections.Generic;
using OrdenesDomain;

namespace OrdenesApplication
{
    public class EmpleadoService : IEmpleadoService
    {
        private readonly IEmpleadoRepository _repo;

        public EmpleadoService(IEmpleadoRepository repo)
        {
            _repo = repo;
        }

        public IEnumerable<Empleado> GetAll()
        {
            return _repo.GetAll();
        }

        public Empleado GetById(int id)
        {
            return _repo.GetById(id);
        }
    }
}
