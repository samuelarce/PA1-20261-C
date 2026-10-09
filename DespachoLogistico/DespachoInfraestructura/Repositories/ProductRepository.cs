using DespachoDomain;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;

namespace DespachoInfraestructura.Repositories
{
    public class ProductDto
    {
        public int ProductID { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public int CategoryID { get; set; }
        public decimal UnitPrice { get; set; }
        public int UnitsInStock { get; set; }
    }

    public class ProductRepository
    {
        private readonly Conexion _conexion = new Conexion();

        public IEnumerable<ProductDto> GetProductsByCategory(int categoryId)
        {
            var list = new List<ProductDto>();
            using var conn = _conexion.CrearConexion();
            if (conn == null) return list;
            conn.Open();

            using var cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT ProductID, ProductName, CategoryID, UnitPrice, UnitsInStock FROM Products WHERE CategoryID = @cat";
            cmd.Parameters.AddWithValue("@cat", categoryId);

            using var rd = cmd.ExecuteReader();
            while (rd.Read())
            {
                list.Add(new ProductDto
                {
                    ProductID = rd.GetInt32(0),
                    ProductName = rd.IsDBNull(1) ? string.Empty : rd.GetString(1),
                    CategoryID = rd.IsDBNull(2) ? 0 : rd.GetInt32(2),
                    UnitPrice = rd.IsDBNull(3) ? 0 : rd.GetDecimal(3),
                    UnitsInStock = rd.IsDBNull(4) ? 0 : rd.GetInt16(4)
                });
            }

            return list;
        }

        public IEnumerable<ProductDto> SearchProductsByName(string namePart)
        {
            var list = new List<ProductDto>();
            using var conn = _conexion.CrearConexion();
            if (conn == null) return list;
            conn.Open();

            using var cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT ProductID, ProductName, CategoryID, UnitPrice, UnitsInStock FROM Products WHERE ProductName LIKE @name";
            cmd.Parameters.AddWithValue("@name", "%" + namePart + "%");

            using var rd = cmd.ExecuteReader();
            while (rd.Read())
            {
                list.Add(new ProductDto
                {
                    ProductID = rd.GetInt32(0),
                    ProductName = rd.IsDBNull(1) ? string.Empty : rd.GetString(1),
                    CategoryID = rd.IsDBNull(2) ? 0 : rd.GetInt32(2),
                    UnitPrice = rd.IsDBNull(3) ? 0 : rd.GetDecimal(3),
                    UnitsInStock = rd.IsDBNull(4) ? 0 : rd.GetInt16(4)
                });
            }

            return list;
        }
    }
}
