using Azure.Core;
using LoyaltySystemApplication.Services.Interfaces;
using LoyaltySystemDomain.Common;
using LoyaltySystemDomain.Entities;
using LoyaltySystemDomain.Models;
using LoyaltySystemInfrastructures.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace LoyaltySystemApplication.Services.implementation
{
	public class PointsService : ServiceBase, IPointsService

	{
		private readonly IUnitOfWork _unitOfWork;
		public PointsService(IUnitOfWork unitOfWork, IDistributedCache distributedCache) : base(distributedCache)
		{
			_unitOfWork = unitOfWork;
		}

		public async Task<ServiceResponse<bool>> EarnPoints(EarnPointsRequest request)
		{
			try
			{
				var cacheKey = $"UserPoints_{request.UserId}";

				string cachedPoints = await GetStringAsync(cacheKey);
				if (!_unitOfWork.UserRepository.Any(a => a.Id == request.UserId))
					return new ServiceResponse<bool>() { Data = false, Success = false, Message = "User Not Found" };

				var point = new Point { UserId = request.UserId, Value = request.Points };
				await _unitOfWork.PointRepository.AddAsync(point);

				bool result = (await _unitOfWork.CommitAsync()) > 0;

				if (string.IsNullOrEmpty(cachedPoints))
				{
					var userPoints = await _unitOfWork.PointRepository.GetAllQueryable().Where(p => p.UserId == request.UserId).Select(q => new PointViewModel() { Id = q.Id, UserId = q.UserId, Value = q.Value }).ToListAsync();

					await SetStringAsync(userPoints, cacheKey);
				}
				return new ServiceResponse<bool>() { Data = result, Success = result, Message = result ? "Saved" : "not Saved !!" };
			}
			catch (Exception ex)
			{
				return await LogError<bool>(ex,false, request);
			}
		}

		public async Task<ServiceResponse<List<PointViewModel>>> GetPoint(int userId)
		{
			try
			{
				var cacheKey = $"UserPoints_{userId}";
				string cachedPoints = await GetStringAsync(cacheKey);

				List<PointViewModel> result = new();

				if (string.IsNullOrEmpty(cachedPoints))
				{
					result = await _unitOfWork.PointRepository.GetAllQueryable().Where(p => p.UserId == userId).Select(q => new PointViewModel() { Id = q.Id, UserId = q.UserId, Value = q.Value }).ToListAsync();

					await SetStringAsync(result, cacheKey);
				}
				else
				{

					result = JsonSerializer.Deserialize<List<PointViewModel>>(cachedPoints);
				}

				//_logger.LogInformation("Cached points for user {UserId}: {CachedPoints}", id, cachedPoints);

				return new ServiceResponse<List<PointViewModel>>() { Data = result, Success = result.Any(), Message = "Done" };
			}
			catch (Exception ex)
			{
				return await LogError<List<PointViewModel>>(ex, null, userId);
			}
		}
	}
}
