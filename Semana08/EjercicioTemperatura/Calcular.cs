using System;
using System.Windows;
using System.Windows.Controls;

namespace EjercicioTemperatura
{
    public partial class MainWindow
    {
        private void Calcular()
        {
            if (txt == null)
            {
                MessageBox.Show("Controles no inicializados.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            int[] rowTotal = new int[4];
            int[] colTotal = new int[4];
            int grandTotal = 0;

            // lectura y validación
            for (int r = 0; r < 4; r++)
            {
                for (int c = 0; c < 4; c++)
                {
                    TextBox tb = txt[r, c];
                    if (tb == null || !int.TryParse(tb.Text, out int v))
                    {
                        MessageBox.Show($"Ingrese un número válido en fila {r + 1}, columna {c + 1}.", "Entrada inválida", MessageBoxButton.OK, MessageBoxImage.Warning);
                        tb?.Focus();
                        return;
                    }

                    rowTotal[r] += v;    // total por partido (fila)
                    colTotal[c] += v;    // total por zona (columna)
                    grandTotal += v;     // total general
                }
            }

            // mostrar totales por partido (columna final junto a cada fila)
            tbRowTotal0.Text = rowTotal[0].ToString();
            tbRowTotal1.Text = rowTotal[1].ToString();
            tbRowTotal2.Text = rowTotal[2].ToString();
            tbRowTotal3.Text = rowTotal[3].ToString();

            // mostrar totales por zona (debajo de la matriz)
            tbColTotal0.Text = colTotal[0].ToString();
            tbColTotal1.Text = colTotal[1].ToString();
            tbColTotal2.Text = colTotal[2].ToString();
            tbColTotal3.Text = colTotal[3].ToString();

            // total final de votantes
            tbGrandTotal.Text = grandTotal.ToString();
            tbTotalVotantes.Text = grandTotal.ToString();

            // candidato ganador (fila con mayor total)
            int idxGanador = 0;
            for (int i = 1; i < 4; i++)
            {
                if (rowTotal[i] > rowTotal[idxGanador]) idxGanador = i;
            }
            tbCandidatoGanador.Text = candidatos[idxGanador];

            // zona con más votantes (columna con mayor total)
            int idxZona = 0;
            for (int i = 1; i < 4; i++)
            {
                if (colTotal[i] > colTotal[idxZona]) idxZona = i;
            }
            tbZonaMasVotantes.Text = zonas[idxZona];
        }
    }
}
