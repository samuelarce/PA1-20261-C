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

namespace EncriptarClave
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnEncriptar_Click(object sender, RoutedEventArgs e)

        {
            if (string.IsNullOrEmpty(pbClave.Password))
            {
                MessageBox.Show("Por favor, ingrese una clave para encriptar.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            string clave = pbClave.Password;
            string claveEncriptada = Encriptar(clave);
            MessageBox.Show("Clave encriptada correctamente.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);

            txtClaveEncriptada.Text = claveEncriptada;

        }
        private string Encriptar(string clave)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(clave);
            string claveEncriptada = Convert.ToBase64String(bytes);
            return claveEncriptada;
            
        }
    }
}