using System.Globalization;

namespace UseThi.Models
{
    public class CryptoCurrency
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Symbol { get; set; }

        public string FormattedPrice => Price.ToString("N3", CultureInfo.InvariantCulture);
    }

}
