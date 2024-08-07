using BusinessLayer.Interfaces;
using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NLayerApi.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests.Controllers
{
    public class CompanyContactControllerTests
    {
        private readonly ICompanyContactService _companyContactService;

        public CompanyContactControllerTests()
        {
            _companyContactService = A.Fake<ICompanyContactService>();
        }

        [Fact]
        public async Task CompanyContactController_GetTypeOfBusinesses_ReturnOK()
        {
            // Arrange
            var types = new List<string>();
            A.CallTo(() => _companyContactService.GetTypeOfBusiness())
                .Returns(Task.FromResult(types));
            var controller = new CompanyContactController(_companyContactService);

            // Act
            var result = await controller.GetTypeOfBusinesses();

            // Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<OkObjectResult>();

            var okResult = result as OkObjectResult;
            okResult.Should().NotBeNull();
            okResult.Value.Should().BeEquivalentTo(new { types });
        }

        [Fact]
        public async Task CompanyContactController_GetSICCode_ReturnOK()
        {
            // Arrange
            var typeOfBusiness = "Type1";
            var sicCode = "12345";
            A.CallTo(() => _companyContactService.GetSICCode(typeOfBusiness))
                .Returns(Task.FromResult(sicCode));
            var controller = new CompanyContactController(_companyContactService);

            // Act
            var result = await controller.GetSICCode(typeOfBusiness);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<OkObjectResult>();

            var okResult = result as OkObjectResult;
            okResult.Should().NotBeNull();
            okResult.Value.Should().BeEquivalentTo(new { SICCode = sicCode });
        }

    }
    //public class CompanyContactControllerTest
    //{
    //    private readonly ICompanyContactService _companyContactService;
    //    public CompanyContactControllerTest(ICompanyContactService companyContactService)
    //    {
    //        _companyContactService = A.Fake<ICompanyContactService>();
    //    }

    //    [Fact]
    //    public async Task CompanyContactController_GetTypeOfBusinesses_ReturnOK()
    //    {
    //        // Arrange
    //        var types = new List<string>();
    //        A.CallTo(() => _companyContactService.GetTypeOfBusiness())
    //            .Returns(Task.FromResult(types));
    //        var controller = new CompanyContactController(_companyContactService);

    //        // Act
    //        var result = await controller.GetTypeOfBusinesses();

    //        // Assert
    //        result.Should().NotBeNull();
    //        result.Should().BeOfType<OkObjectResult>();

    //        var okResult = result as OkObjectResult;
    //        okResult.Should().NotBeNull();
    //        okResult.Value.Should().BeEquivalentTo(new { types });
    //    }

    //    [Fact]
    //    public async Task CompanyContactController_GetSICCode_ReturnOK()
    //    {
    //        // Arrange
    //        var typeOfBusiness = "Tech";
    //        var sicCode = "111";
    //        A.CallTo(() => _companyContactService.GetSICCode(typeOfBusiness))
    //            .Returns(Task.FromResult(sicCode));
    //        var controller = new CompanyContactController(_companyContactService);

    //        // Act
    //        var result = await controller.GetSICCode(typeOfBusiness);

    //        // Assert
    //        result.Should().NotBeNull();
    //        result.Should().BeOfType<OkObjectResult>();

    //        var okResult = result as OkObjectResult;
    //        okResult.Should().NotBeNull();
    //        okResult.Value.Should().BeEquivalentTo(new { SICCode = sicCode });
    //    }

    //}
}
