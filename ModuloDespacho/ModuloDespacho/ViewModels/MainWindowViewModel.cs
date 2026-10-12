using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using MainWindowViewModel.cs;
using ModuloDespacho.Models;

namespace ModuloDespacho.ViewModels
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<ItemRow> Items { get; } = new ObservableCollection<ItemRow>();

        public event Action<AuthorizeRequest>? RequestAuthorize;

        public class AuthorizeRequest
        {
            public string ClientName { get; set; } = string.Empty;
            public string ClientCity { get; set; } = string.Empty;
            public string ClientPhone { get; set; } = string.Empty;
            public string EmployeeName { get; set; } = string.Empty;
            public string TransportName { get; set; } = string.Empty;
            public DateTime Deadline { get; set; }
            public System.Collections.Generic.IEnumerable<ItemRow> Items { get; set; } = new ItemRow[0];
            public decimal Subtotal { get; set; }
            public decimal Flete { get; set; }
        }

        private decimal _subtotal;
        public decimal Subtotal
        {
            get => _subtotal;
            set { _subtotal = value; OnPropertyChanged(nameof(Subtotal)); }
        }

        private decimal _flete;
        public decimal Flete
        {
            get => _flete;
            set { _flete = value; OnPropertyChanged(nameof(Flete)); }
        }

        private decimal _total;
        public decimal Total
        {
            get => _total;
            set { _total = value; OnPropertyChanged(nameof(Total)); }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

        // Commands placeholders
        public ICommand AuthorizeCommand { get; set; }

        // Selected/bound values from the view
        public object? SelectedCustomer { get; set; }
        public object? SelectedTransport { get; set; }
        public DateTime? Deadline { get; set; }
        public string OperatorName { get; set; } = string.Empty;

        public MainWindowViewModel()
        {
            Items.CollectionChanged += (s, e) =>
            {
                // subscribe to PropertyChanged of added items so subtotal updates
                if (e.NewItems != null)
                {
                    foreach (var ni in e.NewItems)
                    {
                        if (ni is INotifyPropertyChanged inpc)
                        {
                            inpc.PropertyChanged += (ss, ee) => RecalculateTotals();
                        }
                    }
                }
                RecalculateTotals();
            };

            AuthorizeCommand = new RelayCommand(p =>
            {
                // Validaciones básicas; la vista puede mostrar mensajes si se requiere
                if (SelectedCustomer == null)
                {
                    System.Windows.MessageBox.Show("Seleccione un cliente antes de autorizar el despacho.", "Validación", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Warning);
                    return;
                }
                if (Items.Count == 0)
                {
                    System.Windows.MessageBox.Show("Agregue al menos un producto al pedido.", "Validación", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Warning);
                    return;
                }
                if (string.IsNullOrWhiteSpace(OperatorName))
                {
                    System.Windows.MessageBox.Show("Ingrese el nombre del operador de despacho.", "Validación", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Warning);
                    return;
                }
                if (SelectedTransport == null)
                {
                    System.Windows.MessageBox.Show("Seleccione el tipo de transportista.", "Validación", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Warning);
                    return;
                }
                if (!Deadline.HasValue)
                {
                    System.Windows.MessageBox.Show("Seleccione la fecha límite de entrega.", "Validación", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Warning);
                    return;
                }

                // calcular subtotal y flete
                var subtotal = Math.Round(Items.Sum(i => i.Subtotal), 2);
                decimal rate = 0.10m;
                try
                {
                    var prop = SelectedTransport.GetType().GetProperty("Rate");
                    if (prop != null)
                    {
                        var v = prop.GetValue(SelectedTransport);
                        if (v is decimal d) rate = d;
                        else if (v != null && decimal.TryParse(v.ToString(), out var pd)) rate = pd;
                    }
                }
                catch { rate = 0.10m; }
                var flete = Math.Round(subtotal * rate, 2);

                // obtener nombre/ciudad/teléfono desde SelectedCustomer por reflexión (la vista puede pasar DTOs)
                string clientName = string.Empty, clientCity = string.Empty, clientPhone = string.Empty;
                {
                    var sc = SelectedCustomer!;
                    var t = sc.GetType();
                    var pcn = t.GetProperty("CompanyName") ?? t.GetProperty("Name");
                    var pc = t.GetProperty("City");
                    var pp = t.GetProperty("Phone");
                    clientName = pcn?.GetValue(sc)?.ToString() ?? string.Empty;
                    clientCity = pc?.GetValue(sc)?.ToString() ?? string.Empty;
                    clientPhone = pp?.GetValue(sc)?.ToString() ?? string.Empty;
                }

                var ar = new AuthorizeRequest
                {
                    ClientName = clientName,
                    ClientCity = clientCity,
                    ClientPhone = clientPhone,
                    EmployeeName = OperatorName,
                    TransportName = (SelectedTransport?.GetType().GetProperty("Name")?.GetValue(SelectedTransport)?.ToString() ?? SelectedTransport?.ToString() ?? string.Empty),
                    Deadline = Deadline.Value,
                    Items = Items.ToList(),
                    Subtotal = subtotal,
                    Flete = flete
                };

                RequestAuthorize?.Invoke(ar);
            }, p => true);
        }

        public void RecalculateTotals(decimal rate = 0.10m)
        {
            Subtotal = Math.Round(Items.Sum(i => i.Subtotal), 2);
            Flete = Math.Round(Subtotal * rate, 2);
            Total = Subtotal + Flete;
        }
    }
}
