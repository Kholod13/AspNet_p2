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
    private readonly SignInManager<User> _signInManager;

    public ProductsController(ShopDbContext context, UserManager<User> userManager, SignInManager<User> signInManager)
    {
        _context = context;
        _userManager = userManager;
        _signInManager = signInManager;
    }

    public async Task<IActionResult> Index()
    {
        var user = await _userManager.GetUserAsync(User);
        var userId = user?.Id;

        // Отримання балансу USDT користувача
        var userCryptoCurrencies = await _context.UserCryptoCurrencies
            .FirstOrDefaultAsync(uc => uc.UserId == userId && uc.CryptoCurrency.Name == "USDT");

        var usdtBalance = userCryptoCurrencies?.AmountOwned ?? 0;

        var viewModel = new WalletViewModel
        {
            UsdtBalance = usdtBalance
        };

        return View(viewModel);
    }
}
