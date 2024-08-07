using BusinessLayer.Interfaces;
using BusinessLayer.Services;
using CommonWeb.Dto;
using DataAccess.Entities;
using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NLayerApi.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests.Controllers
{
    public class OrganisationControllerTest
    {
        private readonly OrganisationsController _controller;
        private readonly IOrganisationService _organisationServiceMock;
        private readonly GeoLocationService _geoLocationServiceMock;
        private readonly IRegionService _regionServiceMock;
        private readonly HttpContext _httpContextMock;

        public OrganisationControllerTest()
        {
            _organisationServiceMock = A.Fake<IOrganisationService>();
            _geoLocationServiceMock = A.Fake<GeoLocationService>();
            _regionServiceMock = A.Fake<IRegionService>();

            // Set up a mock HttpContext with an authenticated user
            _httpContextMock = new DefaultHttpContext();
            _httpContextMock.User = new ClaimsPrincipal(new ClaimsIdentity(new[]
            {
            new Claim(ClaimTypes.Name, "testuser")
        }, "mock"));

            _controller = new OrganisationsController(_organisationServiceMock, _geoLocationServiceMock, _regionServiceMock)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = _httpContextMock
                }
            };
        }

        [Fact]
        public async Task OrganisationController_CreateOrganisation_ReturnsOkResult()
        {
            // Arrange
            var createOrganisationDto = new CreateOrganisationDto
            {
                OrgName="Abc",
                ShortDescription="string",
                LeadContact="Tuan",
                PreferredOrganisation="string",
                ExpressionOfInternet="string",
                Address=new AddressDto
                {
                    Address1="Phung",
                    Address2="Troi",
                    Address3="Tho",
                    PostCode="123",
                    City="Dan Phuong",
                    TownId=2
                },
                CompanyContact=new CompanyContactDto
                {
                    PhoneNumber="1234",
                    Fax="12",
                    Email="string@gmail.com",
                    CharityNumber="1234",
                    CompanyNumber = "12432",
                    TypeOfBusiness="Business",
                    SICCode="123",
                    FullDescription="string"
                }
            };
            var contactId = 1;
            A.CallTo(() => _organisationServiceMock.CreateOrganisationAsync(contactId, createOrganisationDto, "testuser"))
                .Returns(Task.FromResult(true));

            // Act
            var result = await _controller.CreateOrganisation(contactId, createOrganisationDto);

            // Assert
            result.Should().NotBeNull();
            var okResult = result as OkObjectResult;
            okResult.Should().NotBeNull();
            okResult.StatusCode.Should().Be(200);
            okResult.Value.Should().Be("Create successfully");
        }
    }
}
