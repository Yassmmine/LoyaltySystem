using LoyaltySystemApplication.Services.Interfaces;
using LoyaltySystemDomain.Common;
using LoyaltySystemDomain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LoyaltySystemAPI.Controllers
{
    
    [ApiController]
    public class PointsController : ControllerBase
    {

        private readonly IPointsService _pointsService;
        public PointsController(IPointsService pointsService)
        {
            _pointsService = pointsService;
        }
		[Route(RouteClass.Point.AddPoint)]
		[HttpPost]
        public async Task<IActionResult> EarnPoints([FromBody] EarnPointsRequest request)
        {
            return Ok(await _pointsService.EarnPoints( request));
        }

		[Route(RouteClass.Point.GetPoint)]
        [HttpGet]
        public async Task<IActionResult> GetPoint(int userId)
        {
            return Ok(await _pointsService.GetPoint(userId));
        }
		[Route("test")]
        [HttpGet]
        public async Task<IActionResult> Test(int userId)
        {
            return Ok("i am authorized");
        }
    }
}
