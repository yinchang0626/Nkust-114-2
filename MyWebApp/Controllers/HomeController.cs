using System.Diagnostics;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using MyWebApp.Models;

namespace MyWebApp.Controllers;

public class HomeController : Controller
{
    public IActionResult Index()
    {
        var path = Path.Combine(Directory.GetCurrentDirectory(), "Data", "data.json");

        if (!System.IO.File.Exists(path))
        {
            return Content("找不到 data.json");
        }

        try
        {
            var json = System.IO.File.ReadAllText(path);

            var options = new JsonSerializerOptions
            {
                NumberHandling = JsonNumberHandling.AllowReadingFromString
            };

            var data = JsonSerializer.Deserialize<List<DataItem>>(json, options);

            return View(data ?? new List<DataItem>());
        }
        catch (Exception ex)
        {
            return Content("JSON 解析失敗：\n" + ex.Message);
        }
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel
        {
            RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
        });
    }
}