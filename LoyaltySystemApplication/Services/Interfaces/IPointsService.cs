using LoyaltySystemDomain.Common;
using LoyaltySystemDomain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoyaltySystemApplication.Services.Interfaces
{
    public interface IPointsService
    {
        Task<ServiceResponse<bool>> EarnPoints(EarnPointsRequest request);
        Task<ServiceResponse<List<PointViewModel>>> GetPoint(int userId);

    }
}
