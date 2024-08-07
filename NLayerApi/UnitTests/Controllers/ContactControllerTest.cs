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
    public class ContactControllerTests
    {
        private readonly IContactService _contactService;

        public ContactControllerTests()
        {
            _contactService = A.Fake<IContactService>();
        }

        [Fact]
        public async Task ContactController_GetListLeadContact_ReturnOK()
        {
            // Arrange
            var listLeadContact = new List<string> { "Contact1", "Contact2" };
            A.CallTo(() => _contactService.GetAllLeadContacts())
                .Returns(Task.FromResult(listLeadContact));
            var controller = new ContactController(_contactService);

            // Act
            var result = await controller.GetListLeadContact();

            // Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<OkObjectResult>();

            var okResult = result as OkObjectResult;
            okResult.Should().NotBeNull();
            okResult.Value.Should().BeEquivalentTo(new { listLeadContact });
        }
    }
}
