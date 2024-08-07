using BusinessLayer.Interfaces;
using Common.Dto;
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
    public class TeamControllerTest
    {
        private readonly TeamController _controller;
        private readonly ITeamService _teamServiceMock;
        private readonly HttpContext _httpContextMock;

        public TeamControllerTest()
        {
            _teamServiceMock = A.Fake<ITeamService>();

            // Set up a mock HttpContext with an authenticated user
            _httpContextMock = new DefaultHttpContext();
            _httpContextMock.User = new ClaimsPrincipal(new ClaimsIdentity(new[]
            {
            new Claim(ClaimTypes.Name, "testuser")
        }, "mock"));

            _controller = new TeamController(_teamServiceMock)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = _httpContextMock
                }
            };
        }

        [Fact]
        public async Task TeamController_AddTeam_ReturnsOkResult()
        {
            // Arrange
            var createTeam = new CreateTeamDto
            {
                Name = "Team 5",
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
                    PhoneNumber = "555",
                    Fax = "454",
                    Email = "anfield",
                    WebAddress = "anfield.com",
                    CharityNumber = "777",
                    CompanyNumber = "667",
                    TypeOfBusiness = "Tech",
                    SICCode = "900",
                    FullDescription = "Good"
                },
                GetAddressFrom = "Organisation"
            };

            var departmentId = 1;
            var contactId = 2;

            // Set up the mock to return true, indicating a successful addition
            A.CallTo(() => _teamServiceMock.AddTeamAsync(departmentId, contactId, createTeam, "testuser"))
                .Returns(Task.FromResult(true));

            // Act
            var result = await _controller.AddTeam(departmentId, contactId, createTeam);

            // Assert
            result.Should().NotBeNull();
            var okResult = result as OkObjectResult;
            okResult.Should().NotBeNull();
            okResult.StatusCode.Should().Be(200);
            okResult.Value.Should().Be("Create successfully");
        }


        //[Fact]
        //public async Task UpdateTeam_ReturnsOkResult_WhenSuccessful()
        //{
        //    // Arrange
        //    var updateTeam = new UpdateTeamDto
        //    {
        //        // Populate with necessary properties
        //    };
        //    var departmentId = 1;
        //    var contactId = 2;
        //    A.CallTo(() => _teamServiceMock.UpdateTeamAsync(departmentId, contactId, updateTeam, "testuser"))
        //        .Returns(Task.FromResult(true));

        //    // Act
        //    var result = await _controller.UpdateTeam(departmentId, contactId, updateTeam);

        //    // Assert
        //    result.Should().NotBeNull();
        //    var okResult = result as OkObjectResult;
        //    okResult.Should().NotBeNull();
        //    okResult.StatusCode.Should().Be(200);
        //    okResult.Value.Should().Be("Update successfully");
        //}

        [Fact]
        public async Task TeamController_UpdateTeam_ReturnsOkResult()
        {
            // Arrange
            var updateTeam = new UpdateTeamDto
            {
                TeamId = 16,
                Name = "Team 14",
                ShortDescription = "Inter Milan",
                LeadContact = "Inter Milan",
                Address = new AddressDto
                {
                    Address1 = "string",
                    Address2 = "string",
                    Address3 = "string",
                    PostCode = "string",
                    City = "string",
                    TownId = 0
                },
                CompanyContact = new CompanyContactDto
                {
                    PhoneNumber = "57342",
                    Fax = "5742",
                    Email = "inter@gmail.com",
                    WebAddress = "inter.com",
                    CharityNumber = "123234",
                    CompanyNumber = "12234",
                    TypeOfBusiness = "Automatic",
                    SICCode = "123129",
                    FullDescription = "Good"
                },
                GetAddressFrom = "Organisation",
                IsActive = true
            };
            var departmentId = 1;
            var contactId = 2;

            // Set up the mock to return true, indicating a successful update
            A.CallTo(() => _teamServiceMock.UpdateTeamAsync(departmentId, contactId, updateTeam, "testuser"))
                .Returns(Task.FromResult(true));

            // Act
            var result = await _controller.UpdateTeam(departmentId, contactId, updateTeam);

            // Assert
            result.Should().NotBeNull();
            var okResult = result as OkObjectResult;
            okResult.Should().NotBeNull();
            okResult.StatusCode.Should().Be(200);
            okResult.Value.Should().Be("Update successfully");
        }

    }
}