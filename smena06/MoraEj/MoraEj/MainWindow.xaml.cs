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

namespace MoraEj
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
        
        private void btnCalcular_Click(object sender, RoutedEventArgs e)
        {
            string Cliente =tbCliente.Text;

            double Monto= double.Parse(tbMonto.Text);
            
            if (dpVencimiento.SelectedDate == null || dpPago.SelectedDate == null)
            {
                MessageBox.Show("Seleccione ambas fechas");
                return;
            }
            DateTime FechaVencimiento = dpVencimiento.SelectedDate.Value;
            DateTime FechaPago = dpPago.SelectedDate.Value;
            int DiasMoratorios = (FechaPago - FechaVencimiento).Days; 
            Double PocentajeMoratorio = DiasMoratorios * 0.5;
            Double MoraSoles = Monto * (PocentajeMoratorio/100);
            Double MontoPagar = Monto + MoraSoles;
            TBDIASM.Text = DiasMoratorios.ToString();
            tbMoraSoles.Text = MontoPagar.ToString();
            tbMoraPorc.Text = PocentajeMoratorio.ToString();
            tbMoraSoles.Text = MoraSoles.ToString();
            tbMontoaPagar.Text = MontoPagar.ToString();

            int diasmora = 0;

            if(FechaPago > FechaVencimiento)
            {
                TimeSpan.
            }


        }

        private void btnNuevo_Click(object sender, RoutedEventArgs e)
        {
            tbCliente.Clear();
            tbMonto.Clear();    
            tbMontoaPagar.Clear();
            tbMoraPorc.Clear();
            tbMoraSoles.Clear();
            TBDIASM.Clear();

            dpVencimiento.SelectedDate = null;
            dpPago.SelectedDate = null;
        }
    }
}