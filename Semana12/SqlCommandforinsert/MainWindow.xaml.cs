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
using Microsoft.Data.SqlClient; 

namespace SqlCommandforinsert
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

        private void btnAgregar_Click(object sender, RoutedEventArgs e)
        {
            string cn = ConfigurationManager.ConnectionStrings[""].ConnectionString;
            try
            {
                using (SqlConnection conex = new SqlConnection(cn))
                {
                    string query = "INSERT INTO CUSTOMERS(CustomerID,CompanyName) values(@Id,@Nombre)";
                    using (SqlCommand cmd = new SqlCommand(query, conex))
                    {
                        cmd.Parameters.Add("@Id", System.Data.SqlDbType.NChar, 5).Value = txtId.Text;
                        cmd.Parameters.Add("@Nombre", System.Data.SqlDbType.NVarChar, 40).Value = txtNombre.Text;
                        conex.Open();
                        int filasAfectadas = cmd.ExecuteNonQuery();
                        if (filasAfectadas > 0)
                        {
                            MessageBox.Show("Cliente agregado correctamente");
                            this.Nuevo();
                        }
                        else
                        {
                            MessageBox.Show("No fue posible agregar el cliente");
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                if (ex.Number == 2627)
                {
                    MessageBox.Show("El cliente ya existe");
                }
                else
                {
                    MessageBox.Show($"Error al agregar cliente: {ex.Number}, {ex.Message}");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error generico: {ex.Message}");
            }

        }

        private void btnNuevo_Click(object sender, RoutedEventArgs e)
        {
            this.Nuevo();
        }

        private void Nuevo()
        {
            txtId.Clear();
            txtNombre.Clear();
        }
    }
}