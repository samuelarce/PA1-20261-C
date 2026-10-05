using System;
using System.Globalization;
using System.Windows.Data;

namespace GestiondeOrdenesdeVenta
{
    // Convierte el ActualWidth del ListView y un parámetro (peso) en un ancho proporcional para columnas
    public class PropWidthConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double totalWidth && parameter != null)
            {
                if (double.TryParse(parameter.ToString(), NumberStyles.Any, CultureInfo.InvariantCulture, out double weight))
                {
                    // Restar margen aproximado para padding y scroll; ajustable si es necesario
                    double adjusted = Math.Max(0, totalWidth - 32);
                    return Math.Max(30, adjusted * weight);
                }
            }
            return 100.0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
