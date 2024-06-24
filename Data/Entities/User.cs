using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using UseThi.Models; // Імпортуйте неймспейс з CryptoCurrency

namespace Data.Data.Entities
{
    public class User : IdentityUser
    {
        public DateTime? Birthdate { get; set; }
        public decimal? Balance { get; set; }
        public List<UserCryptoCurrency> UserCryptoCurrencies { get; set; }
    }

    public class UserCryptoCurrency
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int CryptoCurrencyId { get; set; }
        public int AmountOwned { get; set; } // Змінили з decimal на int
        public CryptoCurrency CryptoCurrency { get; set; }
        public User User { get; set; }
    }

}
