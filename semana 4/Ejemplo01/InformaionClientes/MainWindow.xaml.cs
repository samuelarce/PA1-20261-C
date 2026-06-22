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

namespace InformaionClientes
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
       ObservableCollection<Impresiones> impresionesList = new ObservableCollection<Impresiones>();
        public MainWindow()
        {
            InitializeComponent();
        }

       
        private void btnRegistrar_Click(object sender, RoutedEventArgs e)
        {
            Cliente cliente = new Cliente();
            cliente.Nombre = tbCliente.Text;
            cliente.Celular = tbCelular.Text;
            Impresiones impresiones = new Impresiones();
            impresiones.Nombre = cliente.Nombre;
            impresiones.Celular = cliente.Celular;
            impresiones.cantidad = tbPaginas.Text != "" ? int.Parse(tbPaginas.Text) : 0;
           if (rbEscolar.IsChecked == true) {
                impresiones.tarifa = 0.30;
            }else if(rbUniversitario.IsChecked== true){ 
                impresiones.tarifa = 0.50;
            }
            else if (rbOrganizacion.IsChecked == true)
            {
                impresiones.tarifa = 0.80;
            }
            else
            {
                MessageBox.Show("Seleccione una tarifa");
                return;
            }
            impresiones.importe = Math.Round(impresiones.cantidad * impresiones.tarifa,2);

            impresionesList.Add(impresiones);
            MessageBox.Show("Cliente registrado exitosamente");
            limpiar();

        }
        public void limpiar()
        {
            tbCliente.Text = "";
            tbCelular.Text = "";
            tbPaginas.Text = "";
            rbEscolar.IsChecked = false;
            rbUniversitario.IsChecked = false;
            rbOrganizacion.IsChecked = false;
        }

        private void btnEstadistica_Click(object sender, RoutedEventArgs e)
        {
            lbCliente.Items.Clear();
            lbCelular.Items.Clear();
            lbPaginas.Items.Clear();
            lbImporte.Items.Clear();
            lbTarifa.Items.Clear();
            int impEscolares = 0;
            int impUniversitarios = 0;
            int impOrganizacion = 0;
            foreach (Impresiones impresiones in impresionesList)
            {
                lbCliente.Items.Add($"* {impresiones.Nombre}");
                lbCelular.Items.Add(impresiones.Celular);
                lbPaginas.Items.Add(impresiones.cantidad);
                lbTarifa.Items.Add(impresiones.tarifa);
                lbImporte.Items.Add(impresiones.importe);
                if (impresiones.tarifa == 0.30)
                {
                    impEscolares += impresiones.cantidad;
                }
                else if (impresiones.tarifa == 0.50)
                {
                    impUniversitarios += impresiones.cantidad;
                }
                else if (impresiones.tarifa == 0.80)
                {
                    impOrganizacion += impresiones.cantidad;
                }
            
            }
            tbImpEscolares.Text = impEscolares.ToString();
            tbImpreOrganizacionales.Text = impOrganizacion.ToString();
            tbImpreUniversitarios.Text = impUniversitarios.ToString();

        }

        private void btnSalir_Click(object sender, RoutedEventArgs e)
        {
            var r = MessageBox.Show("¿Desea salir de la aplicación?", "Confirmar", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (r == MessageBoxResult.Yes)
                this.Close();
        }

        
    }
    }
