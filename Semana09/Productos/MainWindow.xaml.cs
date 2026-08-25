using System.Data;
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

namespace Productos
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string cadenaConexion = "Server=.;Database=Northwind;Integrated Security=True;TrustServerCertificate=True";
        SqlDataAdapter da;
        DataSet ds;
        DataTable dt;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            string query = "select ProductID, ProductName, UnitPrice, UnitsInStock, Discontinued  from Products";
            SqlConnection con = new SqlConnection(cadenaConexion);
            {
                da = new SqlDataAdapter(query, con);
                dt = new DataTable();

                da.Fill(dt);

                dgProductos.ItemsSource = dt.DefaultView;
            }
        }

        private void btnGuardar_Click(object sender, RoutedEventArgs e)
        {
            SqlCommandBuilder builder = new SqlCommandBuilder(da);

            int filasAfectadas = da.Update(dt);

            MessageBox.Show($"Sincronizacion completada, {filasAfectadas} filas afectadas");
        }
    }
}