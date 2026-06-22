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

namespace EjercicioPropuesto1
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
            string nombre = tbNombre.Text;
            string codigo = tbCodigo.Text;
            int ingreso = Int32.Parse(tbIngreso.Text);
            double lbFonaviResultado = 0;
            double lbImpRentaResultado = 0;
            double lbAFPResultado = 0;
            double lbTotalaPagar = 0;
            if (cbFonavi.IsChecked == true)
            {
                lbFonaviResultado = ingreso * 0.08;
            } 
            if(cbImpRenta.IsChecked == true)
            {
                lbImpRentaResultado = ingreso * 0.05;
            }
            if(cbAFP.IsChecked == true)
            {
                lbAFPResultado = ingreso * 0.12;
            }
            lbTotalaPagar = ingreso - (lbFonaviResultado + lbImpRentaResultado + lbAFPResultado);
            this.lbFonaviResultado.Content = lbFonaviResultado;
            this.lbImpRentaResultado.Content = lbImpRentaResultado;
            this.lbAFPResultado.Content = lbAFPResultado;
            this.lbTotalaPagar.Content = lbTotalaPagar;
        }
    }

    }
    
