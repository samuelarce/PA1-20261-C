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

namespace OrdenarCadenas
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
        private string[] arrayNombres;
        private void btnListar_Click(object sender, RoutedEventArgs e)
        {
            string textOrigen = txtCadenaInicial.Text.Trim();

            if (string.IsNullOrWhiteSpace(textOrigen))
            {
                MessageBox.Show("La cadena de nombres esta vacía",
                    "Validar", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            arrayNombres = textOrigen.Split(' ');
            lbNombres.Items.Clear();
            foreach (string nombre in arrayNombres)
            {
                lbNombres.Items.Add(nombre);
            }

            txtCantidadNombres.Text = lbNombres.Items.Count.ToString();

        }

        private void btnFiltrar_Click(object sender, RoutedEventArgs e)
        {
            string filtro = txtLetra.Text;
            lbFiltrado.Items.Clear();
            foreach (string nombre in arrayNombres)
            {
                if (nombre.StartsWith(filtro, StringComparison.OrdinalIgnoreCase))
                {
                    lbFiltrado.Items.Add(nombre);
                }
            }
            txtCantidadFiltrado.Text = lbFiltrado.Items.Count.ToString();
        }
    }
}