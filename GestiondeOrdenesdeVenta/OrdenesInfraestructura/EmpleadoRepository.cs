using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using OrdenesDomain;

namespace OrdenesInfraestructura
{
    public class EmpleadoRepository : OrdenesDomain.IEmpleadoRepository
    {
        private readonly Conexion _conexion = new Conexion();

        public IEnumerable<Empleado> GetAll()
        {
            var lista = new List<Empleado>();
            try
            {
                using var conn = _conexion.CrearConexion();
                conn.Open();
                using var cmd = new SqlCommand("SELECT EmployeeID, FirstName + ' ' + LastName AS FullName FROM Employees ORDER BY LastName", conn);
                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    lista.Add(new Empleado { Id = reader.GetInt32(0), Nombre = reader.GetString(1) });
                }
            }
            catch (Exception)
            {
                // manejar/loggear según convenga
            }
            return lista;
        }

        public Empleado GetById(int id)
        {
            try
            {
                using var conn = _conexion.CrearConexion();
                conn.Open();
                using var cmd = new SqlCommand("SELECT EmployeeID, FirstName + ' ' + LastName AS FullName FROM Employees WHERE EmployeeID = @id", conn);
                cmd.Parameters.AddWithValue("@id", id);
                using var reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    return new Empleado { Id = reader.GetInt32(0), Nombre = reader.GetString(1) };
                }
            }
            catch (Exception)
            {
            }
            return null;
        }
    }
}
