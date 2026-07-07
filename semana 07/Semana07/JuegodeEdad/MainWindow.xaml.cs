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

namespace JuegodeEdad
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private int edadMinima;
        private int edadMaxima;
        private int edadAleatoria;
        private int contadorIntentos;
        Random random = new Random();
        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnGenerarEdad_Click(object sender, RoutedEventArgs e)
        {
            if (!int.TryParse(tbEdadInferior.Text, out edadMinima))
            { 
                MessageBox.Show ("Ingrese Edad Minima Validad","Validad", MessageBoxButton.OK, MessageBoxImage.Error);
                tbEdadInferior.Focus();
                return;
            }
            if (!int.TryParse(tbEdadSuperior.Text, out edadMaxima))
            {
                MessageBox.Show("Ingrese Edad superior Validad", "Validad", MessageBoxButton.OK, MessageBoxImage.Error);
                tbEdadSuperior.Focus();
                return;
            }

            if(edadMaxima < 0)
            {
                MessageBox.Show("Edad maxima debe ser mayor a cero", "Validad", MessageBoxButton.OK, MessageBoxImage.Error);
                tbEdadSuperior.Focus ();
                return; 
            }

            if (edadMinima < 0)
            {
                MessageBox.Show("Edad minima debe ser mayor a cero", "Validad", MessageBoxButton.OK, MessageBoxImage.Error);
                tbEdadInferior.Focus();
                return;
            }

            if (edadMinima >= edadMaxima)
            {
                MessageBox.Show("Edad minima debe ser menor a la edad maxima", "Validad", MessageBoxButton.OK, MessageBoxImage.Error);
                tbEdadInferior.Focus();
                return;
            }

            
            edadAleatoria = random.Next(edadMinima, edadMaxima + 1);
            contadorIntentos++;

            tbEdadGenerada.Text = edadAleatoria.ToString();
        }

        private void btnCorrecto_Click(object sender, RoutedEventArgs e)
        {
            if (contadorIntentos==0)
            {
                MessageBox.Show("Primero debe hacer click en el boton Primer Intento", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            MessageBox.Show($"¡Felicidades! Adivinaste la edad en {contadorIntentos} intentos.", "Resultado", MessageBoxButton.OK, MessageBoxImage.Information);
            reiniciar();        
        }

        private void reiniciar()
        {
            tbEdadInferior.Clear();
            tbEdadSuperior.Clear();
            tbEdadGenerada.Clear();
            contadorIntentos = 0;
        }

        private void btnIncorrecto_Click(object sender, RoutedEventArgs e)
        {
            if (contadorIntentos == 0)
            {
                MessageBox.Show("Primero debe hacer click en el boton Primer Intento", "Error", MessageBoxButton.OK, MessageBoxImage.Error );
                return;
            }

            contadorIntentos++;

            edadAleatoria = random.Next(edadMinima, edadMaxima + 1);
            tbEdadGenerada.Text = edadAleatoria.ToString();

        }
    }
}