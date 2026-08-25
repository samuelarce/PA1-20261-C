using Microsoft.Data.SqlClient;
using System.Configuration;
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


namespace AgregarProducto
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string cn = ConfigurationManager.ConnectionStrings["AgregarProducto.Properties.Settings.NORTHWIND"].ConnectionString;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnNuevo_Click(object sender, RoutedEventArgs e)
        {
            Limpiar();
        }
        private async void btnAgregar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(cn))
                {
                    await conn.OpenAsync();
                    using (SqlCommand command = conn.CreateCommand())
                    {
                        command.CommandTimeout = 60;
                        command.CommandText = "SP_InsertarProducto";
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        command.Parameters.Add("@Nombre", System.Data.SqlDbType.NVarChar, 40).Value = txtNombre.Text;
                        command.Parameters.Add("@Precio", System.Data.SqlDbType.Money).Value = txtPrecio.Text;
                        int id = Convert.ToInt32(await command.ExecuteScalarAsync());
                        MessageBox.Show($"Producto Registrado con id {id}");
                        Limpiar();
                    }
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show($"Error {ex.Number}, {ex.Message}");
            }
        }
        private void Limpiar()
        {
            txtNombre.Clear();
            txtPrecio.Clear();
            txtNombre.Focus();
        }
    }
    }
