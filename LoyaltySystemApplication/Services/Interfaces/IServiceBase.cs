using LoyaltySystemDomain.Common;

namespace LoyaltySystemApplication.Services.Interfaces
{
    public interface IServiceBase
    {
        public Guid UserId { get; set; }
        Task<string> GetStringAsync(string CachKey);
        Task SetStringAsync(object value, string cashKey);
        Task<ServiceResponse<T>> LogError<T>(Exception ex, T data, object inputs);
    }
}
