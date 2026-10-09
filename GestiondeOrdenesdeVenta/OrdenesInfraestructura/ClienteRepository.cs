using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using OrdenesDomain;

namespace OrdenesInfraestructura
{
    public class ClienteRepository : OrdenesDomain.IClienteRepository
    {
        private readonly Conexion _conexion = new Conexion();

        public IEnumerable<Cliente> GetAll()
        {
            var lista = new List<Cliente>();
            try
            {
                using var conn = _conexion.CrearConexion();
                conn.Open();
                using var cmd = new SqlCommand("SELECT CustomerID, CompanyName FROM Customers ORDER BY CompanyName", conn);
                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    lista.Add(new Cliente { Id = reader.GetString(0), Nombre = reader.GetString(1) });
                }
            }
            catch (Exception)
            {
                // manejar/loggear según convenga
            }
            return lista;
        }

        public Cliente GetById(string id)
        {
            try
            {
                using var conn = _conexion.CrearConexion();
                conn.Open();
                using var cmd = new SqlCommand("SELECT CustomerID, CompanyName FROM Customers WHERE CustomerID = @id", conn);
                cmd.Parameters.AddWithValue("@id", id);
                using var reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    return new Cliente { Id = reader.GetString(0), Nombre = reader.GetString(1) };
                }
            }
            catch (Exception)
            {
            }
            return null;
        }
    }
}
