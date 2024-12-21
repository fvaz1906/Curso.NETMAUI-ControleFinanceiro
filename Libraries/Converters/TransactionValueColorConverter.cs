using oovaz_financeiro.Models;
using System.Globalization;

namespace oovaz_financeiro.Libraries.Converters
{
    public class TransactionValueColorConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            Transaction transaction = (Transaction)value;
            if (transaction == null)
            {
                return null;
            }

            if (transaction.Type == TransactionType.Income)
                return Color.FromArgb("#939E5A");
            else
                return Color.FromArgb("#c23004");
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
