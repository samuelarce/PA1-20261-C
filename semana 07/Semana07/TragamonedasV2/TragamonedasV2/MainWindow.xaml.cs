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

namespace TragamonedasV2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private System.Windows.Threading.DispatcherTimer _clockTimer;
        private System.Windows.Threading.DispatcherTimer _gameTimer;
        private int _secondsElapsed;
        private int _score;
        private readonly Random _rand = new Random();
        private readonly string[] _imageFiles = new string[] { "1.png", "2.png", "3.png", "4.png", "5.png", "6.png" };

        public MainWindow()
        {
            InitializeComponent();
            InitializeClock();
        }
        private void InitializeClock()
        {
            // DispatcherTimer para actualizar el reloj superior cada segundo
            _clockTimer = new System.Windows.Threading.DispatcherTimer();
            _clockTimer.Interval = TimeSpan.FromSeconds(1);
            _clockTimer.Tick += ClockTimer_Tick;
            // Actualiza inmediatamente y arranca el temporizador
            UpdateClockText();
            _clockTimer.Start();
        }

        private void ClockTimer_Tick(object? sender, EventArgs e)
        {
            UpdateClockText();
        }

        private void UpdateClockText()
        {
            // Formato HH:mm:ss como en la imagen
            if (TimerText != null)
            {
                TimerText.Text = DateTime.Now.ToString("HH:mm:ss");
            }
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            StartButton.IsEnabled = false;
            StartGame();
        }

        private void StartGame()
        {
            // reiniciar estado
            _secondsElapsed = 0;
            _score = 0;
            UpdateScoreText();
            ResultText.Text = string.Empty;

            // crear timer de juego si no existe
            if (_gameTimer == null)
            {
                _gameTimer = new System.Windows.Threading.DispatcherTimer();
                _gameTimer.Interval = TimeSpan.FromSeconds(1);
                _gameTimer.Tick += GameTimer_Tick;
            }

            // ejecutar inmediatamente una primera tirada y arrancar
            DoGameTick();
            _gameTimer.Start();
        }

        private void StopGame()
        {
            if (_gameTimer != null && _gameTimer.IsEnabled)
                _gameTimer.Stop();
            StartButton.IsEnabled = true;
        }

        private void GameTimer_Tick(object? sender, EventArgs e)
        {
            DoGameTick();
        }

        private void DoGameTick()
        {
            _secondsElapsed++;

            // seleccionar 3 imágenes aleatorias del conjunto
            int a = _rand.Next(0, _imageFiles.Length);
            int b = _rand.Next(0, _imageFiles.Length);
            int c = _rand.Next(0, _imageFiles.Length);

            SetImageSource(Img1, _imageFiles[a]);
            SetImageSource(Img2, _imageFiles[b]);
            SetImageSource(Img3, _imageFiles[c]);

            // calcular puntaje para esta tirada
            int added = 0;
            if (a == b && b == c)
            {
                added = 20;
            }
            else if (a == b || a == c || b == c)
            {
                added = 10;
            }

            _score += added;
            UpdateScoreText();

            // condición de finalización: alcanza 60 puntos o llega a 8 segundos
            if (_score >= 60)
            {
                EndGame(true);
                return;
            }

            if (_secondsElapsed >= 8)
            {
                EndGame(false);
                return;
            }
        }

        private void EndGame(bool won)
        {
            StopGame();

            if (won)
            {
                ResultText.Text = "GANASTE";
            }
            else
            {
                ResultText.Text = $"PERDISTE, puntaje obtenido: {_score}";
            }

            // preguntar si desea seguir jugando
            var msg = won ? "GANASTE. ¿Deseas jugar otra vez?" : $"PERDISTE con {_score} puntos. ¿Deseas jugar otra vez?";
            var resp = MessageBox.Show(msg, "Fin del juego", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (resp == MessageBoxResult.Yes)
            {
                // reiniciar y comenzar de nuevo
                StartGame();
            }
            else
            {
                // dejar la UI lista para otra partida
                StartButton.IsEnabled = true;
            }
        }

        private void UpdateScoreText()
        {
            if (ScoreText != null)
                ScoreText.Text = _score.ToString();
        }

        private void SetImageSource(Image imgControl, string fileName)
        {
            try
            {
                if (imgControl == null) return;
                // Intentar cargar la imagen desde los recursos del ensamblado
                var uri = new Uri($"pack://application:,,,/Imagenes/{fileName}", UriKind.Absolute);

                var bmp = new BitmapImage();
                bmp.BeginInit();
                bmp.UriSource = uri;
                bmp.CacheOption = BitmapCacheOption.OnLoad;
                bmp.EndInit();
                imgControl.Source = bmp;
            }
            catch
            {
                // Silenciar errores de carga de imagen para evitar crashes en tiempo de ejecución
            }
        }

    }
}