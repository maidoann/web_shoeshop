using System.Diagnostics;
using AspNetCoreGeneratedDocument;
using Microsoft.AspNetCore.Mvc;
using WEB_MQDD_SHOESHOP.Models;
using static WEB_MQDD_SHOESHOP.Models.ProductFullViewModel;

namespace WEB_MQDD_SHOESHOP.Controllers;

public class HomeController : Controller
{
    private readonly IHttpClientFactory _httpClientFactory;

    public HomeController(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<IActionResult> Index()
    {
        var client = _httpClientFactory.CreateClient("ApiClient");
        var response = await client.GetFromJsonAsync<ApiResponse<ProductFullViewModel>>("Home/GetProductsHome");

        if (response == null || response.Data == null)
        {
            return View("Index", new List<ProductFullViewModel>());
        }

        var products = response.Data; // List<ProductFullViewModel>
        return View("Index", products);
    }


    public async Task<IActionResult> ProductsShop()
    {
        var client = _httpClientFactory.CreateClient("ApiClient");
        var response = await client.GetFromJsonAsync<ApiResponse<ProductFullViewModel>>("Home/GetProductsShop");
        if(response == null || response.Data == null)
        {
            return View("~/Views/Home/ProductsShop.cshtml", new List<ProductFullViewModel>());

        }
        var products = response.Data;
        return View("~/Views/Home/ProductsShop.cshtml", products);

    }

    public async Task<IActionResult> ProductDetail(int id)
    {
        var client = _httpClientFactory.CreateClient("ApiClient");
        var response = await client.GetFromJsonAsync<ProductFullViewModel>($"Home/GetProduct/{id}");

        if (response == null )
        {
            // Nếu không tìm thấy, trả về view với một object rỗng hoặc view lỗi
            return View("~/Views/Home/ProductDetail.cshtml", new ProductFullViewModel());
        }

        return View("~/Views/Home/ProductDetail.cshtml", response); // Truyền một sản phẩm đơn
    }
    // Định nghĩa ApiResponse generic
    public class ApiResponse<T>
    {
        public List<T> Data { get; set; }
    }
}