using Microsoft.AspNetCore.Mvc;
using System.Collections;
using System.Collections.Generic;
using TrevoDaSorteDigital.Dao;
using TrevoDaSorteDigital.Dao.Models;

namespace TrevoDaSorteDigital.Admin.Controllers
{
    public class UsuariosController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
