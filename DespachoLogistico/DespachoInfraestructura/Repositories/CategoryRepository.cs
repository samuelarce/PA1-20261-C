using DespachoDomain;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;

namespace DespachoInfraestructura.Repositories
{
    public class CategoryDto
    {
        public int CategoryID { get; set; }
        public string CategoryName { get; set; } = string.Empty;
    }

    public class CategoryRepository
    {
        private readonly Conexion _conexion = new Conexion();

        public IEnumerable<CategoryDto> GetAll()
        {
            var list = new List<CategoryDto>();
            using var conn = _conexion.CrearConexion();
            if (conn == null) return list;
            conn.Open();

            using var cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT CategoryID, CategoryName FROM Categories ORDER BY CategoryName";

            using var rd = cmd.ExecuteReader();
            while (rd.Read())
            {
                list.Add(new CategoryDto
                {
                    CategoryID = rd.GetInt32(0),
                    CategoryName = rd.IsDBNull(1) ? string.Empty : rd.GetString(1)
                });
            }

            return list;
        }
    }
}
