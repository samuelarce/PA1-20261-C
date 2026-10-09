using System;
using System.Collections.Generic;
using OrdenesDomain;

namespace OrdenesApplication
{
    public class OrdenService : IOrdenService
    {
        private readonly IOrdenRepository _repo;

        public OrdenService(IOrdenRepository repo)
        {
            _repo = repo;
        }

        public IEnumerable<Orden> GetAll()
        {
            return _repo.GetAll();
        }

        public Orden GetById(int id)
        {
            return _repo.GetById(id);
        }

        public OperationResult<int> Create(Orden orden)
        {
            if (orden == null) return OperationResult<int>.Fail("Orden nula");
            if (string.IsNullOrWhiteSpace(orden.CustomerID)) return OperationResult<int>.Fail("Cliente requerido");
            var id = _repo.Add(orden);
            if (id > 0) return OperationResult<int>.Ok(id);
            return OperationResult<int>.Fail("No se pudo crear la orden");
        }

        public OperationResult Update(Orden orden)
        {
            if (orden == null) return OperationResult.Fail("Orden nula");
            var ok = _repo.Update(orden);
            return ok ? OperationResult.Ok() : OperationResult.Fail("No se pudo actualizar");
        }

        public OperationResult Delete(int id)
        {
            var ok = _repo.Delete(id);
            return ok ? OperationResult.Ok() : OperationResult.Fail("No se pudo eliminar");
        }
    }
}
