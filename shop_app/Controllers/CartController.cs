using Microsoft.AspNetCore.Mvc;
using shop_app.Models;
using shop_app.Extensions;
using Microsoft.EntityFrameworkCore;

namespace shop_app.Controllers
{
    public class CartController : Controller
    {
        // Используем сессию для хранения корзины
        private const string CartSessionKey = "Cart";

        private readonly ProductContext _context;

        public CartController(ProductContext context)
        {
            _context = context;
        }

        // Получение корзины из сессии
        private ShoppingCart GetCart()
        {
            var cart = HttpContext.Session.GetObjectFromJson<ShoppingCart>(CartSessionKey);
            if (cart == null)
            {
                cart = new ShoppingCart();
            }
            return cart;
        }

        // Сохранение корзины в сессии
        private void SaveCart(ShoppingCart cart)
        {
            HttpContext.Session.SetObjectAsJson(CartSessionKey, cart);
        }

        // Добавление товара в корзину
        public async Task<IActionResult> AddToCart(int id)
        {
            var cart = GetCart();

            // Получаем продукт из базы данных
            var product = await _context.Products
                                         .FirstOrDefaultAsync(p => p.Id == id);

            if (product == null)
            {
                return NotFound();
            }

            // Проверка, есть ли товар в корзине
            var existingItem = cart.Items.FirstOrDefault(i => i.ProductId == id);
            if (existingItem != null)
            {
                // Увеличиваем количество товара, если это не превышает доступное количество
                if (existingItem.Quantity < product.Quantity)
                {
                    existingItem.Quantity++;
                }
            }
            else
            {
                cart.Items.Add(new CartItem
                {
                    ProductId = product.Id,
                    ProductName = product.Name,
                    Price = product.Price,
                    Quantity = 1,
                    MaxQuantity = product.Quantity // Передаем максимальное количество товара
                });
            }

            SaveCart(cart);
            return RedirectToAction("Index");
        }

        // Обновление количества товара в корзине
        public async Task<IActionResult> UpdateQuantity(int productId, int quantity)
        {
            var cart = GetCart();

            // Получаем продукт из базы данных
            var product = await _context.Products
                                         .FirstOrDefaultAsync(p => p.Id == productId);

            if (product == null)
            {
                return NotFound();
            }

            var item = cart.Items.FirstOrDefault(i => i.ProductId == productId);

            if (item != null)
            {
                // Если новое количество товара больше 0 и не превышает доступное количество
                if (quantity > 0 && quantity <= product.Quantity)
                {
                    item.Quantity = quantity;
                }
            }

            SaveCart(cart);
            return RedirectToAction("Index");
        }

        // Удаление товара из корзины
        public IActionResult RemoveFromCart(int productId)
        {
            var cart = GetCart();
            var item = cart.Items.FirstOrDefault(i => i.ProductId == productId);

            if (item != null)
            {
                cart.Items.Remove(item);
            }

            SaveCart(cart);
            return RedirectToAction("Index");
        }

        public IActionResult Index()
        {
            var cart = GetCart();
            return View(cart);
        }
        [HttpPost]
        public async Task<IActionResult> Checkout()
        {
            var cart = GetCart();

            foreach (var item in cart.Items)
            {
                var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == item.ProductId);
                if (product == null)
                {
                    return BadRequest("Not enough stock for item: " + product.Name);
                }
                else if (product.Quantity == item.Quantity)
                {
                    _context.Products.Remove(product);
                }
                else
                {
                    product.Quantity -= item.Quantity;
                }
                _context.SaveChanges();
            }
            return RedirectToAction("Index","Products");
        }
    }
}
