using Data.Data.Entities;
using Data.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UseThi.Models;
using System.Threading.Tasks;

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
            .FirstOrDefaultAsync(uc => uc.UserId == userId && uc.CryptoCurrency.Name == "USDT");

        var usdtBalance = userCryptoCurrencies?.AmountOwned ?? 0;

        var viewModel = new WalletViewModel
        {
            UsdtBalance = usdtBalance
        };

        return View(viewModel); // Ensure this matches the expected view model in the view
    }
}
