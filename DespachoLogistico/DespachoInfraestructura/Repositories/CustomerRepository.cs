using DespachoDomain;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;

namespace DespachoInfraestructura.Repositories
{
    public class CustomerDto
    {
        public string CustomerID { get; set; } = string.Empty;
        public string CompanyName { get; set; } = string.Empty;
    }

    public class CustomerRepository
    {
        private readonly Conexion _conexion = new Conexion();

        public IEnumerable<CustomerDto> GetAll()
        {
            var list = new List<CustomerDto>();
            using var conn = _conexion.CrearConexion();
            if (conn == null) return list;
            conn.Open();

            using var cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT CustomerID, CompanyName FROM Customers ORDER BY CompanyName";

            using var rd = cmd.ExecuteReader();
            while (rd.Read())
            {
                list.Add(new CustomerDto
                {
                    CustomerID = rd.IsDBNull(0) ? string.Empty : rd.GetString(0),
                    CompanyName = rd.IsDBNull(1) ? string.Empty : rd.GetString(1)
                });
            }

            return list;
        }
    }
}
