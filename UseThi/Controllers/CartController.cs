using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Crypto.Utilities;
using System.Text.Json;
using Data.Data;
using UseThi.Extensions;
using UseThi.Models;

namespace UseThi.Controllers
{
    public class CartController : Controller
    {
        private readonly ShopDbContext context;
        private readonly IMapper mapper;
        public CartController(ShopDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public IActionResult Index()
        {
            var ids = HttpContext.Session.Get<List<int>>("cart");
            if (ids == null)
                ids = new List<int>();

            var entities = context.Products.Where(x => ids.Contains(x.Id)).ToList();
            var list = mapper.Map<List<ProductCartModel>>(entities);

            return View(list);
        }

        public IActionResult Append(int id)
        {
            var ids = HttpContext.Session.Get<List<int>>("cart");
            if (ids == null) ids = new List<int>();

            ids.Add(id);

            HttpContext.Session.Set("cart", ids);

            return RedirectToAction("Index", "Home");
        }

        public IActionResult Remove(int id)
        {
            return View();
        }

        public IActionResult Clear()
        {
            return View();
        }
    }

}
