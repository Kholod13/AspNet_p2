using Data.Data.Entities;
using Data.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UseThi.Models;
using System.Linq;
using System.Threading.Tasks;

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
        if (user == null)
        {
            // Handle case where user is not authenticated
            return RedirectToAction("Login", "Account"); // Redirect to login page
        }

        var userId = user.Id;

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
