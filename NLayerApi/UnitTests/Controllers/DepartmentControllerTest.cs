using BusinessLayer.Interfaces;
using Common.Dto;
using Common.Helper;
using Common.Models;
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
    public class DepartmentControllerTests
    {
        private readonly IDepartmentService _departmentService;
        private readonly DepartmentController _controller;

        public DepartmentControllerTests()
        {
            _departmentService = A.Fake<IDepartmentService>();
            _controller = new DepartmentController(_departmentService);

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
        //public async Task GetDepartments_ReturnsOkResult_WithDepartments()
        //{
        //    // Arrange
        //    var departmentParams = new DepartmentParams();
        //    var departments = A.Fake<PagedList<DepartmentDto>>();
        //    A.CallTo(() => _departmentService.GetDepartmentAsync(departmentParams))
        //        .Returns(Task.FromResult(departments));
        //    A.CallTo(() => departments.MetaData).Returns(new MetaData());

        //    // Act
        //    var result = await _controller.GetDepartments(departmentParams);

        //    // Assert
        //    result.Should().NotBeNull();
        //    result.Result.Should().BeOfType<OkObjectResult>();
        //}

        [Fact]
        public async Task DepartmentController_CreateDepartment_ReturnsOkResult()
        {
            // Arrange
            var createDepartmentModel = new CreateDepartmentModel
            {
                DepartmentDto = new UpdateDepartmentDto
                {
                    DirectorateId = 1,
                    ContactId = 3,
                    Name = "Amstecdam",
                    ShortDescription = "Holand",
                    LeadContact = "string",
                    IsActive = true,
                    CompanyContactId = 22,
                    AddressId = 4
                },
                AddressDto = new AddressDto
                {
                    Address1 = "string",
                    Address2 = "string",
                    Address3 = "string",
                    PostCode = "string",
                    City = "string",
                    TownId = 0
                },
                CopyAddressFrom = "Organisation",
                DepartmentFullDescription = "Good",
                PhoneNumber = "43872",
                Fax = "123",
                Email = "holand@gmail.com"
            };
            var departmentDto = new DepartmentDto();
            A.CallTo(() => _departmentService.CreateDepartment(createDepartmentModel, "testuser"))
                .Returns(Task.FromResult(departmentDto));

            // Act
            var result = await _controller.CreateDepartment(createDepartmentModel);

            // Assert
            result.Should().NotBeNull();
            result.Result.Should().BeOfType<OkObjectResult>();

            var okResult = result.Result as OkObjectResult;
            okResult.Value.Should().BeEquivalentTo(departmentDto);
        }


        [Fact]
        public async Task DepartmentController_UpdateDepartment_ReturnsOkResult()
        {
            // Arrange
            var updateDepartmentModel = new UpdateDepartmentModel
            {
                DepartmentId = 13,
                DepartmentDto = new UpdateDepartmentDto
                {
                    DepartmentId = 13,
                    DirectorateId = 1,
                    ContactId = 3,
                    Name = "Matcova",
                    ShortDescription = "Russia",
                    LeadContact = "string",
                    IsActive = true,
                    CompanyContactId = 29,
                    AddressId = 1
                },
                AddressDto = new AddressDto
                {
                    AddressId = 0,
                    Address1 = "string",
                    Address2 = "string",
                    Address3 = "string",
                    PostCode = "string",
                    City = "string",
                    TownId = 0
                },
                CopyAddressFrom = "Organisation",
                DepartmentFullDescription = "Good",
                PhoneNumber = "12312",
                Fax = "4343",
                Email = "string@gmail.com"
            };
            var departmentDto = new DepartmentDto();
            A.CallTo(() => _departmentService.UpdateDepartment(updateDepartmentModel, "testuser"))
                .Returns(Task.FromResult(departmentDto));

            // Act
            var result = await _controller.UpdateDepartment(updateDepartmentModel);

            // Assert
            result.Should().NotBeNull();
            result.Result.Should().BeOfType<OkObjectResult>();

            var okResult = result.Result as OkObjectResult;
            okResult.Value.Should().BeEquivalentTo(departmentDto);
        }

    }
}