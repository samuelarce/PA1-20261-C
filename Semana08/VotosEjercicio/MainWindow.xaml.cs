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

namespace VotosEjercicio
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private TextBox[] txtVotos= new TextBox[16];
        private string[] candidatos = new string[] { "Buhito", "Aguila", "Torito", "Lorito" };
        private string[] zonas = new string[] { "A", "B", "C", "D" };
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            txt00.Text = "122";
            txt01.Text = "254";
            txt02.Text = "382";
            txt03.Text = "445";

            txt10.Text = "472";
            txt11.Text = "364";
            txt12.Text = "205";
            txt13.Text = "228";

            txt20.Text = "143";
            txt21.Text = "117";
            txt22.Text = "474";
            txt23.Text = "293";

            txt30.Text = "411";
            txt31.Text = "202";
            txt32.Text = "261";
            txt33.Text = "335";
        }
    }
}