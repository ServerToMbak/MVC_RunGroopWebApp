using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Mvc;
using RunGroopWebApp.Interfaces;
using RunGroopWebApp.Models;
using RunGroopWebApp.ViewModels;

namespace RunGroopWebApp.Controllers
{
    public class DashboardController : Controller
    {
        private readonly IDashboardRepository _dashboardRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IPhotoService _photoService;

        public DashboardController(IDashboardRepository dashboardRepository, IHttpContextAccessor httpContextAccessor, IPhotoService photoService)
        {
            _dashboardRepository = dashboardRepository;
            _httpContextAccessor = httpContextAccessor;
            _photoService = photoService;
        }
        public async Task<IActionResult> Index()
        {
            var userRaces =await _dashboardRepository.GetAllUserRaces();
            var userClubs =await _dashboardRepository.GetAllUserClubs();
            var dashboardViewModel = new DashboardViewModel
            {
                Clubs = userClubs,
                Races = userRaces
            };
            return View(dashboardViewModel);
        }

        public async Task<IActionResult> EditUserProfile()
        {
            var curUserId = _httpContextAccessor.HttpContext.User.GetUserId();
            AppUser user  = await _dashboardRepository.GetUserById(curUserId);
            if(user == null)  return View("Error");

            var editUserViewModel = new EditUserDashboardViewModel
            {
                Id = curUserId,
                Pace = user.Pace,
                Mileage = user.Mileage,
                ProfileImageUrl = user.ProfileImageUrl,
                City = user.City,
                State = user.State
            };
            return View(editUserViewModel);
        }
        private void MapUserEdit(AppUser user, EditUserDashboardViewModel editVM, ImageUploadResult photoResult)
        {
            user.Id = editVM.Id;
            user.Pace = editVM.Pace;
            user.Mileage = editVM.Mileage;
            user.ProfileImageUrl = photoResult.Url.ToString();
            user.City = editVM.City;
            user.State = editVM.State;
        }


        [HttpPost]
        public async Task<IActionResult> EditUserProfile(EditUserDashboardViewModel editVM)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Failed to edit profile");
                return View("EditUserProfile",editVM);
            }
            AppUser user = await _dashboardRepository.GetByIdNoTracking(editVM.Id);

            if(user.ProfileImageUrl == "" || user.ProfileImageUrl ==null)
            {
                var photoResult =await _photoService.AddPhotoAsync(editVM.Image);

                MapUserEdit(user, editVM, photoResult);

                _dashboardRepository.Update(user);
                return RedirectToAction("Index");
            }
            else
            {
                try
                {
                    await _photoService.DeletePhotoAsync(user.ProfileImageUrl);
                }catch (Exception ex) 
                {
                    ModelState.AddModelError("", "Could not delete photo");
                    return View();
                }

                var photoResult = await _photoService.AddPhotoAsync(editVM.Image);

                MapUserEdit(user, editVM, photoResult);

                _dashboardRepository.Update(user);
                return RedirectToAction("Index");
            }
            }
    }
}
