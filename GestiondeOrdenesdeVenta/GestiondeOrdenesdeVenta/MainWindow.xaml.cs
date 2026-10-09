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
using OrdenesDomain;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using OrdenesApplication;

namespace GestiondeOrdenesdeVenta
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly IClienteService _clienteService;
        private readonly IEmpleadoService _empleadoService;
        private readonly IOrdenService _ordenService;
        private readonly ObservableCollection<Orden> _ordenes = new ObservableCollection<Orden>();

        public MainWindow(IClienteService clienteService, IEmpleadoService empleadoService, IOrdenService ordenService)
        {
            _clienteService = clienteService;
            _empleadoService = empleadoService;
            _ordenService = ordenService;

            InitializeComponent();
            CargarCombosDesdeServicios();
            // bind ObservableCollection so UI se actualiza automáticamente
            lvOrden.ItemsSource = _ordenes;
            CargarOrdenes();
        }

        private void BtnGuardar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var orden = new Orden();
                orden.CustomerID = cboCliente.SelectedValue as string;
                orden.EmployeeID = cboEmpleado.SelectedValue == null ? null : (int?)((int)cboEmpleado.SelectedValue);
                orden.ShipVia = GetSelectedShipVia();
                orden.OrderDate = dpFecha.SelectedDate;
                if (double.TryParse(txtMontodeEnvio.Text, out double freight)) orden.Freight = freight;
                orden.Destino = txtNombreDestino.Text;
                orden.Ciudad = txtCiudadDestino.Text;

                var result = _ordenService.Create(orden);
                if (result.Success)
                {
                    MessageBox.Show("Orden creada. ID: " + result.Data);
                    // Intentar recuperar la orden insertada para verificar existencia
                    var guardada = _ordenService.GetById(result.Data);
                    if (guardada != null)
                    {
                        CargarOrdenes();
                        // seleccionar la nueva orden en la lista
                        SelectOrdenEnLista(result.Data);
                        // limpiar campos opcionalmente
                        LimpiarCampos();
                    }
                    else
                    {
                        MessageBox.Show("La orden fue insertada pero no se pudo leer con GetById.");
                    }
                }
                else
                {
                    MessageBox.Show("Error: " + result.Message);
                }
            }
            catch (System.Exception ex)
            {
                MessageBox.Show("Error guardando orden: " + ex.Message);
            }
        }

        private void BtnActualizar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!int.TryParse(txtOrderId.Text, out int id)) { MessageBox.Show("ID inválido"); return; }
                var orden = new Orden { ID = id };
                orden.CustomerID = cboCliente.SelectedValue as string;
                orden.EmployeeID = cboEmpleado.SelectedValue == null ? null : (int?)((int)cboEmpleado.SelectedValue);
                orden.ShipVia = GetSelectedShipVia();
                orden.OrderDate = dpFecha.SelectedDate;
                if (double.TryParse(txtMontodeEnvio.Text, out double freight)) orden.Freight = freight;
                orden.Destino = txtNombreDestino.Text;
                orden.Ciudad = txtCiudadDestino.Text;

                var result = _ordenService.Update(orden);
                MessageBox.Show(result.Success ? "Actualizado" : "Error: " + result.Message);
                if (result.Success) { CargarOrdenes(); LimpiarCampos(); }
            }
            catch (System.Exception ex)
            {
                MessageBox.Show("Error actualizando: " + ex.Message);
            }
        }

        private void BtnEliminar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!int.TryParse(txtOrderId.Text, out int id)) { MessageBox.Show("ID inválido"); return; }
                var res = MessageBox.Show("Confirma eliminar orden " + id + "?", "Confirmar", MessageBoxButton.YesNo);
                if (res != MessageBoxResult.Yes) return;
                var result = _ordenService.Delete(id);
                MessageBox.Show(result.Success ? "Eliminado" : "Error: " + result.Message);
                if (result.Success) { CargarOrdenes(); LimpiarCampos(); }
            }
            catch (System.Exception ex)
            {
                MessageBox.Show("Error eliminando: " + ex.Message);
            }
        }

        private void BtnLimpiar_Click(object sender, RoutedEventArgs e)
        {
            LimpiarCampos();
        }

        private void LimpiarCampos()
        {
            txtOrderId.Text = string.Empty;
            cboCliente.SelectedIndex = -1;
            cboEmpleado.SelectedIndex = -1;
            rbSpeedy.IsChecked = false;
            rbUnited.IsChecked = false;
            rbFederal.IsChecked = false;
            dpFecha.SelectedDate = null;
            txtMontodeEnvio.Text = string.Empty;
            txtNombreDestino.Text = string.Empty;
            txtCiudadDestino.Text = string.Empty;
        }

        private void LvOrden_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lvOrden.SelectedItem is Orden orden)
            {
                txtOrderId.Text = orden.ID.ToString();
                // intentar seleccionar cliente/empleado por id
                if (!string.IsNullOrEmpty(orden.CustomerID)) cboCliente.SelectedValue = orden.CustomerID;
                if (orden.EmployeeID.HasValue) cboEmpleado.SelectedValue = orden.EmployeeID.Value;
                // seleccionar transportista
                if (orden.ShipVia.HasValue)
                {
                    rbSpeedy.IsChecked = orden.ShipVia == 1;
                    rbUnited.IsChecked = orden.ShipVia == 2;
                    rbFederal.IsChecked = orden.ShipVia == 3;
                }
                dpFecha.SelectedDate = orden.OrderDate;
                txtMontodeEnvio.Text = orden.Freight.ToString();
                txtNombreDestino.Text = orden.Destino;
                txtCiudadDestino.Text = orden.Ciudad;
            }
        }

        private void CargarOrdenes()
        {
            try
            {
                var lista = new List<Orden>(_ordenService.GetAll());
                // actualizar ObservableCollection para que la vista reaccione
                _ordenes.Clear();
                foreach (var o in lista)
                {
                    _ordenes.Add(o);
                }
                // no seleccionar por defecto
                lvOrden.SelectedIndex = -1;
            }
            catch (System.Exception ex)
            {
                MessageBox.Show("Error cargando órdenes: " + ex.Message);
            }
        }

        private void SelectOrdenEnLista(int id)
        {
            if (lvOrden.ItemsSource is IEnumerable<Orden> items)
            {
                foreach (var o in items)
                {
                    if (o.ID == id)
                    {
                        lvOrden.SelectedItem = o;
                        lvOrden.ScrollIntoView(o);
                        break;
                    }
                }
            }
        }

        private int? GetSelectedShipVia()
        {
            if (rbSpeedy.IsChecked == true) return 1;
            if (rbUnited.IsChecked == true) return 2;
            if (rbFederal.IsChecked == true) return 3;
            return null;
        }

        private void CargarCombosDesdeServicios()
        {
            try
            {
                var clientes = _clienteService.GetAll();
                cboCliente.ItemsSource = clientes;
                cboCliente.DisplayMemberPath = "Nombre";
                cboCliente.SelectedValuePath = "Id";

                var empleados = _empleadoService.GetAll();
                cboEmpleado.ItemsSource = empleados;
                cboEmpleado.DisplayMemberPath = "Nombre";
                cboEmpleado.SelectedValuePath = "Id";
            }
            catch (System.Exception ex)
            {
                MessageBox.Show("Error cargando combos: " + ex.Message);
            }
        }

    }
}