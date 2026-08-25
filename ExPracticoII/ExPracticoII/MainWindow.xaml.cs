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
using System.Data;
using Microsoft.Data.SqlClient;

namespace ExPracticoII
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DispatcherTimer timer = new DispatcherTimer();
        public MainWindow()
        {
            InitializeComponent();
            timer.Interval = TimeSpan.FromSeconds(5);
            timer.Tick += Timer_Tick;
        }
        private void Timer_Tick(object sender, EventArgs e)
        {
            CargarPedidos();
        }
        private void CargarPedidos()
        {
            string CadenaConexion = @"Server=localhost;Database=Northwind;Integrated Security=True;TrustServerCertificate=True;";

            using (SqlConnection conexion = new SqlConnection(CadenaConexion))
            {
                try
                {
                    conexion.Open();

                    MessageBox.Show("Conexión correcta");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }
    }
}