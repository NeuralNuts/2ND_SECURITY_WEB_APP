using _2ND_SECURITY_WEB_APP.Models;
using _2ND_SECURITY_WEB_APP.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace _2ND_SECURITY_WEB_APP.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly FileUploaderServices _uploader;

        public HomeController(ILogger<HomeController> logger, FileUploaderServices uploader)
        {
            _logger = logger;
            _uploader = uploader;
        }

        [HttpPost]
        public async Task<IActionResult> ImageUpload(IFormFile file)
        {
            await _uploader.SaveFile(file);
            return View("Encryption");
        }

        [HttpPost]
        public async Task<ActionResult> DownloadFile(string fileName)
        {
            byte[]? fileBytes = await _uploader.LoadEncryptedFile(fileName);

            if (fileBytes == null || fileBytes.Length == 0)
            {
                return RedirectToAction(nameof(Index));
            }

            return File(fileBytes, "application/octet-stream", fileDownloadName: fileName);
        }


        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Login() 
        {
            return View();
        }

        public IActionResult Logout() 
        {
            Response.Cookies.Delete("Cookie");
            Response.Cookies.Delete("Cookie");

            return View();
        }

        [Authorize(Roles = "guest")]
        public IActionResult Encryption()
        {
            return View();
        }

        [Authorize(Roles = "admin")]
        public IActionResult CRUD() 
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}