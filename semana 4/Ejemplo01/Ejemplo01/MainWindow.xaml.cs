using RegistroClientes;
using System.Collections.ObjectModel;
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

namespace Ejemplo01
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ObservableCollection<Cliente> listaClientes = new ObservableCollection<Cliente>
        {
            new Cliente
            {
                Apellidos = "Perez",
                Nombres = "Juan",
                EstadoCivil = "Soltero(a)",
                Dni = "12345678",
                Direccion = "Av. Siempre Viva 123"
            },
            new Cliente
            {
                Apellidos = "Gomez",
                Nombres = "Maria",
                EstadoCivil = "Casado(a)",
                Dni = "87654321",
                Direccion = "Calle Falsa 456"
            },
            new Cliente
            {
                Apellidos = "Lopez",
                Nombres = "Carlos",
                EstadoCivil = "Soltero(a)",
                Dni = "11223344",
                Direccion = "Av. Libertad 789"
            }
        };
        
        public MainWindow()
        {
            InitializeComponent();
            lvClientes.ItemsSource = listaClientes;
        }

        private void btnGrabar_Click(object sender, RoutedEventArgs e)
        {
            Cliente cliente = new Cliente();
            cliente.Apellidos = txtApellidos.Text;
            cliente.Nombres = txtNombres.Text;
            ComboBoxItem selectedItemEC = (ComboBoxItem)cmbEstadoCivil.SelectedItem;
            cliente.EstadoCivil = selectedItemEC.Content.ToString();
            cliente.Dni = txtDNI.Text;
            cliente.Direccion = txtDireccion.Text;
   

            listaClientes.Add(cliente); 
           
            lvClientes.ItemsSource = listaClientes;

            limpiar();
           
        }
        private void limpiar()
        {
            txtApellidos.Text = "";
            txtNombres.Text = "";
            cmbEstadoCivil.SelectedIndex = -1;
            txtDNI.Text = "";
            txtDireccion.Text = "";
        }

        private void btnNuevo_Click(object sender, RoutedEventArgs e)
        {
            limpiar();
        }

        private void btnEstadistica_Click(object sender, RoutedEventArgs e)
        {
            int solteros = 0;
            int casados = 0;
            foreach (Cliente cliente in listaClientes)
            {
                if (cliente.EstadoCivil == "Soltero(a)")
                {
                    solteros++;
                }
                else if (cliente.EstadoCivil == "Casado(a)")
                {
                    casados++;
                }
            }
            txtCasados.Text = casados.ToString();
            txtSolteros.Text = solteros.ToString();
        }

        private void btnSalir_Click(object sender, RoutedEventArgs e)
        {
            var r = MessageBox.Show("¿Desea salir de la aplicación?", "Confirmar", MessageBoxButton.YesNo, MessageBoxImage.Question); 
            if (r == MessageBoxResult.Yes) 
                this.Close();

        }
    }
}