using System;
using System.Collections.Generic;
using System.Text;

namespace RegistroClientes
{
    internal class Cliente
    {
        public Cliente()
        {

        }
        public Cliente (string apellidos, string nombre)
        {
            this.Apellidos = apellidos;
            this.Nombres= "rosmel";
        }
        public string Apellidos { get; set; }
        public string Nombres { get; set; }
        public string Dni { get; set; }
        public String Direccion { get; set; }
        public String EstadoCivil  { get; set; }

    }
}
