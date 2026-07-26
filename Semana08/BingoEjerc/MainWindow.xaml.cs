using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Xps;

namespace BingoEjerc_
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<Casillas> misCasillas = new List<Casillas>();
        private List<int> bombo = new List<int>();
        private Random random = new Random();
        public MainWindow()
        {
            InitializeComponent();
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {


                txt00.Text = "14";
                txt01.Text = "20";
                txt02.Text = "32";
                txt03.Text = "52";
                txt04.Text = "71";

                txt10.Text = "10";
                txt11.Text = "27";
                txt12.Text = "42";
                txt13.Text = "55";
                txt14.Text = "64";

                txt20.Text = "7";
                txt21.Text = "23";
                txt23.Text = "58";
                txt24.Text = "69";

                txt30.Text = "11";
                txt31.Text = "28";
                txt32.Text = "34";
                txt33.Text = "56";
                txt34.Text = "72";

                txt40.Text = "15";
                txt41.Text = "25";
                txt42.Text = "33";
                txt43.Text = "53";
                txt44.Text = "66";
        
    
                misCasillas.Add(new Casillas(14));
                misCasillas.Add(new Casillas(20));
                misCasillas.Add(new Casillas(32));
                misCasillas.Add(new Casillas(52));
                misCasillas.Add(new Casillas(71));
                misCasillas.Add(new Casillas(10));
                misCasillas.Add(new Casillas(27));
                misCasillas.Add(new Casillas(42));
                misCasillas.Add(new Casillas(55));
                misCasillas.Add(new Casillas(64));
                misCasillas.Add(new Casillas(7));
                misCasillas.Add(new Casillas(23));
                misCasillas.Add(new Casillas(0, esEspacioLibre: true) { EstaMarcada = true });
                misCasillas.Add(new Casillas(58));
                misCasillas.Add(new Casillas(69));
                misCasillas.Add(new Casillas(11));
                misCasillas.Add(new Casillas(28));
                misCasillas.Add(new Casillas(34));
                misCasillas.Add(new Casillas(56));
                misCasillas.Add(new Casillas(72));
                misCasillas.Add(new Casillas(15));
                misCasillas.Add(new Casillas(25));
                misCasillas.Add(new Casillas(33));
                misCasillas.Add(new Casillas(53));
                misCasillas.Add(new Casillas(66));


                bombo.Clear();
                for (int i = 1; i <= 75; i++) bombo.Add(i);

                tbEstado.Text = "Juego listo. Presione 'Sacar Bolilla' para comenzar.";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar la cartilla: {ex.Message}", "Error de Carga", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        
        }
       
        private void BtnNuevoJuego_Click(object sender, RoutedEventArgs e)
        {
            try
            {
    
                bombo.Clear();
                for (int i = 1; i <= 75; i++) bombo.Add(i);

                misCasillas.Clear();
                foreach (var c in misCasillas)
                {
                    if (c.EsEspacioLibre)
                        c.EstaMarcada = true;
                    else
                        c.EstaMarcada = false;
                }

              
                Limpiar();

                tbBolilla.Text = "--";
                tbEstado.Text = "Cartilla reiniciada para un nuevo juego.";
                btnSacarNumero.IsEnabled = true;

                List<int> bolsadebolillos = new List<int>();
                for (int i = 1; i <= 75; i++) bolsadebolillos.Add(i);

                List<int> nuevosNumeros = new List<int>();
                for (int i = 0; i < 24; i++)
                {
                    int id = random.Next(bolsadebolillos.Count);
                    nuevosNumeros.Add(bolsadebolillos[id]);
                    bolsadebolillos.RemoveAt(id); 
                }

                txt00.Text = nuevosNumeros[0].ToString();
                txt01.Text = nuevosNumeros[1].ToString();
                txt02.Text = nuevosNumeros[2].ToString();
                txt03.Text = nuevosNumeros[3].ToString();
                txt04.Text = nuevosNumeros[4].ToString();

                txt10.Text = nuevosNumeros[5].ToString();
                txt11.Text = nuevosNumeros[6].ToString();
                txt12.Text = nuevosNumeros[7].ToString();
                txt13.Text = nuevosNumeros[8].ToString();
                txt14.Text = nuevosNumeros[9].ToString();

                txt20.Text = nuevosNumeros[10].ToString();
                txt21.Text = nuevosNumeros[11].ToString();
                txt23.Text = nuevosNumeros[12].ToString();
                txt24.Text = nuevosNumeros[13].ToString();

                txt30.Text = nuevosNumeros[14].ToString();
                txt31.Text = nuevosNumeros[15].ToString();
                txt32.Text = nuevosNumeros[16].ToString();
                txt33.Text = nuevosNumeros[17].ToString();
                txt34.Text = nuevosNumeros[18].ToString();

                txt40.Text = nuevosNumeros[19].ToString();
                txt41.Text = nuevosNumeros[20].ToString();
                txt42.Text = nuevosNumeros[21].ToString();
                txt43.Text = nuevosNumeros[22].ToString();
                txt44.Text = nuevosNumeros[23].ToString();


                misCasillas.Add(new Casillas(nuevosNumeros[0]));
                misCasillas.Add(new Casillas(nuevosNumeros[1]));
                misCasillas.Add(new Casillas(nuevosNumeros[2]));
                misCasillas.Add(new Casillas(nuevosNumeros[3]));
                misCasillas.Add(new Casillas(nuevosNumeros[4]))
                    ;
                misCasillas.Add(new Casillas(nuevosNumeros[5]));
                misCasillas.Add(new Casillas(nuevosNumeros[6]));
                misCasillas.Add(new Casillas(nuevosNumeros[7]));
                misCasillas.Add(new Casillas(nuevosNumeros[8]));
                misCasillas.Add(new Casillas(nuevosNumeros[9]));

                misCasillas.Add(new Casillas(nuevosNumeros[10]));
                misCasillas.Add(new Casillas(nuevosNumeros[11]));
                misCasillas.Add(new Casillas(0, esEspacioLibre: true) { EstaMarcada = true });
                misCasillas.Add(new Casillas(nuevosNumeros[12]));
                misCasillas.Add(new Casillas(nuevosNumeros[13]));

                misCasillas.Add(new Casillas(nuevosNumeros[14]));
                misCasillas.Add(new Casillas(nuevosNumeros[15]));
                misCasillas.Add(new Casillas(nuevosNumeros[16]));
                misCasillas.Add(new Casillas(nuevosNumeros[17]));
                misCasillas.Add(new Casillas(nuevosNumeros[18]));

                misCasillas.Add(new Casillas(nuevosNumeros[19]));
                misCasillas.Add(new Casillas(nuevosNumeros[20]));
                misCasillas.Add(new Casillas(nuevosNumeros[21]));
                misCasillas.Add(new Casillas(nuevosNumeros[22]));
                misCasillas.Add(new Casillas(nuevosNumeros[23]));
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al reiniciar el juego: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
        private void Limpiar()
        {
            TextBlock[] controles = { txt00, txt01, txt02, txt03, txt04, txt10, txt11, txt12, txt13, txt14, txt20, txt21, txt23, txt24, txt30, txt31, txt32, txt33, txt34, txt40, txt41, txt42, txt43, txt44 };

            foreach (var txt in controles)
            {
                if (txt != null)
                {
                    txt.Text = ""; 
                    if (txt.Parent is Border borde)
                    {
                        borde.Background = Brushes.White;
                    }
                }
            }
        }

        private void BtnSacarNumero_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (bombo.Count == 0)
                {
                    tbEstado.Text = "Se han sacado todas las bolillas.";
                    btnSacarNumero.IsEnabled = false;
                    return;
                }


                int posicionAleatoria = random.Next(bombo.Count);
                int bolilla = bombo[posicionAleatoria];
                bombo.RemoveAt(posicionAleatoria);


                string letra = LetraBingo(bolilla);
                string bolillaFormateada = $"{letra}-{bolilla}";
                int colInd = ObtenerIndiceColumna(letra);
                tbBolilla.Text = bolillaFormateada;
                bool encontrado = false;

                if (colInd != -1)
                {
                    
                    for (int fila = 0; fila < 5; fila++)
                    {
                        int indiceCasilla = colInd + (fila * 5);
                        var casilla = misCasillas[indiceCasilla];

                        if (!casilla.EsEspacioLibre && casilla.Numero == bolilla)
                        {
                            casilla.EstaMarcada = true;
                            encontrado = true;
                            break; 
                        }
                    }

                    foreach (var c in misCasillas)
                    {
                        if (!c.EsEspacioLibre && c.Numero == bolilla)
                        {
                            c.EstaMarcada = true;
                            encontrado = true;
                        }
                    }

                    if (encontrado)
                    {
                        PintarCasilla(bolilla);
                        tbEstado.Text = $"¡El número {bolilla} está en tu cartilla!";
                    }
                    else
                    {
                        tbEstado.Text = $"Bolilla {bolilla} sacada. No está en la cartilla.";
                    }

                    if (VerificarBingo())
                    {
                        MessageBoxResult respuesta = MessageBox.Show(
                            "¡BINGO!.\n¿Desea jugar nuevamente?",
                            "¡Ganaste!",
                            MessageBoxButton.YesNo,
                            MessageBoxImage.Information);

                        if (respuesta == MessageBoxResult.Yes)
                        {
                            BtnNuevoJuego_Click(sender, e);
                        }
                        else
                        {
                            this.Close();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ocurrió un error inesperado al sacar la bolilla: {ex.Message}", "Error de Ejecución", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }
        private int ObtenerIndiceColumna(string letra)
        {
            switch (letra)
            {
                case "B": 
                    return 0;
                case "I":
                    return 1;
                case "N":
                    return 2;
                case "G":
                    return 3;
                case "O":
                    return 4;
                default: 
                    return -1;
            }
        }


        private string LetraBingo(int numero)
        {
            if (numero >= 1 && numero <= 15) return "B";
            if (numero >= 16 && numero <= 30) return "I";
            if (numero >= 31 && numero <= 45) return "N";
            if (numero >= 46 && numero <= 60) return "G";
            if (numero >= 61 && numero <= 75) return "O";

            return "";
        }
        private void PintarCasilla(int numero)
        {
            TextBlock[] controles = { txt00, txt01, txt02, txt03, txt04, txt10, txt11, txt12, txt13, txt14, txt20, txt21, txt23, txt24, txt30, txt31, txt32, txt33, txt34, txt40, txt41, txt42, txt43, txt44 };

            foreach (var txt in controles)
            {
                if (txt != null && txt.Text == numero.ToString())
                {
                    if (txt.Parent is Border borde)
                    {
                        borde.Background = Brushes.Gold;
                    }
                }
            }
        }
        private bool VerificarBingo()
        {
         
            for (int f = 0; f < 5; f++)
            {
                int inicio = f * 5;
                if (misCasillas[inicio].EstaMarcada &&
                    misCasillas[inicio + 1].EstaMarcada &&
                    misCasillas[inicio + 2].EstaMarcada &&
                    misCasillas[inicio + 3].EstaMarcada &&
                    misCasillas[inicio + 4].EstaMarcada)
                    return true;
            }

            
            for (int c = 0; c < 5; c++)
            {
                if (misCasillas[c].EstaMarcada &&
                    misCasillas[c + 5].EstaMarcada &&
                    misCasillas[c + 10].EstaMarcada &&
                    misCasillas[c + 15].EstaMarcada &&
                    misCasillas[c + 20].EstaMarcada)
                    return true;
            }

            if (misCasillas[0].EstaMarcada && misCasillas[6].EstaMarcada && misCasillas[12].EstaMarcada && misCasillas[18].EstaMarcada && misCasillas[24].EstaMarcada)
                return true;

 
            if (misCasillas[4].EstaMarcada && misCasillas[8].EstaMarcada && misCasillas[12].EstaMarcada && misCasillas[16].EstaMarcada && misCasillas[20].EstaMarcada)
                return true;

            return false;
        }


    }
}