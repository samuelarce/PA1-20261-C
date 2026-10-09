using System;
using System.Collections.Generic;
using System.Text;

namespace OrdenesDomain
{
    internal class Ordenes
    {
        public string ID { get; set; }
        public string Cliente { get; set; }
        public string Empleado { get; set; }
        public string Transportista { get; set; }
        public string Fecha { get; set; }
        public Double Freight { get; set; }
        public string Destino { get; set; } = string.Empty;
        public string Ciudad { get; set; } = string.Empty;
    }
}
