
using DentalSystem.Domain.Dto;


namespace DentalSystem.Application.Interfaces.Services
{
    public interface IPatientService
    {
        Task<ServiceResponse<CollectionResponse<GetPatientsFilterDto>>> GetPatientsAsync(PaginationDto model);
        // Task<ServiceResponse<int>> DeletePatientAsync(Guid[] ids);
        Task<ServiceResponse<List<PatientDropDownDto>>> GetPatientAsync();
        Task<ServiceResponse<GetPatientDto>> GetPersonalData(Guid patientId);
        Task<ServiceResponse<Guid>> AddPersonalDataAsync(PatientAddDto model);
        Task<ServiceResponse<PatientViewFilterDto>> GetPatientById(Guid id);
    
        Task<ServiceResponse<List<TeethChartViewDto>>> GetTeethChartAsync(Guid id);
        Task<ServiceResponse<int>> AddTeethChartAsync(TeethChartAddEditDto model);
        
        Task<ServiceResponse<PatientsMedicalViewFilterDto>> GetPatientsMedicalAsync(Guid patientId);
        Task<ServiceResponse<int>> AddPatientsMedicalAsync(PatientsMedicalAddEditDto model);
        Task<ServiceResponse<int>> EditCost(AddCostDto model);

    }
}