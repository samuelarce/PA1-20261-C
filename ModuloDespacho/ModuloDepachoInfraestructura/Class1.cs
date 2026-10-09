using System;
using System.Collections.Generic;
using System.Configuration;
using Microsoft.Data.SqlClient;

namespace ModuloDepachoInfraestructura
{
    
    public class CustomerDto
    {
        public string CustomerId { get; set; } = string.Empty;
        public string CompanyName { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
    }

    
    public class ProductDto
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public decimal UnitPrice { get; set; }
    }

    public class NorthwindRepository
    {
        private readonly string _connectionString;

        public NorthwindRepository()
        {
            var cs = ConfigurationManager.ConnectionStrings["ModuloDespacho.Properties.Settings.NORTHWIND"];
            if (cs == null)
                throw new InvalidOperationException("Connection string 'ModuloDespacho.Properties.Settings.NORTHWIND' not found in config.");
            _connectionString = cs.ConnectionString;
        }

        public List<CustomerDto> GetCustomers()
        {
            var list = new List<CustomerDto>();
            using (var cn = new SqlConnection(_connectionString))
            using (var cmd = cn.CreateCommand())
            {
                cmd.CommandText = "SELECT CustomerID, CompanyName, City, Phone FROM Customers";
                cn.Open();
                using (var r = cmd.ExecuteReader())
                {
                    while (r.Read())
                    {
                        list.Add(new CustomerDto
                        {
                            CustomerId = r["CustomerID"] as string ?? string.Empty,
                            CompanyName = r["CompanyName"] as string ?? string.Empty,
                            City = r["City"] as string ?? string.Empty,
                            Phone = r["Phone"] as string ?? string.Empty
                        });
                    }
                }
            }
            return list;
        }

        public List<ProductDto> GetProducts()
        {
            var list = new List<ProductDto>();
            using (var cn = new SqlConnection(_connectionString))
            using (var cmd = cn.CreateCommand())
            {
                cmd.CommandText = "SELECT ProductID, ProductName, UnitPrice FROM Products";
                cn.Open();
                using (var r = cmd.ExecuteReader())
                {
                    while (r.Read())
                    {
                        decimal price = 0;
                        if (r["UnitPrice"] != DBNull.Value)
                            price = Convert.ToDecimal(r["UnitPrice"]);

                        list.Add(new ProductDto
                        {
                            ProductId = Convert.ToInt32(r["ProductID"]),
                            ProductName = r["ProductName"] as string ?? string.Empty,
                            UnitPrice = price
                        });
                    }
                }
            }
            return list;
        }
    }
}
