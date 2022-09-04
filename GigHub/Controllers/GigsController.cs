using Microsoft.AspNetCore.Mvc;

namespace GigHub.Controllers
{
    public class GigsController : Controller
    {
        // GET: Gigs
        public IActionResult Create()
        {
            return View();
        }

    }
}
