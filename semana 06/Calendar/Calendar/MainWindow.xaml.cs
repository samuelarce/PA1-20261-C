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

namespace Calendar
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

        private void btnCronograma_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtPaciente.Text))
            {
                MessageBox.Show("Por favor, ingrese el nombre del paciente.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            string paciente = txtPaciente.Text;
            string tratamiento = ((ComboBoxItem)cbxTratamiento1.SelectedItem)?.Content?.ToString();
            string pieza = ((ComboBoxItem)cbxPiezaDental.SelectedItem)?.Content?.ToString();
            DateTime fechaCita = calCita.SelectedDate.Value;
            DateTime citaProxima = fechaCita.AddDays(15); // Asumiendo que la próxima cita es 15 días después de la actual

            string reporte = $"Reporte de Cita\n" +
                "========================\n" +
                $"Paciente: {paciente}\n" +
                $"Tratamiento: {tratamiento}\n" +
                $"Pieza Dental: {pieza}\n" +
                $"Fecha de Cita: {fechaCita.ToLongDateString()}\n" +
                "------------------------\n" +
                $"Fecha Proxima cita: {citaProxima.ToShortDateString()}";

            txtReporte.Text = reporte;
        }
    }
}