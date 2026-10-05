using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;
using Microsoft.Data.SqlClient;

namespace OrdenesInfraestructura
{
    internal class Conexion
    {
        public SqlConnection CrearConexion()
        {
            try
            {
                string cadena = ConfigurationManager.ConnectionStrings["GestiondeOrdenesdeVenta.Properties.Settings.Northwind"].ConnectionString;

                return new SqlConnection(cadena);
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
