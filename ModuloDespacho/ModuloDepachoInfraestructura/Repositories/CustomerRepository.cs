using System.Collections.Generic;
using ModuloDespachoDomain.Models;
using ModuloDespachoDomain.Interfaces;
using System.Configuration;
using Microsoft.Data.SqlClient;

namespace ModuloDepachoInfraestructura.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly string _connectionString;
        public CustomerRepository()
        {
            _connectionString = ConfigurationManager.ConnectionStrings["ModuloDespacho.Properties.Settings.NORTHWIND"]?.ConnectionString
                ?? throw new System.InvalidOperationException("Connection string not found");
        }

        public List<Customer> GetAll()
        {
            var list = new List<Customer>();
            using var cn = new SqlConnection(_connectionString);
            using var cmd = cn.CreateCommand();
            cmd.CommandText = "SELECT CustomerID, CompanyName, City, Phone FROM Customers";
            cn.Open();
            using var r = cmd.ExecuteReader();
            while (r.Read())
            {
                list.Add(new Customer
                {
                    CustomerId = r["CustomerID"] as string ?? string.Empty,
                    CompanyName = r["CompanyName"] as string ?? string.Empty,
                    City = r["City"] as string ?? string.Empty,
                    Phone = r["Phone"] as string ?? string.Empty
                });
            }
            return list;
        }
    }
}
