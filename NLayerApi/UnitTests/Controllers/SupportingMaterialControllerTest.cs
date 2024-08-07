using BusinessLayer.Interfaces;
using Common.Dto;
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
    public class SupportingMaterialControllerTest
    {
        private readonly SupportingMaterialController _controller;
        private readonly ISupportingMaterialService _supportingMaterialServiceMock;
        private readonly HttpContext _httpContextMock;

        public SupportingMaterialControllerTest()
        {
            _supportingMaterialServiceMock = A.Fake<ISupportingMaterialService>();

            // Set up a mock HttpContext with an authenticated user
            _httpContextMock = new DefaultHttpContext();
            _httpContextMock.User = new ClaimsPrincipal(new ClaimsIdentity(new[]
            {
            new Claim(ClaimTypes.Name, "testuser")
        }, "mock"));

            _controller = new SupportingMaterialController(_supportingMaterialServiceMock)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = _httpContextMock
                }
            };
        }

        [Fact]
        public async Task SupportingMaterialController_UpdateSupportingMaterial_ReturnsOkResult()
        {
            // Arrange
            var updateSupportingMaterial = new UpdateSupporttingMaterial
            {
                Url = "tuandinhManh",
                Description = "Good",
                Type = "Movie3"
            };
            var materialId = 1;

            // Set up the mock to return true, indicating a successful update
            A.CallTo(() => _supportingMaterialServiceMock.UpdateSupportingMaterialAsync(materialId, updateSupportingMaterial, "testuser"))
                .Returns(Task.FromResult(true));

            // Act
            var result = await _controller.UpdateSupportingMaterial(materialId, updateSupportingMaterial);

            // Assert
            result.Should().NotBeNull();
            var okResult = result as OkObjectResult;
            okResult.Should().NotBeNull();
            okResult.StatusCode.Should().Be(200);
            okResult.Value.Should().Be("Update successfully");
        }

    }
}
