using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using OrdenesDomain;

namespace OrdenesInfraestructura
{
    public class OrdenRepository : OrdenesDomain.IOrdenRepository
    {
        private readonly Conexion _conexion = new Conexion();

        public IEnumerable<Orden> GetAll()
        {
            var lista = new List<Orden>();
            try
            {
                using var conn = _conexion.CrearConexion();
                conn.Open();
                using var cmd = new SqlCommand(@"
SELECT o.OrderID, o.CustomerID, c.CompanyName, o.EmployeeID, (e.FirstName + ' ' + e.LastName) AS EmployeeName,
       o.ShipVia, o.OrderDate, o.Freight, o.ShipName, o.ShipCity
FROM Orders o
LEFT JOIN Customers c ON o.CustomerID = c.CustomerID
LEFT JOIN Employees e ON o.EmployeeID = e.EmployeeID
ORDER BY o.OrderID", conn);
                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    var orden = new Orden
                    {
                        ID = reader.GetInt32(0),
                        CustomerID = reader.IsDBNull(1) ? null : reader.GetString(1),
                        // CompanyName is at index 2
                        // EmployeeID is at index 3
                        EmployeeID = reader.IsDBNull(3) ? null : reader.GetInt32(3),
                        ShipVia = reader.IsDBNull(5) ? null : reader.GetInt32(5),
                        OrderDate = reader.IsDBNull(6) ? null : reader.GetDateTime(6),
                        Freight = reader.IsDBNull(7) ? 0 : Convert.ToDouble(reader.GetDecimal(7)),
                        Destino = reader.IsDBNull(8) ? string.Empty : reader.GetString(8),
                        Ciudad = reader.IsDBNull(9) ? string.Empty : reader.GetString(9)
                    };
                    // Display fields: use joined names when available
                    orden.Cliente = reader.IsDBNull(2) ? orden.CustomerID : reader.GetString(2);
                    orden.Empleado = reader.IsDBNull(4) ? (orden.EmployeeID?.ToString()) : reader.GetString(4);
                    orden.Fecha = orden.OrderDate?.ToShortDateString();
                    lista.Add(orden);
                }
            }
            catch (Exception ex)
            {
                // Propagar la excepción para que la capa superior pueda mostrarla y facilitar depuración
                throw new Exception("OrdenRepository.GetAll fallo: " + ex.Message, ex);
            }
            return lista;
        }

        public Orden GetById(int id)
        {
            try
            {
                using var conn = _conexion.CrearConexion();
                conn.Open();
                using var cmd = new SqlCommand(@"
SELECT o.OrderID, o.CustomerID, c.CompanyName, o.EmployeeID, (e.FirstName + ' ' + e.LastName) AS EmployeeName,
       o.ShipVia, o.OrderDate, o.Freight, o.ShipName, o.ShipCity
FROM Orders o
LEFT JOIN Customers c ON o.CustomerID = c.CustomerID
LEFT JOIN Employees e ON o.EmployeeID = e.EmployeeID
WHERE o.OrderID = @id", conn);
                cmd.Parameters.AddWithValue("@id", id);
                using var reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    var orden = new Orden
                    {
                        ID = reader.GetInt32(0),
                        CustomerID = reader.IsDBNull(1) ? null : reader.GetString(1),
                        // CompanyName is at index 2
                        EmployeeID = reader.IsDBNull(3) ? null : reader.GetInt32(3),
                        ShipVia = reader.IsDBNull(5) ? null : reader.GetInt32(5),
                        OrderDate = reader.IsDBNull(6) ? null : reader.GetDateTime(6),
                        Freight = reader.IsDBNull(7) ? 0 : Convert.ToDouble(reader.GetDecimal(7)),
                        Destino = reader.IsDBNull(8) ? string.Empty : reader.GetString(8),
                        Ciudad = reader.IsDBNull(9) ? string.Empty : reader.GetString(9)
                    };
                    orden.Cliente = reader.IsDBNull(2) ? orden.CustomerID : reader.GetString(2);
                    orden.Empleado = reader.IsDBNull(4) ? (orden.EmployeeID?.ToString()) : reader.GetString(4);
                    orden.Fecha = orden.OrderDate?.ToShortDateString();
                    return orden;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("OrdenRepository.GetById fallo: " + ex.Message, ex);
            }
            return null;
        }

        public int Add(Orden orden)
        {
            try
            {
                using var conn = _conexion.CrearConexion();
                conn.Open();
                using var cmd = new SqlCommand(@"INSERT INTO Orders (CustomerID, EmployeeID, OrderDate, Freight, ShipName, ShipCity, ShipVia)
VALUES (@customer, @employee, @orderDate, @freight, @shipName, @shipCity, @shipVia);
SELECT SCOPE_IDENTITY();", conn);
                cmd.Parameters.AddWithValue("@customer", (object)orden.CustomerID ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@employee", (object)orden.EmployeeID ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@orderDate", (object)orden.OrderDate ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@freight", orden.Freight);
                cmd.Parameters.AddWithValue("@shipName", (object)orden.Destino ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@shipCity", (object)orden.Ciudad ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@shipVia", (object)orden.ShipVia ?? DBNull.Value);
                var result = cmd.ExecuteScalar();
                if (result != null)
                {
                    // SCOPE_IDENTITY devuelve un tipo numérico; convertir a int de forma segura
                    return Convert.ToInt32(result);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("OrdenRepository.Add fallo: " + ex.Message, ex);
            }
            return 0;
        }

        public bool Update(Orden orden)
        {
            try
            {
                using var conn = _conexion.CrearConexion();
                conn.Open();
                using var cmd = new SqlCommand(@"UPDATE Orders SET CustomerID=@customer, EmployeeID=@employee, OrderDate=@orderDate, Freight=@freight, ShipName=@shipName, ShipCity=@shipCity, ShipVia=@shipVia WHERE OrderID=@id", conn);
                cmd.Parameters.AddWithValue("@customer", (object)orden.CustomerID ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@employee", (object)orden.EmployeeID ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@orderDate", (object)orden.OrderDate ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@freight", orden.Freight);
                cmd.Parameters.AddWithValue("@shipName", (object)orden.Destino ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@shipCity", (object)orden.Ciudad ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@shipVia", (object)orden.ShipVia ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@id", orden.ID);
                var rows = cmd.ExecuteNonQuery();
                return rows > 0;
            }
            catch (Exception ex)
            {
                throw new Exception("OrdenRepository.Update fallo: " + ex.Message, ex);
            }
            return false;
        }

        public bool Delete(int id)
        {
            try
            {
                using var conn = _conexion.CrearConexion();
                conn.Open();
                using var cmd = new SqlCommand("DELETE FROM Orders WHERE OrderID = @id", conn);
                cmd.Parameters.AddWithValue("@id", id);
                var rows = cmd.ExecuteNonQuery();
                return rows > 0;
            }
            catch (Exception ex)
            {
                throw new Exception("OrdenRepository.Delete fallo: " + ex.Message, ex);
            }
            return false;
        }
    }
}
