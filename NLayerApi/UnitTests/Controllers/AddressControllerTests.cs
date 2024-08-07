using BusinessLayer.Interfaces;
using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NLayerApi.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests.Controllers
{
    //public class AddressControllerTests
    //{
    //    private readonly Mock<IAddressService> _addressServiceMock;

    //    public AddressControllerTests()
    //    {
    //        _addressServiceMock = new Mock<IAddressService>();
    //    }

    //    [Fact]
    //    public async Task AddressController_GetPostCode_ReturnsOk()
    //    {
    //        // Arrange
    //        var listPostCode = new List<string> { "12345", "67890" };
    //        _addressServiceMock.Setup(service => service.GetListPostCode())
    //            .ReturnsAsync(listPostCode);

    //        var controller = new AddressController(_addressServiceMock.Object);

    //        // Act
    //        var result = await controller.GetPostCode();

    //        // Assert
    //        result.Should().NotBeNull();
    //        result.Should().BeOfType<OkObjectResult>();

    //        var okResult = result as OkObjectResult;
    //        okResult.Should().NotBeNull();
    //        okResult.Value.Should().BeEquivalentTo(new { listPostCode });
    //    }
    //}
    //public class AddressControllerTests
    //{
    //    private readonly IAddressService _addressService;
    //    public AddressControllerTests()
    //    {
    //        _addressService = A.Fake<IAddressService>();
    //    }
    //    [Fact]
    //    public async void AddressController_GetPostCode_ReturnOK()
    //    {
    //        //Arrange
    //        //var postcode=
    //        var listPostCode = A.Fake<List<string>>();
    //        A.CallTo(() => _addressService.GetListPostCode()).Returns(Task.FromResult(listPostCode));
    //        var controller = new AddressController(_addressService);

    //        //Act
    //        var result =await controller.GetListPostCode();

    //        //Assert
    //        result.Should().NotBeNull();
    //        result.Should().BeOfType(typeof(OkObjectResult));
    //    }
    //}

    using System.Collections.Generic;
    using System.Threading.Tasks;
    using BusinessLayer.Interfaces;
    using Microsoft.AspNetCore.Mvc;
    using Moq;
    using Xunit;
    using FluentAssertions;

    public class AddressControllerTests
    {
        private readonly IAddressService _addressService;

        public AddressControllerTests()
        {
            _addressService = A.Fake<IAddressService>();
        }

        [Fact]
        public async Task AddressController_GetPostCode_ReturnOK()
        {
            // Arrange
            var listPostCode = new List<string>();
            A.CallTo(() => _addressService.GetListPostCode())
                .Returns(Task.FromResult(listPostCode));
            var controller = new AddressController(_addressService);

            // Act
            var result = await controller.GetPostCode();

            // Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<OkObjectResult>();
        }
        // [Fact]
        // public async Task AddressController_CreateAddress_ReturnsOkResult()
        // {
    }

}
