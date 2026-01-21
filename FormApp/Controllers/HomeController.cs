using Microsoft.AspNetCore.Mvc;
using FormApp.Models;

namespace FormApp.Controllers;

using Microsoft.AspNetCore.Mvc;


public class HomeController : Controller
{
    public IActionResult Index()
    {
        // ÖNEMLİ: 'new()' yerine 'Repository.Products' kullanıyoruz
        var products = Repository.Products; 
        return View(products);
    }
    public IActionResult Privacy()
    {
        return View();
    }
}