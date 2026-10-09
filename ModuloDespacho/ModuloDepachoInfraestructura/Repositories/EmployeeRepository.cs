using System.Collections.Generic;
using ModuloDespachoDomain.Models;
using System.Configuration;
using Microsoft.Data.SqlClient;

namespace ModuloDepachoInfraestructura.Repositories
{
    public class EmployeeRepository
    {
        private readonly string _connectionString;
        public EmployeeRepository()
        {
            _connectionString = ConfigurationManager.ConnectionStrings["ModuloDespacho.Properties.Settings.NORTHWIND"]?.ConnectionString
                ?? throw new System.InvalidOperationException("Connection string not found");
        }

        public List<Employee> GetAll()
        {
            var list = new List<Employee>();
            using var cn = new SqlConnection(_connectionString);
            using var cmd = cn.CreateCommand();
            cmd.CommandText = "SELECT EmployeeID, FirstName, LastName FROM Employees";
            cn.Open();
            using var r = cmd.ExecuteReader();
            while (r.Read())
            {
                list.Add(new Employee
                {
                    EmployeeId = Convert.ToInt32(r["EmployeeID"]),
                    FirstName = r["FirstName"] as string ?? string.Empty,
                    LastName = r["LastName"] as string ?? string.Empty
                });
            }
            return list;
        }
    }
}
