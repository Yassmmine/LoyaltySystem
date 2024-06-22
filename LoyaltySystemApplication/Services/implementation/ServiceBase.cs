using LoyaltySystemApplication.Services.Interfaces;
using LoyaltySystemDomain.Common;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace LoyaltySystemApplication.Services.implementation
{
    public class ServiceBase : IServiceBase
    {
        private readonly IDistributedCache _cache;
        public ServiceBase(IDistributedCache cache)
        {
            _cache = cache;
        }

        public Guid UserId { get; set; }

        public async Task<ServiceResponse<T>> LogError<T>(Exception ex, T data, object inputs)
        {
            return new ServiceResponse<T>() { Success = false, Data = data, Message = ex.Message + Environment.NewLine + ex.InnerException?.Message };
        }


        public async Task<string> GetStringAsync(string CachKey)
        {
            return await _cache.GetStringAsync(CachKey);
        }

        public async Task SetStringAsync(object value, string cashKey)
        {
            var cacheOptions = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(1)
            };

            var serializedPoints = JsonSerializer.Serialize(value);

            await _cache.SetStringAsync(cashKey, serializedPoints, cacheOptions); ;
        }

    }
}
