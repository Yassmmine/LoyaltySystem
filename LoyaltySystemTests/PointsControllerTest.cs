using System.Threading.Tasks;
using FluentAssertions;
using LoyaltySystemAPI.Controllers;
using LoyaltySystemApplication.Services.Interfaces;
using LoyaltySystemDomain.Common;
using LoyaltySystemDomain.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace LoyaltySystemTest
{
    public class PointsControllerTest
    {
        private readonly Mock<IPointsService> _pointsServiceMock;
        private readonly PointsController _controller;

        public PointsControllerTest()
        {
            _pointsServiceMock = new Mock<IPointsService>();
            _controller = new PointsController(_pointsServiceMock.Object);
        }

        [Fact]
        public async Task EarnPointsTest()
        {
            var request = new EarnPointsRequest { Points = 10,UserId=1 };
            var serviceResponse = new ServiceResponse<bool> { Data = true, Success = true };
            _pointsServiceMock.Setup(service => service.EarnPoints(It.IsAny<EarnPointsRequest>()))
                .ReturnsAsync(serviceResponse);

            var result = await _controller.EarnPoints(request);

            result.Should().BeOfType<OkObjectResult>();
            var okResult = result as OkObjectResult;
            okResult.Value.Should().BeEquivalentTo(serviceResponse);
        }

        [Fact]
        public async Task GetPointTest()
        {
            var serviceResponse = new ServiceResponse<List<PointViewModel>> { Data = new List<PointViewModel>(), Success = true };
            _pointsServiceMock.Setup(service => service.GetPoint(It.IsAny<int>()))
                .ReturnsAsync(serviceResponse);

            var result = await _controller.GetPoint(1);

            result.Should().BeOfType<OkObjectResult>();
            var okResult = result as OkObjectResult;
            okResult.Value.Should().BeEquivalentTo(serviceResponse);
        }
    }
}