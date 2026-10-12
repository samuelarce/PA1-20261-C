using System.Text;
using System.Windows;
using System.ComponentModel;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ModuloDespacho.Models;

namespace ModuloDespacho
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly ModuloDepachoInfraestructura.NorthwindRepository _repo;
        private System.Collections.Generic.List<ModuloDespachoDomain.Models.Employee> _employees = new System.Collections.Generic.List<ModuloDespachoDomain.Models.Employee>();

        public MainWindow()
        {
            InitializeComponent();
            _repo = new ModuloDepachoInfraestructura.NorthwindRepository();
            var vm = new ViewModels.MainWindowViewModel();
            DataContext = vm;
            ItemsDataGrid.ItemsSource = vm.Items;
            AttachCollectionHandlers();

            ClientsListBox.SetBinding(ListBox.SelectedItemProperty,
                new Binding(nameof(vm.SelectedCustomer)) { Mode = BindingMode.TwoWay });
            OperatorTextBox.SetBinding(TextBox.TextProperty,
                new Binding(nameof(vm.OperatorName))
                { Mode = BindingMode.TwoWay, UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged });
            TransportTypeCombo.SetBinding(ComboBox.SelectedItemProperty,
                new Binding(nameof(vm.SelectedTransport)) { Mode = BindingMode.TwoWay });
            DeadlineDatePicker.SetBinding(DatePicker.SelectedDateProperty,
                new Binding(nameof(vm.Deadline)) { Mode = BindingMode.TwoWay });
            CatalogListBox.MouseDoubleClick -= CatalogListBox_MouseDoubleClick;
            CatalogListBox.MouseDoubleClick += CatalogListBox_MouseDoubleClick;

            var timer = new System.Windows.Threading.DispatcherTimer();
            timer.Interval = System.TimeSpan.FromSeconds(1);
            timer.Tick += (s, e) =>
            {

                if (this.FindName("SystemTimeText") is System.Windows.Controls.TextBlock sysText)
                    sysText.Text = System.DateTime.Now.ToString("HH:mm");


                var uptime = System.DateTime.Now - System.Diagnostics.Process.GetCurrentProcess().StartTime;
                if (this.FindName("SessionDurationText") is System.Windows.Controls.TextBlock sessionText)
                    sessionText.Text = uptime.ToString("hh\\:mm\\:ss");
            };
            timer.Start();


            try
            {

                var custRepo = new ModuloDepachoInfraestructura.Repositories.CustomerRepository();
                var prodRepo = new ModuloDepachoInfraestructura.Repositories.ProductRepository();
                var empRepo = new ModuloDepachoInfraestructura.Repositories.EmployeeRepository();

                var customers = custRepo.GetAll();
                ClientsListBox.ItemsSource = customers;

                var products = prodRepo.GetAll();
                CatalogListBox.ItemsSource = products;


                var employees = empRepo.GetAll();


                var transportRepo = new ModuloDepachoInfraestructura.Repositories.TransportRepository();
                var transports = transportRepo.GetAll();
                TransportTypeCombo.ItemsSource = transports;
                TransportTypeCombo.DisplayMemberPath = "Name";



                _employees = employees;





                ItemsDataGrid.ItemsSource = vm.Items;

                vm.RequestAuthorize += (req) =>
                {
                    var win = new DispatchGuideWindow();
                    win.Owner = this;
                    win.InitializeFromMain(req.ClientName, req.ClientCity, System.Text.RegularExpressions.Regex.Replace(req.ClientPhone ?? string.Empty, "\\D", ""), req.EmployeeName, req.TransportName, req.Deadline, req.Items, req.Subtotal, req.Flete);
                    win.ShowDialog();
                };


                UpdateTotal();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error cargando datos: " + ex.Message);
            }
        }

        // Helper para extraer información de cliente desde diferentes tipos posibles
        private (string Id, string CompanyName, string City, string Phone)? ExtractCustomer(object selected)
        {
            if (selected == null) return null;
            // CustomerDto en Infraestructura
            if (selected is ModuloDepachoInfraestructura.CustomerDto cdto)
                return (cdto.CustomerId, cdto.CompanyName, cdto.City, cdto.Phone);
            // Domain Customer
            if (selected is ModuloDespachoDomain.Models.Customer cdom)
                return (cdom.CustomerId, cdom.CompanyName, cdom.City, cdom.Phone);
            // Si hay un objeto anónimo con propiedades comunes
            var t = selected.GetType();
            var pid = t.GetProperty("CustomerId") ?? t.GetProperty("Id") ?? t.GetProperty("ID");
            var pcn = t.GetProperty("CompanyName") ?? t.GetProperty("Name");
            var pc = t.GetProperty("City");
            var pp = t.GetProperty("Phone");
            if (pid != null && pcn != null)
            {
                var id = pid.GetValue(selected)?.ToString() ?? string.Empty;
                var name = pcn.GetValue(selected)?.ToString() ?? string.Empty;
                var city = pc?.GetValue(selected)?.ToString() ?? string.Empty;
                var phone = pp?.GetValue(selected)?.ToString() ?? string.Empty;
                return (id, name, city, phone);
            }
            return null;
        }

        private void ClientsListBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            // La selección y carga del cliente se realizan en SelectionChanged.
        }

        private void TransportTypeCombo_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (sender is System.Windows.Controls.ComboBox combo)
            {
                var sel = combo.SelectedItem;
                string text = string.Empty;
                if (sel == null) text = string.Empty;
                else
                {
                    // if item is a ComboBoxItem (hardcoded) or a TransportType object
                    if (sel is System.Windows.Controls.ComboBoxItem cbi) text = cbi.Content?.ToString() ?? string.Empty;
                    else
                    {
                        var prop = sel.GetType().GetProperty("Name");
                        if (prop != null) text = prop.GetValue(sel)?.ToString() ?? sel.ToString();
                        else text = sel.ToString();
                    }
                }

                if (this.FindName("SelectedTransportType") is System.Windows.Controls.TextBlock selectedText)
                    selectedText.Text = text;
                // Recalcular totales cuando cambia el transportista (afecta al flete)
                UpdateTotal();
            }
        }

        private void ClientsListBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (sender is System.Windows.Controls.ListBox lb)
            {
                var info = ExtractCustomer(lb.SelectedItem);
                if (info != null)
                {
                    var cust = info.Value;
                    // SelectedClientLine removed from XAML; values are shown in ClientNameText/ClientCityText/ClientPhoneText

                    if (this.FindName("ClientNameText") is System.Windows.Controls.TextBlock cn)
                        cn.Text = cust.CompanyName;
                    if (this.FindName("ClientCityText") is System.Windows.Controls.TextBlock cc)
                        cc.Text = cust.City;
                    if (this.FindName("ClientPhoneText") is System.Windows.Controls.TextBlock cp)
                    {
                        var raw = cust.Phone ?? string.Empty;
                        var cleaned = System.Text.RegularExpressions.Regex.Replace(raw, "\\D", "");
                        cp.Text = cleaned;
                    }

                    try
                    {
                        var ordersRepo = new ModuloDepachoInfraestructura.Repositories.OrdersRepository();
                        var lastItems = ordersRepo.GetLastOrderDetails(cust.Id);
                        _items.Clear();
                        if (lastItems != null && lastItems.Count > 0)
                        {
                            foreach (var it in lastItems)
                            {
                                _items.Add(new ItemRow { Id = it.ProductId, Producto = it.ProductName, PrecioUnitario = it.UnitPrice, Cantidad = it.Quantity, Dto = (decimal)it.Discount * 100m });
                            }
                        }
                        UpdateTotal();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error cargando pedidos previos: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                }
                else
                {
                    if (this.FindName("SelectedClientLine") is System.Windows.Controls.TextBlock clientLine)
                        clientLine.Text = string.Empty;
                    if (this.FindName("ClientNameText") is System.Windows.Controls.TextBlock cn)
                        cn.Text = string.Empty;
                    if (this.FindName("ClientCityText") is System.Windows.Controls.TextBlock cc)
                        cc.Text = string.Empty;
                    if (this.FindName("ClientPhoneText") is System.Windows.Controls.TextBlock cp)
                        cp.Text = string.Empty;
                }
            }
        }

        private void DeadlineDatePicker_SelectedDateChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (DeadlineDatePicker.SelectedDate.HasValue)
            {
                DeadlineSelectedText.Text = DeadlineDatePicker.SelectedDate.Value.ToString("yyyy-MM-dd");
            }
            else
            {
                DeadlineSelectedText.Text = string.Empty;
            }
        }

        private void AuthorizeButton_Click(object sender, RoutedEventArgs e)
        {

            var custInfo = ExtractCustomer(ClientsListBox.SelectedItem);
            if (custInfo == null)
            {
                MessageBox.Show("Seleccione un cliente antes de autorizar el despacho.", "Validación", MessageBoxButton.OK, MessageBoxImage.Warning);
                ClientsListBox.Focus();
                return;
            }
            var selectedCustomer = custInfo.Value;

            var vmLocal = this.DataContext as ViewModels.MainWindowViewModel;
            if (vmLocal == null)
            {
                MessageBox.Show("Error interno: ViewModel no inicializado.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (vmLocal.Items.Count == 0)
            {
                MessageBox.Show("Agregue al menos un producto al pedido.", "Validación", MessageBoxButton.OK, MessageBoxImage.Warning);
                CatalogListBox.Focus();
                return;
            }

            var operatorName = (OperatorTextBox.Text ?? string.Empty).Trim();
            if (string.IsNullOrEmpty(operatorName))
            {
                MessageBox.Show("Ingrese el nombre del operador de despacho.", "Validación", MessageBoxButton.OK, MessageBoxImage.Warning);
                OperatorTextBox.Focus();
                return;
            }

            // Validar que el nombre del operario sea un nombre válido (solo letras y espacios, al menos 2 caracteres)
            var nameClean = System.Text.RegularExpressions.Regex.Replace(operatorName, "\\s+", " ").Trim();
            if (nameClean.Length < 2 || !System.Text.RegularExpressions.Regex.IsMatch(nameClean, "^[A-Za-zÀ-ÖØ-öø-ÿ ]+$"))
            {
                MessageBox.Show("Ingrese un nombre de operario válido.", "Validación", MessageBoxButton.OK, MessageBoxImage.Warning);
                OperatorTextBox.Focus();
                return;
            }

            if (TransportTypeCombo.SelectedItem == null)
            {
                MessageBox.Show("Seleccione el tipo de transportista.", "Validación", MessageBoxButton.OK, MessageBoxImage.Warning);
                TransportTypeCombo.Focus();
                return;
            }

            if (!DeadlineDatePicker.SelectedDate.HasValue)
            {
                MessageBox.Show("Seleccione la fecha límite de entrega.", "Validación", MessageBoxButton.OK, MessageBoxImage.Warning);
                DeadlineDatePicker.Focus();
                return;
            }

            // Validar que la fecha límite sea mayor o igual a la fecha actual
            if (DeadlineDatePicker.SelectedDate.HasValue)
            {
                var selectedDate = DeadlineDatePicker.SelectedDate.Value.Date;
                if (selectedDate < System.DateTime.Now.Date)
                {
                    MessageBox.Show("La fecha límite debe ser igual o posterior a la fecha actual.", "Validación", MessageBoxButton.OK, MessageBoxImage.Warning);
                    DeadlineDatePicker.Focus();
                    return;
                }
            }

            // Preparar datos reales para la guía
            var clientName = selectedCustomer.CompanyName;
            var clientCity = selectedCustomer.City;
            var clientPhone = selectedCustomer.Phone;

            string transportName = string.Empty;
            var tsel = TransportTypeCombo.SelectedItem;
            var prop = tsel.GetType().GetProperty("Name");
            if (prop != null) transportName = prop.GetValue(tsel)?.ToString() ?? tsel.ToString();
            else transportName = tsel.ToString();

            // items para la guía: usar la colección del ViewModel
            var guideItems = vmLocal.Items;

            // calcular subtotal
            decimal subtotal = 0;
            foreach (var it in vmLocal.Items) subtotal += it.Subtotal;

            // calcular flete según transportista si tiene propiedad Rate
            decimal flete = 0;
            try
            {
                var rateProp = tsel.GetType().GetProperty("Rate");
                if (rateProp != null)
                {
                    var rateVal = rateProp.GetValue(tsel);
                    if (rateVal is decimal d) flete = Math.Round(subtotal * d, 2);
                    else
                    {
                        // intentar convertir
                        if (rateVal != null && decimal.TryParse(rateVal.ToString(), out var dv))
                            flete = Math.Round(subtotal * dv, 2);
                    }
                }
                else
                {
                    flete = Math.Round(subtotal * 0.10m, 2);
                }
            }
            catch
            {
                flete = Math.Round(subtotal * 0.10m, 2);
            }

            var win = new DispatchGuideWindow();
            win.Owner = this;
            win.InitializeFromMain(clientName, clientCity, System.Text.RegularExpressions.Regex.Replace(clientPhone ?? string.Empty, "\\D", ""), operatorName, transportName, DeadlineDatePicker.SelectedDate ?? System.DateTime.Now, guideItems, subtotal, flete);
            win.ShowDialog();
        }


        // ItemRow moved to ModuloDespacho.Models.ItemRow and is used throughout the UI to avoid duplicate types.

        // Alias a la única colección utilizada por la tabla y Autorizar.
        private System.Collections.ObjectModel.ObservableCollection<ItemRow> _items
            => ((ViewModels.MainWindowViewModel)DataContext).Items;

        private void AttachCollectionHandlers()
        {
            _items.CollectionChanged -= Items_CollectionChanged;
            _items.CollectionChanged += Items_CollectionChanged;
            // Suscribir a cambios de propiedad de items existentes
            foreach (var it in _items)
            {
                it.PropertyChanged -= Item_PropertyChanged;
                it.PropertyChanged += Item_PropertyChanged;
            }
        }

        private void Items_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
            {
                foreach (var ni in e.NewItems)
                {
                    if (ni is ItemRow ir)
                    {
                        ir.PropertyChanged -= Item_PropertyChanged;
                        ir.PropertyChanged += Item_PropertyChanged;
                    }
                }
            }
            if (e.OldItems != null)
            {
                foreach (var oi in e.OldItems)
                {
                    if (oi is ItemRow ir)
                        ir.PropertyChanged -= Item_PropertyChanged;
                }
            }

            UpdateTotal();
        }

        private void Item_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e == null) return;
            if (e.PropertyName == nameof(ItemRow.Subtotal) || e.PropertyName == nameof(ItemRow.Cantidad) || e.PropertyName == nameof(ItemRow.Dto) || e.PropertyName == nameof(ItemRow.PrecioUnitario))
                UpdateTotal();
        }

        private void CatalogListBox_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (DataContext is not ViewModels.MainWindowViewModel vm)
                return;
            var sel = CatalogListBox.SelectedItem;
            if (sel == null) return;

            int id;
            string name;
            decimal price;
            int stock = int.MaxValue;

            if (sel is ModuloDepachoInfraestructura.ProductDto p)
            {
                id = p.ProductId;
                name = p.ProductName;
                price = p.UnitPrice;
            }
            else if (sel is ModuloDespachoDomain.Models.Product dp)
            {
                id = dp.ProductId;
                name = dp.ProductName;
                price = dp.UnitPrice;
                stock = dp.UnitsInStock;
            }
            else
            {
                return;
            }

            var existing = System.Linq.Enumerable.FirstOrDefault(vm.Items, it => it.Id == id);
            if (existing != null)
            {
                if (existing.Cantidad + 1 > stock)
                {
                    MessageBox.Show($"No hay suficiente stock para '{name}'. Stock disponible: {stock}", "Stock insuficiente", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                else
                {
                    existing.Cantidad += 1;

                }
            }
            else
            {
                if (1 > stock)
                {
                    MessageBox.Show($"No hay stock disponible para '{name}'.", "Stock insuficiente", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                else
                {
                    vm.Items.Add(new ItemRow { Id = id, Producto = name, PrecioUnitario = price, Cantidad = 1, Dto = 0 });
                }
            }


            UpdateTotal();
        }

        private void ItemsDataGrid_RemoveButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is FrameworkElement fe && fe.DataContext is ItemRow row)
            {
                if (_items.Contains(row))
                {
                    _items.Remove(row);

                    UpdateTotal();
                }
            }
        }

        // Operator now entered manually in OperatorTextBox; no selection handler required

        private void CatalogFilterTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var tb = sender as TextBox;
            var filter = tb?.Text?.Trim() ?? string.Empty;
            if (string.IsNullOrEmpty(filter))
            {
                CatalogListBox.ItemsSource = ModuloDepachoInfraestructura.Repositories.ProductRepositoryCache.Products;
                return;
            }

            var list = System.Linq.Enumerable.Where(ModuloDepachoInfraestructura.Repositories.ProductRepositoryCache.Products, p => p.ProductName.IndexOf(filter, System.StringComparison.OrdinalIgnoreCase) >= 0);
            CatalogListBox.ItemsSource = System.Linq.Enumerable.ToList(list);
        }

        private void UpdateTotal()
        {
            if (DataContext is not ViewModels.MainWindowViewModel vm) return;
            decimal total = 0;
            foreach (var it in vm.Items)
                total += it.Subtotal;
            // calcular subtotal, flete (5%) y total general
            decimal subtotal = total;
            // Determinar tasa de flete según transportista seleccionado (si existe)
            decimal rate = 0.10m; // por defecto 10%
            try
            {
                if (TransportTypeCombo?.SelectedItem != null)
                {
                    var tsel = TransportTypeCombo.SelectedItem;
                    var rateProp = tsel.GetType().GetProperty("Rate");
                    if (rateProp != null)
                    {
                        var rv = rateProp.GetValue(tsel);
                        if (rv is decimal dv) rate = dv;
                        else if (rv != null && decimal.TryParse(rv.ToString(), out var pd)) rate = pd;
                    }
                }
            }
            catch { rate = 0.10m; }
            decimal flete = Math.Round(subtotal * rate, 2);
            decimal totalGeneral = subtotal + flete;
            vm.RecalculateTotals(rate);

            // actualizar TextBlocks nombrados en XAML (nuevos names)
            if (this.FindName("SubtotalTb") is System.Windows.Controls.TextBlock st)
                st.Text = $"Subtotal: S/{subtotal:0.00}";
            if (this.FindName("FleteTb") is System.Windows.Controls.TextBlock ft)
                ft.Text = $"Flete: S/{flete:0.00}";
            if (this.FindName("TotalTb") is System.Windows.Controls.TextBlock tb)
                tb.Text = $"S/{totalGeneral:0.00}";
        }


        private T FindVisualChildByName<T>(DependencyObject parent, string name) where T : DependencyObject
        {
            if (parent == null) return null;
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);
                if (child is FrameworkElement fe && fe.Name == name) return (T)child;
                var result = FindVisualChildByName<T>(child, name);
                if (result != null) return result;
            }
            return null;
        }

        private void ItemsDataGrid_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {

            Dispatcher.BeginInvoke(new System.Action(() =>
            {

                UpdateTotal();
            }));
        }


    }
}