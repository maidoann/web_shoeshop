using Microsoft.AspNetCore.Mvc;
using WEB_MQDD_SHOESHOP.Models;

namespace WEB_MQDD_SHOESHOP.Controllers
{
    public class AuthController : Controller
    {
        public IActionResult Login()
        {
            return View("~/Views/Auth/Login.cshtml");
        }
        public IActionResult Register()
        {
            return View("~/Views/Auth/Register.cshtml");
        }

        //public async Task<IActionResult> AccountSave(string name, string email, string password, string confirmPassword)
        //{

        //}
    }
}
