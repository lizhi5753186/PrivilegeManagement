using System.Web.Mvc;
using LH.Application.ServiceContract;
using Microsoft.AspNet.Identity;

namespace LH.Web.API.Controllers
{
    public class HomeController : Controller
    {
        private readonly IUserService _userService;

        public HomeController(IUserService userService)
        {
            _userService = userService;
        }

        // GET: Home
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Login()
        {
            return View();
        }
        public ActionResult UnLogin()
        {
            return Redirect("Login");
        }

        [HttpPost]
        public ActionResult Login(string username, string password)
        {
            if(_userService.Exist(username, password))
                return RedirectToAction("Index", "Home");
            else
                return View("error");
        }

        public ActionResult Html(string dir, string name)
        {
            var html = string.Format("~/App/views/{0}/{1}.html", dir, name);
            if (!System.IO.File.Exists(Server.MapPath(html)))
                return Content(string.Format("当前请求的页面“{0}”不存在！", html));
            return File(html, "text/html; charset=utf-8");
        }
    }
}