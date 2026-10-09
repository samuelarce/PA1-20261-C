using System.Collections.Generic;
using System.Configuration;
using Microsoft.Data.SqlClient;
using System;

namespace ModuloDepachoInfraestructura.Repositories
{
    public class OrderItemDto
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
        public decimal Discount { get; set; } 
    }

    public class OrdersRepository
    {
        private readonly string _connectionString;
        public OrdersRepository()
        {
            var cs = ConfigurationManager.ConnectionStrings["ModuloDespacho.Properties.Settings.NORTHWIND"];
            if (cs == null) throw new InvalidOperationException("Connection string not found");
            _connectionString = cs.ConnectionString;
        }

        public List<OrderItemDto> GetLastOrderDetails(string customerId)
        {
            var result = new List<OrderItemDto>();
            using var cn = new SqlConnection(_connectionString);
            using var cmd = cn.CreateCommand();
            cmd.CommandText = "SELECT TOP(1) OrderID FROM Orders WHERE CustomerID = @cid AND OrderDate IS NOT NULL ORDER BY OrderDate DESC";
            cmd.Parameters.AddWithValue("@cid", customerId);
            cn.Open();
            object? orderIdObj = cmd.ExecuteScalar();
            if (orderIdObj == null || orderIdObj == DBNull.Value) return result;
            var orderId = Convert.ToInt32(orderIdObj);

            using var cmd2 = cn.CreateCommand();
            cmd2.CommandText = @"SELECT od.ProductID, p.ProductName, od.UnitPrice, od.Quantity, od.Discount
FROM [Order Details] od
JOIN Products p ON od.ProductID = p.ProductID
WHERE od.OrderID = @oid";
            cmd2.Parameters.AddWithValue("@oid", orderId);
            using var r = cmd2.ExecuteReader();
            while (r.Read())
            {
                decimal price = 0;
                if (r["UnitPrice"] != DBNull.Value) price = Convert.ToDecimal(r["UnitPrice"]);
                int qty = r["Quantity"] != DBNull.Value ? Convert.ToInt32(r["Quantity"]) : 0;
                decimal disc = r["Discount"] != DBNull.Value ? Convert.ToDecimal(r["Discount"]) : 0m;
                result.Add(new OrderItemDto
                {
                    ProductId = Convert.ToInt32(r["ProductID"]),
                    ProductName = r["ProductName"] as string ?? string.Empty,
                    UnitPrice = price,
                    Quantity = qty,
                    Discount = disc
                });
            }
            return result;
        }
    }
}
