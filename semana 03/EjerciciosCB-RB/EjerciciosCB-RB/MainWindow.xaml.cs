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

namespace EjerciciosCB_RB
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

        private void btnAplicar_Click(object sender, RoutedEventArgs e)
        {
            lbResultado.FontFamily = new FontFamily("Segoe UI");
            lbResultado.Foreground = Brushes.Black;
            lbResultado.Background = Brushes.Transparent;
            if (CheckTipoLetra.IsChecked == true)
            {
                lbResultado.FontFamily = new FontFamily("Arial");
            }
            if(CheckColorTexto.IsChecked == true)
            {
                lbResultado.Foreground = Brushes.Red;
            }
            if (CheckColorFondo.IsChecked == true)
            {
                lbResultado.Background = Brushes.Aqua;
            }
        

        }

        private void btnAplicarRB_Click(object sender, RoutedEventArgs e)
        {
            lbResultadoRB.FontFamily = new FontFamily("Segoe UI");
            lbResultadoRB.Foreground = Brushes.Black;
            lbResultadoRB.Background = Brushes.Transparent;

            if (rbTipodeLetra.IsChecked == true)
            {
                lbResultadoRB.FontFamily = new FontFamily("Times New Roman");
            }else if (rbColorTexto.IsChecked == true)
            {
                lbResultadoRB.Foreground = Brushes.Red;
            }
            else if (rbColorFondo.IsChecked == true)
            {
                lbResultadoRB.Background = Brushes.Yellow;
            }
    }
}
}
