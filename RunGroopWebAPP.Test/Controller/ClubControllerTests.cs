using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RunGroopWebApp.Controllers;
using RunGroopWebApp.Interfaces;
using RunGroopWebApp.Models;

namespace RunGroopWebAPP.Test.Controller
{
    public class ClubControllerTests
    {
        private readonly IClubRepository _clubRepository;
        private readonly IPhotoService _photoServices;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ClubController _clubController;

        public ClubControllerTests()
        {
            _clubRepository = A.Fake<IClubRepository>();
            _photoServices = A.Fake<IPhotoService>();
            _httpContextAccessor = A.Fake<IHttpContextAccessor>();

            //SUT --
            _clubController = new ClubController(_clubRepository, _photoServices, _httpContextAccessor);
        }

        [Fact]
        public void ClubController_Index_ReturnSuccess()
        {
            //Arrange
            var clubs = A.Fake<IEnumerable<Club>>();
            A.CallTo(()=> _clubRepository.GetAll()).Returns(clubs);
            //Act
            var result = _clubController.Index();

            //Assert
            result.Should().BeOfType<Task<IActionResult>>();

        }
        [Fact]
        public void ClubController_Detail_ReturnSuccess()
        {
            //Arrange
            var id = 1;
            var club = A.Fake<Club>();
            A.CallTo(() => _clubRepository.GetByIdAsync(id)).Returns(club);

            //Act
            var result = _clubController.Detail(id);
            //Assert
            result.Should().BeOfType<Task<IActionResult>>();
        }

    }
}
