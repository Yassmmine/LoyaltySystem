using System;
using System.Threading.Tasks;
using DentalSystem.Domain.Dto;


namespace DentalSystem.Application.Interfaces.Services
{
    public interface IVisitsAndServviceService
    {
        Task<ServiceResponse<int>> AddVisitsAndServviceAsync(VisitsAndServviceAddEditDto model);
        Task<ServiceResponse<int>> UpdateVisitsAndServviceAsync(VisitsAndServviceAddEditDto model); 
        Task<ServiceResponse<VisitsAndServviceDto>> GetVisitsAndServviceById(Guid id);
        Task<ServiceResponse<int>> DeleteVisitsAndServviceAsync(Guid[] ids);
        Task<ServiceResponse<CollectionResponse<VisitsAndServviceViewPatientDto>>> GetVisitsAndServvicesAsync(VisitsAndServvicePaginationForPatinentDto model);
        Task<ServiceResponse<CollectionResponse<VisitsAndServviceViewFilterDto>>> GetVisitsAndServvicesAsync(VisitsAndServvicePagingSortFilterDto model);
    }
}