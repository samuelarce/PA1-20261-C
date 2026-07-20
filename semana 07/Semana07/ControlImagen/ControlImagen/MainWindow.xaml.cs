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

namespace ControlImagen
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
        private void window_Loaded(object sender, RoutedEventArgs e)
        {
            // Cargar la imagen desde un archivo

            BitmapImage bitmapImage = new BitmapImage();
            bitmapImage.BeginInit();
            bitmapImage.UriSource = new Uri(@"C:\Users\samaq\Downloads\images.jpg");
            bitmapImage.EndInit();
            imagen1.Source = bitmapImage;


        }
    }
}