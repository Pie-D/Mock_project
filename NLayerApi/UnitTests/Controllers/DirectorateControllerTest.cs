using BusinessLayer.Interfaces;
using Common.Helper;
using CommonWeb.Dto;
using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NLayerApi.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests.Controllers
{
    public class DirectorateControllerTest
    {
        private readonly IDirectorateService _directorateService;
        private readonly DirectorateController _controller;

        public DirectorateControllerTest()
        {
            _directorateService = A.Fake<IDirectorateService>();
            _controller = new DirectorateController(_directorateService);

            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
            new Claim(ClaimTypes.Name, "testuser")
            }, "mock"));

            _controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = user }
            };
        }

        //[Fact]
        //public async Task GetDirectorates_ReturnsOkResult_WithDirectorates()
        //{
        //    // Arrange
        //    var directorateParams = new DirectorateParams();
        //    var directorates = A.Fake<PagedList<DirectorateDto>>();
        //    A.CallTo(() => _directorateService.GetDirectorateAsync(directorateParams))
        //        .Returns(Task.FromResult(directorates));
        //    A.CallTo(() => directorates.MetaData).Returns(new MetaData());

        //    // Act
        //    var result = await _controller.GetDirectorates(directorateParams);

        //    // Assert
        //    result.Should().NotBeNull();
        //    result.Result.Should().BeOfType<OkObjectResult>();
        //}

        [Fact]
        public async Task DirectorateController_AddDirectorate_ReturnsOkResult()
        {
            // Arrange
            var createDirectorate = new CreateDirectorateDto
            {
                Name = "Liver",
                ShortDescription = "string",
                LeadContact = "string",
                Address = new AddressDto
                {
                    Address1 = "",
                    Address2 = "",
                    Address3 = "",
                    PostCode = "",
                    City = "",
                    TownId = 0
                },
                CompanyContact = new CompanyContactDto
                {
                    PhoneNumber = "456",
                    Fax = "555",
                    Email = "Liver@gmail.com",
                    WebAddress = "",
                    CharityNumber = "666",
                    CompanyNumber = "999",
                    TypeOfBusiness = "",
                    SICCode = "",
                    FullDescription = "In England"
                },
                GetAddressFrom = "Organisation"
            };

            // Mocking the AddDirectorateAsync method to return true
            A.CallTo(() => _directorateService.AddDirectorateAsync(4, 1, createDirectorate, "testuser"))
                .Returns(Task.FromResult(true));

            // Act
            var result = await _controller.AddDirectorate(4, 1, createDirectorate);

            // Assert
            result.Should().NotBeNull();
            var okResult = result as OkObjectResult;
            okResult.Should().NotBeNull();
            okResult.StatusCode.Should().Be(200);
            okResult.Value.Should().Be("Create successfully");
        }


        //[Fact]
        //public async Task AddDirectorate_ReturnsOkResult_WhenSuccessful()
        //{
        //    // Arrange
        //    var createDirectorate = new CreateDirectorateDto
        //    {

        //    };
        //    A.CallTo(() => _directorateService.AddDirectorateAsync(4, 1, createDirectorate, "testuser"))
        //        .Returns(Task.FromResult(true));

        //    // Act
        //    var result = await _controller.AddDirectorate(4, 1, createDirectorate);

        //    // Assert
        //    result.Should().NotBeNull();
        //    result.Should().BeOfType<OkObjectResult>();
        //}

        [Fact]
        public async Task DirectorateController_UpdateDirectorate_ReturnsOkResult()
        {
            // Arrange
            var updateDirectorate = new UpdateDirectorateDto
            {
                DirectorateId = 10,
                Name = "Chease",
                ShortDescription = "string",
                LeadContact = "string",
                Address = new AddressDto
                {
                    Address1 = "LaiXa",
                    Address2 = "Troi",
                    Address3 = "DucThuong",
                    PostCode = "88",
                    City = "Ha Noi",
                    TownId = 1
                },
                CompanyContact = new CompanyContactDto
                {
                    PhoneNumber = "333",
                    Fax = "string",
                    Email = "string",
                    WebAddress = "string",
                    CharityNumber = "string",
                    CompanyNumber = "string",
                    TypeOfBusiness = "string",
                    SICCode = "string",
                    FullDescription = "string"
                },
                GetAddressFrom = "string",
                IsActive = true
            };

            // Mocking the UpdateDirectorateAsync method to return true
            A.CallTo(() => _directorateService.UpdateDirectorateAsync(4, 1, updateDirectorate, "testuser"))
                .Returns(Task.FromResult(true));

            // Act
            var result = await _controller.UpdateDirectorate(4, 1, updateDirectorate);

            // Assert
            result.Should().NotBeNull();
            var okResult = result as OkObjectResult;
            okResult.Should().NotBeNull();
            okResult.StatusCode.Should().Be(200);
            okResult.Value.Should().Be("Update successfully");
        }
        //[Fact]
        //public async Task UpdateDirectorate_ReturnsBadRequest_WhenUpdateFails()
        //{
        //    // Arrange
        //    var updateDirectorate = new UpdateDirectorateDto
        //    {
        //        DirectorateId = 8,
        //        Name = "Inter Milan",
        //        ShortDescription = "string",
        //        LeadContact = "string",
        //        Address = new AddressDto
        //        {
        //            Address1 = "LaiXa",
        //            Address2 = "Troi",
        //            Address3 = "DucThuong",
        //            PostCode = "88",
        //            City = "Ha Noi",
        //            TownId = 1
        //        },
        //        CompanyContact = new CompanyContactDto
        //        {
        //            PhoneNumber = "111",
        //            Fax = "222",
        //            Email = "milan@gmail.com",
        //            WebAddress = "milan.net",
        //            CharityNumber = "222",
        //            CompanyNumber = "333",
        //            TypeOfBusiness = "Tech",
        //            SICCode = "9899",
        //            FullDescription = "Good"
        //        },
        //        GetAddressFrom = "string",
        //        IsActive = false
        //    };

        //    // Mocking the UpdateDirectorateAsync method to return false
        //    A.CallTo(() => _directorateService.UpdateDirectorateAsync(8, 1, updateDirectorate, "testuser"))
        //        .Returns(Task.FromResult(false));

        //    // Act
        //    var result = await _controller.UpdateDirectorate(8, 1, updateDirectorate);

        //    // Assert
        //    result.Should().NotBeNull();
        //    var badRequestResult = result as BadRequestObjectResult;
        //    badRequestResult.Should().NotBeNull();
        //    badRequestResult.StatusCode.Should().Be(400);
        //    var problemDetails = badRequestResult.Value as ProblemDetails;
        //    problemDetails.Should().NotBeNull();
        //    problemDetails.Title.Should().Be("Problem updating directorate");
        //}

    }
}
