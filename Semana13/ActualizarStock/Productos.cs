using System;
using System.Collections.Generic;
using System.Text;

namespace ActualizarStock
{
    internal class Productos
    {
        public int Id { get; set; }
        public string? Nombre { get; set; } = string.Empty;
        public Decimal? Precio { get; set; }

        public Int16? Stock { get; set; }

        public byte[]? RowVersion { get; set; }
    }
}
