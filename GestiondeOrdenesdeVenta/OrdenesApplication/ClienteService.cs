using System;
using System.Collections.Generic;
using OrdenesDomain;

namespace OrdenesApplication
{
    public class ClienteService : IClienteService
    {
        private readonly IClienteRepository _repo;

        public ClienteService(IClienteRepository repo)
        {
            _repo = repo;
        }

        public IEnumerable<Cliente> GetAll()
        {
            // Passthrough por ahora; añadir validaciones o transformaciones aquí
            return _repo.GetAll();
        }

        public Cliente GetById(string id)
        {
            return _repo.GetById(id);
        }
    }
}
