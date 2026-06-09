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

namespace EjercicioRButton
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
            int medida = Int32.Parse(txtMedida.Text);
            double total = 0;
            if (rbCP.IsChecked == true)
            {
                total = medida /2.54;
            }
            else if( rbPC.IsChecked == true)
            {
                total = medida * 2.54;
            }
            lbResultado.Content = $"Resultado: {Math.Round(total, 2)}";
        }
        0
    }
}