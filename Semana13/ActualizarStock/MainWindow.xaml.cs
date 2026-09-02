using Microsoft.Data.SqlClient;
using System.Configuration;
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

namespace ActualizarStock
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string cn = ConfigurationManager.ConnectionStrings["ActualizarStock.Properties.Settings.NORTWHIND"].ConnectionString;
        Productos producto;

        public MainWindow()
        {
            InitializeComponent();
        }


        private void dgCategorias_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgProductos.SelectedItem != null)
            {
                producto = (Productos)dgProductos.SelectedItem;

                txtId.Text = producto.Id.ToString();
                txtNombre.Text = producto.Nombre;
                txtStock.Text = producto.Stock.ToString();
            }


        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            CargarListaProductos();
        }
        private void CargarListaProductos()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(cn))
                {
                    string query = "SELECT ProductID,ProductName,UnitPrice,UnitsInStock FROM Products ORDER BY ProductName";
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(query, conn);
                    SqlDataReader reader = cmd.ExecuteReader();
                    List<Productos> lista = new List<Productos>();

                    while (reader.Read())
                    {
                        lista.Add(new Productos
                        {
                            Id = reader.GetInt32(0),
                            Nombre = reader.GetString(1),
                            Precio = reader.IsDBNull("UnitPrice") ? null : reader.GetDecimal(2),
                            Stock = reader.IsDBNull("UnitsInStock") ? null : reader.GetInt16(3),
                            RowVersion = (byte[])reader["RowVersion"]
                        });
                    }
                    dgProductos.ItemsSource = lista;
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show($"Error en sql {ex.Number}, {ex.Message}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error general {ex.Message}");
            }
        }
        private void btnActualizar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string id = txtId.Text;
                if (string.IsNullOrEmpty(id))
                {
                    return;
                }
                using (SqlConnection con = new SqlConnection(cn))
                using (SqlCommand cmd = new SqlCommand())
                {
                    con.Open();
                    cmd.Connection = con;
                    cmd.CommandText = "SP_UpadateProductoStock";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@ProducID", SqlDbType.Int).Value = id;
                    cmd.Parameters.Add("@UnitsInStock", SqlDbType.SmallInt).Value = txtStock.Text;
                    cmd.Parameters.Add("@RowVersion", SqlDbType.Timestamp).Value = producto.RowVersion;

                    int filasAfectadas = cmd.ExecuteNonQuery();
                    if (filasAfectadas > 0)
                    {
                        MessageBox.Show($"Stock Actualizado para el producto: {txtNombre.Text}");
                        CargarListaProductos();
                    }
                    else
                    {
                        MessageBox.Show($"Producto fue modificado por otro usuario");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }
    }
}