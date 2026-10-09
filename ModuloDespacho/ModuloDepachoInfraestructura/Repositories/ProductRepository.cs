using System.Collections.Generic;
using ModuloDespachoDomain.Models;
using ModuloDespachoDomain.Interfaces;
using System.Configuration;
using Microsoft.Data.SqlClient;

namespace ModuloDepachoInfraestructura.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly string _connectionString;
        public ProductRepository()
        {
            _connectionString = ConfigurationManager.ConnectionStrings["ModuloDespacho.Properties.Settings.NORTHWIND"]?.ConnectionString
                ?? throw new System.InvalidOperationException("Connection string not found");
        }

        public List<Product> GetAll()
        {
            var list = new List<Product>();
            using var cn = new SqlConnection(_connectionString);
            using var cmd = cn.CreateCommand();
            // incluir UnitsInStock para mostrar stock en la UI
            cmd.CommandText = "SELECT ProductID, ProductName, UnitPrice, UnitsInStock FROM Products";
            cn.Open();
            using var r = cmd.ExecuteReader();
            while (r.Read())
            {
                decimal price = 0;
                if (r["UnitPrice"] != DBNull.Value) price = Convert.ToDecimal(r["UnitPrice"]);
                list.Add(new Product
                {
                    ProductId = Convert.ToInt32(r["ProductID"]),
                    ProductName = r["ProductName"] as string ?? string.Empty,
                    UnitPrice = price,
                    UnitsInStock = r["UnitsInStock"] != DBNull.Value ? Convert.ToInt32(r["UnitsInStock"]) : 0
                });
            }
            ProductRepositoryCache.Products = list;
          
            return list;
        }
    }

    
    public static class ProductRepositoryCache
    {
        public static List<Product> Products { get; set; } = new List<Product>();
    }
}
