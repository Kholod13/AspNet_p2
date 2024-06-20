﻿using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using UseThi.Models;

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
            var cryptocurrencies = await _cryptoService.GetCryptocurrenciesAsync();
            return View(cryptocurrencies);
        }

        public IActionResult Privacy()
        {
            return View();
        }
    }
}
