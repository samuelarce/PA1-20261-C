using System.Collections.Generic;
using System.Configuration;
using Microsoft.Data.SqlClient;

namespace ModuloDepachoInfraestructura.Repositories
{
    public class TransportType
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        // Porcentaje de flete aplicado sobre el subtotal (ej. 0.10 = 10%)
        public decimal Rate { get; set; } = 0.10m;
    }

    public class TransportRepository
    {
        private readonly string _connectionString;
        public TransportRepository()
        {
            _connectionString = ConfigurationManager.ConnectionStrings["ModuloDespacho.Properties.Settings.NORTHWIND"]?.ConnectionString
                ?? throw new System.InvalidOperationException("Connection string not found");
        }

        public List<TransportType> GetAll()
        {
            var list = new List<TransportType>();
            using var cn = new SqlConnection(_connectionString);
            using var cmd = cn.CreateCommand();
            cmd.CommandText = "SELECT ShipperID, CompanyName FROM Shippers";
            cn.Open();
            using var r = cmd.ExecuteReader();
            while (r.Read())
            {
                var name = r["CompanyName"] as string ?? string.Empty;
                var tt = new TransportType
                {
                    Id = Convert.ToInt32(r["ShipperID"]),
                    Name = name,
                    Rate = 0.10m // tasa por defecto: 10%
                };
                list.Add(tt);
            }
            return list;
        }
    }
}
