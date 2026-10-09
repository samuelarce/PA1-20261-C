using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using ModuloDespacho.Models;

namespace ModuloDespacho.ViewModels
{
    public class DispatchGuideViewModel : INotifyPropertyChanged
    {
        public string ClientName { get; set; }
        public string ClientCity { get; set; }
        public string ClientPhone { get; set; }

        public string EmployeeName { get; set; }
        public string TransportName { get; set; }
        public DateTime Deadline { get; set; }

        public ObservableCollection<ItemRow> Items { get; set; } = new ObservableCollection<ItemRow>();

        public decimal Subtotal { get; set; }
        public decimal Flete { get; set; }
        public decimal Total { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
