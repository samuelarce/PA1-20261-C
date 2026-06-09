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

namespace CheckBoxyradioBut
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

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void btnCalcular_Click(object sender, RoutedEventArgs e)
        {
            int cantidad = Int32.Parse(txtCantidad.Text);
            double total = 0;
            if (cbCebolla.IsChecked == true)
            {
                total += cantidad * 3;
            }
            if (cbTomate.IsChecked == true)
            {
                total += cantidad * 1.50;
            }
            if (cbPapa.IsChecked == true)
            {
                total += cantidad * 2;
            }

            lbTotal.Content = $"Total: {total:C}";
        }
    }
}