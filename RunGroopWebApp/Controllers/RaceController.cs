using Microsoft.AspNetCore.Mvc;
using RunGroopWebApp.Data;
using RunGroopWebApp.Interfaces;
using RunGroopWebApp.Models;

namespace RunGroopWebApp.Controllers
{
    public class RaceController : Controller
    {
        private readonly IRaceRepoistory _raceRepoistory;

        public RaceController(ApplicationDbContext context, IRaceRepoistory raceRepoistory)
        {
            _raceRepoistory = raceRepoistory;
        }
        public async Task<IActionResult> Index()
        {
            IEnumerable<Race> races =await _raceRepoistory.GetAll();    
            return View(races);
        }
        public async Task<IActionResult> Detail(int id)
        {
            Race race = await _raceRepoistory.GetByIdAsync(id);
            return View(race);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Race race)
        {
            if (!ModelState.IsValid)
            {
                return View(race);
            }
            _raceRepoistory.Add(race);
            return RedirectToAction("Index");
        }
    }
}
