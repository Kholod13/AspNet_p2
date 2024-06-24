using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Linq;
using UseThi.Models;
using System.Collections.Generic;
using Data.Data.Entities;

namespace UseThi.Controllers
{
    public class HomeController : Controller
    {
        private readonly CryptoService _cryptoService;

        public HomeController(CryptoService cryptoService)
        {
            _cryptoService = cryptoService;
        }

        public async Task<IActionResult> Index()
        {
            var userCryptocurrencies = await _cryptoService.GetCryptocurrenciesAsync();

            var cryptocurrencies = userCryptocurrencies.Select(uc => new CryptoCurrency
            {
                Id = uc.Id,
                Name = uc.Name,
                Symbol = uc.Symbol,
                Price = uc.Price
                // Заповніть інші властивості, якщо необхідно
            }).ToList();

            var viewModel = new WalletViewModel
            {
                Cryptocurrencies = userCryptocurrencies,
            };

            return View(viewModel); // Передача моделі WalletViewModel у представлення
        }

        public IActionResult Privacy()
        {
            return View();
        }
    }
}
