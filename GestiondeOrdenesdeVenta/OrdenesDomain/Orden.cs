using System;

namespace OrdenesDomain
{
    public class Orden
    {
        public int ID { get; set; }
        // For display
        public string Cliente { get; set; }
        public string Empleado { get; set; }
        public string Transportista { get; set; }
        public string Fecha { get; set; }
        public double Freight { get; set; }
        public string Destino { get; set; }
        public string Ciudad { get; set; }

        // For persistence
        public string CustomerID { get; set; }
        public int? EmployeeID { get; set; }
        public int? ShipVia { get; set; }
        public DateTime? OrderDate { get; set; }
    }
}
