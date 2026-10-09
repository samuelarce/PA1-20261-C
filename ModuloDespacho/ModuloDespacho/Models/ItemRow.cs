using System;
using System.ComponentModel;

namespace ModuloDespacho.Models
{
    public class ItemRow : INotifyPropertyChanged
    {
        public int Id { get; set; }
        public string Producto { get; set; }

        private decimal _precioUnitario;
        public decimal PrecioUnitario
        {
            get => _precioUnitario;
            set
            {
                if (_precioUnitario != value)
                {
                    _precioUnitario = value;
                    OnPropertyChanged(nameof(PrecioUnitario));
                    OnPropertyChanged(nameof(Subtotal));
                }
            }
        }

        private int _cantidad;
        public int Cantidad
        {
            get => _cantidad;
            set
            {
                if (_cantidad != value)
                {
                    _cantidad = value;
                    OnPropertyChanged(nameof(Cantidad));
                    OnPropertyChanged(nameof(Subtotal));
                }
            }
        }

        private decimal _dto;
        public decimal Dto
        {
            get => _dto;
            set
            {
                if (_dto != value)
                {
                    _dto = value;
                    OnPropertyChanged(nameof(Dto));
                    OnPropertyChanged(nameof(Subtotal));
                }
            }
        }

        public decimal Subtotal => Math.Round((PrecioUnitario * Cantidad) * (1 - Dto / 100m), 2);

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
