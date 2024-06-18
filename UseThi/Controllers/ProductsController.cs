using Microsoft.AspNetCore.Mvc;
using Data.Data.Entities;
using Data.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

public class ProductsController : Controller
{
    private readonly ShopDbContext _context;
    private readonly IMapper mapper;
    private readonly UserManager<User> _userManager;

    public ProductsController(ShopDbContext context, IMapper mapper, UserManager<User> userManager)
    {
        _context = context;
        this.mapper = mapper;
        _userManager = userManager;
    }

    public async Task<IActionResult> Index()
    {
        var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        // Знаходимо користувача за його Id
        var currentUser = await _userManager.FindByIdAsync(currentUserId);

        if (currentUser == null)
        {
            // Обробка ситуації, коли користувач не знайдений
            return NotFound();
        }

        // Отримуємо криптовалютні позиції поточного користувача
        var cryptocurrencies = _context.UserCryptoCurrencies
                                      .Where(uc => uc.UserId == currentUserId)
                                      .Include(uc => uc.CryptoCurrency)
                                      .ToList();

        return View(cryptocurrencies);
    }


    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }
    [HttpGet]
    public IActionResult Edit(int id)
    {
        var item = _context.Products.Find(id);
        if (item == null) return NotFound();
        ViewBag.Categories = _context.Categories.ToList();
        return View(item);
    }
    [HttpPost]
    public IActionResult Edit(Product model)
    {
        var existingProduct = _context.Products.Find(model.Id);
        if (existingProduct == null)
        {
            return NotFound("Product not found");
        }

        var category = _context.Categories.Find(model.CategoryId);
        if (category == null)
        {
            return NotFound("Category not found");
        }

        existingProduct.Name = model.Name;
        existingProduct.Description = model.Description;
        existingProduct.Location = model.Location;
        existingProduct.Contact = model.Contact;
        existingProduct.Price = model.Price;
        existingProduct.Quantity = model.Quantity;
        existingProduct.Discount = model.Discount;
        existingProduct.Status = model.Status;
        existingProduct.Category = category;

        _context.Products.Update(existingProduct); // Оновлення запису в базі даних
        _context.SaveChanges();

        return RedirectToAction(nameof(Index));
    }

    [HttpPost] // Використовуємо [HttpPost] для обробки даних форми при створенні продукту
    public IActionResult Create(Product model)
    {
        var category = _context.Categories.Find(model.CategoryId);
        if (category == null)
        {
            return NotFound("Category not found");
        }
        model.Category = category;
        if (model.ImageUrl == null)
        {
            model.ImageUrl = "https://static.vecteezy.com/system/resources/previews/004/141/669/original/no-photo-or-blank-image-icon-loading-images-or-missing-image-mark-image-not-available-or-image-coming-soon-sign-simple-nature-silhouette-in-frame-isolated-illustration-vector.jpg";
        }
        if (model.Name == null)
        {
            model.Name = "None";
        }
        _context.Products.Add(model);
        _context.SaveChanges();
        return RedirectToAction(nameof(Index));
    }

    public IActionResult Delete(int id)
    {
        var item = _context.Products.Find(id);
        if (item == null) return NotFound();

        if (item.Quantity <= 1)
        {
            _context.Products.Remove(item);
        }
        else
        {
            item.Quantity--;
        }
        _context.SaveChanges();

        return RedirectToAction(nameof(Index));
    }
}
