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

namespace RadioButton
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
            int numeroA= Int32.Parse(txtNumeroA.Text);
            int numeroB= Int32.Parse(txtNumeroB.Text);
            double total= 0;
            if (rbSuma.IsChecked == true)
            {
                total= numeroA + numeroB;
            }
            else if (rbResta.IsChecked == true)
            {
                total = numeroA - numeroB;
            }
            else if (rbMultiplicacion.IsChecked == true)
            {
                total = numeroA * numeroB;
            }
            else if (rbDivision.IsChecked == true)
            {
               total = Convert.ToDouble(numeroA) / Convert.ToDouble(numeroB);
            }
            lbResultado.Content = $"Resultado: {Math.Round(total, 2)}";
        }
    }
}