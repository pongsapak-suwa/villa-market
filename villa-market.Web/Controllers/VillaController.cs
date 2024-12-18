using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

        public async Task<IActionResult> Index()
        {
            var villas = await _villaService.Villas.ToListAsync();
            return View(villas);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Villa obj)
        {
            if (obj.Name == obj.Description)
            {
                ModelState.AddModelError("name", "The description cannot exactly match the Name.");
            }
            if (ModelState.IsValid)
            {
                await _villaService.AddAsync(obj);
                await _villaService.SaveChangesAsync();
                TempData["success"] = "The villa has been created successfully.";
                return RedirectToAction(nameof(Index));
            }
            return View(obj);
        }

        public async Task<IActionResult> Update(int villaId)
        {
            if (ModelState.IsValid)
            {
                var obj = await _villaService.Villas.FirstOrDefaultAsync(u => u.Id == villaId);
                if (obj == null)
                {
                    return RedirectToAction("Error", "Home");
                }
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Update(Villa obj)
        {
            if (ModelState.IsValid && obj.Id > 0)
            {
                _villaService.Update(obj);
                await _villaService.SaveChangesAsync();
                TempData["success"] = "The villa has been updated successfully.";
                return RedirectToAction(nameof(Index));
            }
            return View(obj);
        }

        public async Task<IActionResult> Delete(int villaId)
        {
            if (ModelState.IsValid)
            {
                Villa? obj = await _villaService.Villas.FirstOrDefaultAsync(u => u.Id == villaId);
                if (obj is null)
                {
                    TempData["error"] = "Villa not found.";
                    return RedirectToAction(nameof(Index));
                }
                return View(obj);
            }
            else
            {
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        public async Task<IActionResult> Delete(Villa obj)
        {
            if (!ModelState.IsValid)
            {
                var objFromDb = await _villaService.Villas.FirstOrDefaultAsync(u => u.Id == obj.Id);
                if (objFromDb is null)
                {
                    TempData["error"] = "Villa not found.";
                    return RedirectToAction(nameof(Index));
                }

                _villaService.Villas.Remove(obj);
                await _villaService.SaveChangesAsync();
                TempData["success"] = "The villa has been deleted successfully.";
            }
            return View();
        }


    }
}
