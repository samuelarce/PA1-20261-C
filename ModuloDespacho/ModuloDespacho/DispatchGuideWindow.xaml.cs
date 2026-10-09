using System.Windows;
using System.Globalization;

namespace ModuloDespacho
{
    public partial class DispatchGuideWindow : Window
    {
        public DispatchGuideWindow()
        {
            InitializeComponent();

            GuideClientName.Text = "";
            GuideClientCity.Text = "";
            GuideClientPhone.Text = "";
            GuideEmployee.Text = "";
            GuideTransport.Text = "";
            GuideDeadline.Text = "Fecha requerida: " + System.DateTime.Now.ToString("yyyy-MM-dd");
        }

        public void InitializeFromMain(string clientName, string clientCity, string clientPhone, string employeeName, string transportName, System.DateTime deadline, System.Collections.Generic.IEnumerable<object> items, decimal subtotal, decimal flete)
        {
            // Usar ViewModel para la guía y asignar DataContext
            var vm = new ViewModels.DispatchGuideViewModel();
            vm.ClientName = clientName;
            vm.ClientCity = clientCity;
            vm.ClientPhone = clientPhone;
            vm.EmployeeName = employeeName?.ToUpperInvariant() ?? string.Empty;
            vm.TransportName = transportName;
            vm.Deadline = deadline;

            // Copiar items transformándolos al modelo de la guía (soporta distintos tipos de origen)
            if (items is System.Collections.IEnumerable ie)
            {
                foreach (var it in ie)
                {
                    if (it == null) continue;
                    // Extraer propiedades comunes por reflexión para soportar MainWindow.ItemRow o DTOs
                    var t = it.GetType();
                    int id = 0;
                    string producto = string.Empty;
                    decimal precioUnitario = 0m;
                    int cantidad = 0;
                    decimal dto = 0m;

                    var pid = t.GetProperty("Id") ?? t.GetProperty("ProductId");
                    var pprod = t.GetProperty("Producto") ?? t.GetProperty("ProductName") ?? t.GetProperty("Name");
                    var ppu = t.GetProperty("PrecioUnitario") ?? t.GetProperty("UnitPrice") ?? t.GetProperty("Price");
                    var pcant = t.GetProperty("Cantidad") ?? t.GetProperty("Quantity");
                    var pdto = t.GetProperty("Dto") ?? t.GetProperty("Discount");

                    if (pid != null) int.TryParse(pid.GetValue(it)?.ToString(), out id);
                    if (pprod != null) producto = pprod.GetValue(it)?.ToString() ?? string.Empty;
                    if (ppu != null) decimal.TryParse(ppu.GetValue(it)?.ToString(), out precioUnitario);
                    if (pcant != null) int.TryParse(pcant.GetValue(it)?.ToString(), out cantidad);
                    if (pdto != null)
                    {
                        var v = pdto.GetValue(it);
                        if (v is decimal dec) dto = dec;
                        else if (v is double d) dto = (decimal)d;
                        else if (v is float f) dto = (decimal)f;
                        else if (v != null) decimal.TryParse(v.ToString(), out dto);
                    }

                    vm.Items.Add(new ModuloDespacho.Models.ItemRow { Id = id, Producto = producto, PrecioUnitario = precioUnitario, Cantidad = cantidad, Dto = dto });
                }
            }

            vm.Subtotal = subtotal;
            vm.Flete = flete;
            vm.Total = subtotal + flete;

            this.DataContext = vm;

            // Asignar valores visuales (la vista es simple y usa TextBlocks)
            GuideItemsGrid.ItemsSource = vm.Items;
            GuideClientName.Text = $"Cliente: {vm.ClientName}";
            GuideClientCity.Text = $"Ciudad: {vm.ClientCity}";
            GuideClientPhone.Text = $"Teléfono: {vm.ClientPhone}";
            GuideEmployee.Text = vm.EmployeeName;
            GuideEmployee.FontWeight = System.Windows.FontWeights.Bold;
            GuideTransport.Text = $"Transportista: {vm.TransportName}";
            GuideDeadline.Text = "Fecha requerida: " + vm.Deadline.ToString("dd 'de' MMMM 'de' yyyy", new CultureInfo("es-ES"));
            GuideSubtotal.Text = $"Subtotal: S/{vm.Subtotal:0.00}";
            GuideFlete.Text = $"Flete: S/{vm.Flete:0.00}";
            GuideTotal.Text = $"S/{vm.Total:0.00}";
        }

        // La guía es solo lectura y muestra los totales recibidos desde la ventana principal.

        private void CloseGuideButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void GuideItemsGrid_RemoveButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is FrameworkElement fe && fe.DataContext != null)
            {
                var item = fe.DataContext;
                var list = GuideItemsGrid.ItemsSource as System.Collections.IList;
                if (list != null && list.Contains(item))
                {
                    list.Remove(item);
                    // actualizar totales si el ItemsSource es ObservableCollection<ItemRow>
                    GuideItemsGrid.ItemsSource = null;
                    GuideItemsGrid.ItemsSource = list;
                }
            }
        }
    }
}
