using Data.Data.Entities;

namespace UseThi.Models
{
    public class WalletViewModel
    {
        public string UserEmail { get; set; }
        public List<UserCryptoCurrency> Cryptocurrencies { get; set; }
        public decimal UsdtBalance { get; set; }
    }
}
