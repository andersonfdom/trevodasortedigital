using Microsoft.AspNetCore.Mvc;

namespace TrevoDaSorteDigital.Admin.Controllers
{
    public class ClientesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Dados(int id)
        {
            return View();
        }
    }
}
