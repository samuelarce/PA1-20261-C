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

namespace Autolavado
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ObservableCollection<Vehiculo> Vehiculoslist = new ObservableCollection<Vehiculo>();
        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnRegistrar_Click(object sender, RoutedEventArgs e)
        {
            Vehiculo vehiculo = new Vehiculo();
            vehiculo.Placa = tbPlaca.Text;
            if (rbAuto.IsChecked == true)
            {
                vehiculo.Tarifa = 20.00;
            }
            else if (rbCamioneta.IsChecked == true)
            {
                vehiculo.Tarifa = 30.00;
            }
            else if (rbSuv.IsChecked == true)
            {
                vehiculo.Tarifa = 35.00;
            }
            else
                MessageBox.Show("Seleccione Moderlo");
            
                if(cbEncerado.IsChecked==true){
                    vehiculo.Tarifa = vehiculo.Tarifa + 15;
                }if(cbLavado.IsChecked == true) {
                    vehiculo.Tarifa = vehiculo.Tarifa + 20;
                }if(cbLimpieza.IsChecked == true) {
                    vehiculo.Tarifa = vehiculo.Tarifa + 25;
                }

                Vehiculoslist.Add(vehiculo);
            MessageBox.Show("Vehiculo Registrado");
            limpiar();
        }
        public void limpiar()
        {
            tbPlaca.Text = "";
            rbAuto.IsChecked = false;
            rbCamioneta.IsChecked= false;
            rbSuv.IsChecked= false;
            cbEncerado.IsChecked= false;
            cbLavado.IsChecked = false;
            cbLimpieza.IsChecked= false;

        }

        private void btnEstadistica_Click(object sender, RoutedEventArgs e)
        {
            
        }
    }
}
