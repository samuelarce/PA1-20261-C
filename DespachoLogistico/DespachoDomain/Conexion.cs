using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;

namespace DespachoDomain
{
    public class Conexion
    {
        public SqlConnection CrearConexion()
        {
            string cadena = null;
            try
            {
                cadena = ConfigurationManager.ConnectionStrings["DespachoLogistico.Properties.Settings.NORTHWIND"].ConnectionString;
                return new SqlConnection(cadena);
            }
            catch (Exception ex)
            {
                // Devolver null y permitir que el consumidor maneje el error.
                System.Diagnostics.Debug.WriteLine($"Error al crear conexión. Cadena='{cadena}' Ex: {ex}");
                return null;
            }
        }
    }
}
