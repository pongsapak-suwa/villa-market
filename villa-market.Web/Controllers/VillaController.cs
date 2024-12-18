using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using villa_market.Domain.Entities;
using villa_market.Infrastructure.Data;

namespace villa_market.Web.Controllers
{
    [Authorize]
    public class VillaController : Controller
    {
        private readonly ApplicationDbContext _villaService;
        public VillaController(ApplicationDbContext villaService)
        {
            _villaService = villaService;
        }

        public IActionResult Index()
        {
            var villas = _villaService.Villas.ToList();
            return View(villas);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Villa obj)
        {

            if (obj.Name == obj.Description)
            {
                ModelState.AddModelError("name", "The description cannot exactly match the Name.");
            }
            if (ModelState.IsValid)
            {

                _villaService.Add(obj);
                _villaService.SaveChanges();
                TempData["success"] = "The villa has been created successfully.";
                return RedirectToAction(nameof(Index));
            }
            return View();
        }

    }
}
