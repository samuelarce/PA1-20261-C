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
using System.Windows.Threading;

namespace Tragamonedas
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private DispatcherTimer timerReloj;
        private DispatcherTimer timerJuego;
        private Random random = new Random();

        private int contadorTicks = 0;
        private const int NUMERO_MAXIMO_TICKS = 60;
        public MainWindow()
        {
            InitializeComponent();
            tblReloj.Text = DateTime.Now.ToString("HH:mm:ss");
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            timerReloj = new DispatcherTimer();
            timerReloj.Interval = TimeSpan.FromSeconds(1);
            timerReloj.Tick += TimerReloj_Tick;
            timerReloj.Start();

            timerJuego = new DispatcherTimer();
            timerJuego.Interval = TimeSpan.FromMilliseconds(100);
            timerJuego.Tick += TimerJuego_Tick;
        }

        private void TimerJuego_Tick(object? sender, EventArgs e)
        {
            int n1 = random.Next(10, 15);
            int n2 = random.Next(10, 15);
            int n3 = random.Next(10, 15);

            txtJugada1.Text = n1.ToString();
            txtJugada2.Text = n2.ToString();
            txtJugada3.Text = n3.ToString();

            contadorTicks++;

            if (contadorTicks >= NUMERO_MAXIMO_TICKS)
            {
                timerJuego.Stop();
                Validar_Jugada(n1, n2, n3);
            }


        }

        private void TimerReloj_Tick(object? sender, EventArgs e)
        {
            tblReloj.Text = DateTime.Now.ToString("HH:mm:ss");
        }

        private void Validar_Jugada(int n1, int n2, int n3)
        {
            if (n1 == n2 && n2 == n3)
            {
                lbResultadoJuego.Content = "¡Ganaste!";
                lbResultadoJuego.Background = Brushes.Green;
            }
            else
            {
                lbResultadoJuego.Content = "Perdiste";
                Color color = (Color)ColorConverter.ConvertFromString("#FFAFD4EC");
                lbResultadoJuego.Background = new SolidColorBrush(color);
            }
            lbResultadoJuego.Visibility = Visibility.Visible;
            btnJugar.IsEnabled = true;
        }
    }
}