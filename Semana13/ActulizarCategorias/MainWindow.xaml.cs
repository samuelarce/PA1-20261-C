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

namespace ActulizarCategorias
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string cn = ConfigurationManager.ConnectionStrings["ActulizarCategorias.Properties.Settings.NORTWHIND"].ConnectionString;
        public MainWindow()
        {
            InitializeComponent();
        }
        private void btnNuevo_Click(object sender, RoutedEventArgs e)
        {
            this.Nuevo();
        }

        private void Nuevo()
        {
            txtId.Clear();
            txtNombre.Clear();
            txtDescripcion.Clear();
            txtNombre.Focus();
        }
        private void btnAgregar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(cn))
                {
                    conn.Open();
                    SqlCommand cmd = conn.CreateCommand();
                    cmd.CommandType = System.Data.CommandType.Text;

                    string id = txtId.Text;
                    if (string.IsNullOrEmpty(id))
                    {
                        cmd.CommandText = "INSERT INTO Categories(CategoryName,Description) VALUES(@Nombre,@Descripcion); select SCOPE_IDENTITY();";
                        cmd.Parameters.Add("@Nombre", System.Data.SqlDbType.NVarChar, 15).Value = txtNombre.Text;
                        cmd.Parameters.Add("@Descripcion", System.Data.SqlDbType.NText).Value = string.IsNullOrEmpty(txtDescripcion.Text) ? (object)DBNull.Value : txtDescripcion.Text;
                        int idGenerado = Convert.ToInt32(cmd.ExecuteScalar());

                        MessageBox.Show($"Categoria agregada con Id {idGenerado}");
                        this.Nuevo();
                        this.CargarListaCategorias();
                    }
                    else
                    {
                        cmd.CommandText = @"UPDATE CATEGORIES 
                                            SET CategoryName=@Nombre,
                                            Description=@Descripcion
                                            WHERE CategoryID=@Id";
                        cmd.Parameters.Add("@Nombre", System.Data.SqlDbType.NVarChar, 15).Value = txtNombre.Text;
                        cmd.Parameters.Add("@Descripcion", System.Data.SqlDbType.NText).Value = string.IsNullOrEmpty(txtDescripcion.Text) ? (object)DBNull.Value : txtDescripcion.Text;
                        cmd.Parameters.Add("@Id", System.Data.SqlDbType.Int).Value = id;

                        int filas = cmd.ExecuteNonQuery();

                        if (filas > 0)
                        {
                            MessageBox.Show("Categoria actualizada");
                            this.CargarListaCategorias();
                        }
                    }

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
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.CargarListaCategorias();
        }
        private void CargarListaCategorias()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(cn))
                {
                    string query = "SELECT CategoryID,CategoryName,Description FROM Categories ORDER BY CategoryID";
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(query, conn);
                    SqlDataReader reader = cmd.ExecuteReader();
                    List<Categoria> lista = new List<Categoria>();

                    while (reader.Read())
                    {
                        lista.Add(new Categoria
                        {
                            Id = reader.GetInt32(0),
                            Nombre = reader.GetString(1),
                            Descripcion = reader.IsDBNull("Description") ? null : reader.GetString(2)
                        });
                    }
                    dgCategorias.ItemsSource = lista;
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
        private void dgCategorias_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgCategorias.SelectedItem != null)
            {
                Categoria categoria = (Categoria)dgCategorias.SelectedItem;
                txtId.Text = categoria.Id.ToString();
                txtNombre.Text = categoria.Nombre.ToString();
                txtDescripcion.Text = categoria.Descripcion;
            }
        }

    }
}