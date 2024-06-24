using Data.Data.Entities;
using Data.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using UseThi.Models;

public class ProductsController : Controller
{
    private readonly ShopDbContext _context;
    private readonly UserManager<User> _userManager;

    public ProductsController(ShopDbContext context, UserManager<User> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    public async Task<IActionResult> Index()
    {
        var user = await _userManager.GetUserAsync(User);
        var userId = user?.Id;

        var userCryptoCurrencies = await _context.UserCryptoCurrencies
            .Include(uc => uc.CryptoCurrency)
            .Where(uc => uc.UserId == userId)
            .ToListAsync();

        var usdtCryptoCurrency = userCryptoCurrencies.FirstOrDefault(uc => uc.CryptoCurrency.Name == "USDT");

        var usdtBalance = usdtCryptoCurrency?.AmountOwned ?? 1000;

        var viewModel = new WalletViewModel
        {
            UsdtBalance = usdtBalance,
            UserEmail = user.Email,
            Cryptocurrencies = userCryptoCurrencies.Select(uc => uc.CryptoCurrency).ToList()
        };

        // Присвоюємо usdtBalance для елементу USDT в Cryptocurrencies
        if (usdtCryptoCurrency != null)
        {
            var usdtCrypto = viewModel.Cryptocurrencies.FirstOrDefault(c => c.Id == usdtCryptoCurrency.CryptoCurrencyId);
            if (usdtCrypto != null)
            {
                usdtCrypto.UsdtBalance = usdtBalance;
            }
        }

        return View(viewModel);
    }

    [HttpPost]
    public async Task<IActionResult> Deposit(decimal amount)
    {
        if (amount <= 0)
        {
            ModelState.AddModelError("", "Invalid deposit amount");
            return RedirectToAction("Index");
        }

        var user = await _userManager.GetUserAsync(User);
        var userId = user?.Id;

        var userCryptoCurrencies = await _context.UserCryptoCurrencies
            .FirstOrDefaultAsync(uc => uc.UserId == userId && uc.CryptoCurrency.Name == "USDT");

        if (userCryptoCurrencies != null)
        {
            userCryptoCurrencies.AmountOwned += (int)amount; // Оновлення AmountOwned
            _context.Update(userCryptoCurrencies);
        }
        else
        {
            // Створення нового запису, якщо він не існує
            userCryptoCurrencies = new UserCryptoCurrency
            {
                UserId = userId,
                CryptoCurrencyId = 1, // ID криптовалюти USDT (потрібно вставити коректний ID)
                AmountOwned = (int)amount
            };
            _context.Add(userCryptoCurrencies);
        }

        await _context.SaveChangesAsync();

        return RedirectToAction("Index");
    }

}
