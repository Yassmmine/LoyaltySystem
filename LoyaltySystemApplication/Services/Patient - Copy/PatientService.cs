using System.Linq.Expressions;
using DentalSystem.Domain.Dto;
using DentalSystem.Application.Interfaces.Services;
using DentalSystem.Application.Mapping;
using DentalSystemApplication.Interfaces;
using DentalSystem.Application.Common;
using DentalSystem.Application.Resources;
using DentalSystem.Domain.Enum;
using System.Collections.Generic;
using DentalSystem.Domain.Entities;

namespace DentalSystem.Application.Services
{
    public class CostHistoryService : ServiceBase, ICostHistoryService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CostHistoryService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }



        public async Task<ServiceResponse<CollectionResponse<GetPatientsFilterDto>>> GetPatientsAsync(PaginationDto model)
        {
            try
            {
                int length = -1;
                IQueryable<Patient> patients = null;

                Expression<Func<Patient, bool>> query = y => !y.Is_Deleted 
                                                          &&( string.IsNullOrEmpty(model.Search)
                                                          || y.Phone.Contains(model.Search.Trim())
                                                          || y.Name.Contains(model.Search.Trim()));

                #region search
                
                patients = _unitOfWork.PatientRepository.GetAllDescendingAsync<DateTime>(z => z.Create_Date, query);

                length = patients.Count();
                #endregion

                patients = patients.Skip((model.PageNumber - 1) * model.PageSize).Take(model.PageSize);

                var data = (await _unitOfWork.PatientRepository.ToListAsync(patients)).Convert();
                return new ServiceResponse<CollectionResponse<GetPatientsFilterDto>> 
                { 
                    Data = new CollectionResponse<GetPatientsFilterDto> (length,data) , 
                    Success = length>0,
                    Message = CultureHelper.GetResourceMessage(SystemResource.ResourceManager, length > 0? nameof(SystemResource.GetAllSuccess): nameof(SystemResource.NotFound))
                };
            }
            catch (Exception ex)
            {
                return await LogError<CollectionResponse<GetPatientsFilterDto>>(ex, null, model);
            }
        }
        public async Task<ServiceResponse<List<PatientDropDownDto>>> GetPatientAsync()
        {
            try
            {
                #region search
                 var patients = _unitOfWork.PatientRepository.GetAll();
                #endregion
                var data = patients.Select(a=>new PatientDropDownDto() { PatientId=a.Id ,PatientName=a.Name}).ToList();
                return new ServiceResponse<List<PatientDropDownDto>>
                {
                    Data = data,
                    Success = data.Any(),
                    Message = CultureHelper.GetResourceMessage(SystemResource.ResourceManager, data.Any() ? nameof(SystemResource.GetAllSuccess) : nameof(SystemResource.NotFound))
                };
            }
            catch (Exception ex)
            {
                return await LogError<List<PatientDropDownDto>>(ex, null,null);
            }
        }
        public async Task<ServiceResponse<PatientViewFilterDto>> GetPatientById(Guid id)
        {
            try
            {
                throw new NotImplementedException();
            }
            catch (Exception ex)
            {
                return await LogError<PatientViewFilterDto>(ex, null, id);
            }
        }
        
   
        //public async Task<ServiceResponse<int>> DeletePatientAsync(Guid[] ids)
        //{
        //    try
        //    {
        //        throw new NotImplementedException();
        //    }
        //    catch (Exception ex)
        //    {
        //        return await LogError<int>(ex, 0, ids);
        //    }
        //}

       
        #region Cost
        public async Task<ServiceResponse<int>> EditCost(AddCostDto model)
        {
            try
            {
                if (model.Id == Guid.Empty)
                    return new ServiceResponse<int>() { Data = -1, Success = false, Message = CultureHelper.GetResourceMessage(SystemResource.ResourceManager, nameof(SystemResource.InValidField), "Patient") };

                var found = _unitOfWork.PatientRepository.FindByID( model.Id);
                if (found == null)
                    return new ServiceResponse<int>() { Data = -1, Success = false, Message = CultureHelper.GetResourceMessage(SystemResource.ResourceManager, nameof(SystemResource.NotFound)) };

                found.Toatal_Cost = model.Cost;
                found.Paid = model.Paid;
                int result = await _unitOfWork.CommitAsync();
                return new ServiceResponse<int>() { Data = result, Success = result > 0, Message = CultureHelper.GetResourceMessage(SystemResource.ResourceManager, result > 0 ? nameof(SystemResource.Updated) : nameof(SystemResource.NotUpdated)) };

            }
            catch (Exception ex)
            {
                return await LogError<int>(ex, -1, model);
            }
        }
        #endregion
    }
}
