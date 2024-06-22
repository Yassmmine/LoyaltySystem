using System;
using System.Threading.Tasks;
using DentalSystem.Domain.Dto;


namespace DentalSystem.Application.Interfaces.Services
{
    public interface IOutPutService
    {
        Task<ServiceResponse<int>> AddOutPutAsync(OutPutAddEditDto model);
        Task<ServiceResponse<int>> UpdateOutPutAsync(OutPutAddEditDto model); 
        Task<ServiceResponse<OutPutDto>> GetOutPutById(Guid id);
        Task<ServiceResponse<int>> DeleteOutPutAsync(Guid[] ids);
        Task<ServiceResponse<CollectionWithStatsics<OutPutViewFilterDto, OutPutStatisticsDto>>> GetOutPutsAsync(OutPutPagingSortFilterDto model);
    }
}