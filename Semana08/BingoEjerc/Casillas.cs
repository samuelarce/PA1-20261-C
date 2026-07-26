using System;
using System.Collections.Generic;
using System.Text;

namespace BingoEjerc_
{
    internal class Casillas
    {
        public int Numero { get; set; }
        public bool EstaMarcada { get; set; }
        public bool EsEspacioLibre { get; set; }

        public Casillas(int numero, bool esEspacioLibre = false)
        {
            Numero = numero;
            EsEspacioLibre = esEspacioLibre;
            EstaMarcada = esEspacioLibre;
        }
    }
}
