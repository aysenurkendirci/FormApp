using Microsoft.AspNetCore.Mvc;
using FormApp.Models;

namespace FormApp.Controllers;

using Microsoft.AspNetCore.Mvc;


public class HomeController : Controller
{
    public IActionResult Index()
    {
        // Şimdilik boş liste gönderiyoruz
        List<Product> products = new();
        return View(products);
    }

    public IActionResult Privacy()
    {
        return View();
    }
}