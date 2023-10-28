using Microsoft.AspNetCore.Mvc;
using RunGroopWebApp.Data;
using RunGroopWebApp.Interfaces;
using RunGroopWebApp.Models;
using RunGroopWebApp.Repository;
using RunGroopWebApp.Services;
using RunGroopWebApp.ViewModels;

namespace RunGroopWebApp.Controllers
{
    public class RaceController : Controller
    {
        private readonly IRaceRepoistory _raceRepoistory;
        private readonly IPhotoService _photoService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public RaceController(IRaceRepoistory raceRepoistory, IPhotoService photoService, IHttpContextAccessor httpContextAccessor)
        {
            _raceRepoistory = raceRepoistory;
            _photoService = photoService;
            _httpContextAccessor = httpContextAccessor;
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
            var curUserId = _httpContextAccessor.HttpContext.User.GetUserId();
            var createRaceViewModel = new CreateRaceViewModel { AppUserId = curUserId};
            return View(createRaceViewModel);
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateRaceViewModel raceVM)
        {
            if (ModelState.IsValid)
            {
                var result = await _photoService.AddPhotoAsync(raceVM.Image);
                var Race = new Race
                {
                    Title = raceVM.Title,
                    Description = raceVM.Description,
                    Image = result.Url.ToString(),
                    AppUserId = raceVM.AppUserId,
                    Address = new Address
                    {
                        City = raceVM.Address.City,
                        State = raceVM.Address.State,
                        Street = raceVM.Address.Street,
                    }
                };
                _raceRepoistory.Add(Race);
                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError("", "Photo upload failed");
            }
            return View(raceVM);
        }
        
        public async Task<IActionResult> Edit(int id)
        {
            var race =await _raceRepoistory.GetByIdAsync(id);
            if (race == null) return View("Error");
            var raceVM = new EditRaceViewModel
            {
                Title = race.Title,
                Description = race.Description,
                AddressId = race.AddressId,
                Address = race.Address,
                RaceCategory = race.RaceCategory,
            };
            return View(raceVM);    
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, EditRaceViewModel raceVM)
        {
            if(!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Failed to edit Race");
                return View("Edit");
            }
            var userClub = await _raceRepoistory.GetByIdAsyncNoTracking(id);
            if (userClub != null)
            {
                try
                {
                    await _photoService.DeletePhotoAsync(userClub.Image);
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Could not delete phone");
                    return View(raceVM);
                }
                var photoResult = await _photoService.AddPhotoAsync(raceVM.Image);

                var race = new Race
                {
                    Id = id,
                    Title = raceVM.Title,
                    Description = raceVM.Description,
                    Image = photoResult.Url.ToString(),
                    AddressId = raceVM.AddressId,
                    Address = raceVM.Address,

                };

                _raceRepoistory.Update(race);

                return RedirectToAction("Index");
            }
            else
            {
                return View(raceVM);
            }
        }

        public async Task<IActionResult> Delete(int id)
        {
            var raceDetails = await _raceRepoistory.GetByIdAsync(id);
            if(raceDetails == null) return View("Error");  
            return View(raceDetails);
        }
        [HttpPost, ActionName("delete")]
        public async Task<IActionResult> DeleteClub(int id)
        {
            var raceDetails =await _raceRepoistory.GetByIdAsync(id);
            if (raceDetails == null) return View("Error");

            _raceRepoistory.Delete(raceDetails);
            return RedirectToAction("Index");   
        }
    }
}
