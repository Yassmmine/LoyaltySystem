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
    public class PatientService : ServiceBase, IPatientService
    {
        private readonly IUnitOfWork _unitOfWork;

        public PatientService(IUnitOfWork unitOfWork)
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

       
        public async Task<ServiceResponse<Guid>> AddPersonalDataAsync(PatientAddDto model)
        {
            try
            {
               var invalidName=_unitOfWork.PatientRepository.HasAny(a=>a.Name.Trim()==model.Name.Trim());
                if (invalidName)
                    return new ServiceResponse<Guid>() { Data = Guid.Empty, Success = false, Message = CultureHelper.GetResourceMessage(SystemResource.ResourceManager, nameof(SystemResource.DublicatedName)) };
                var patient = model.Convert();
                _unitOfWork.PatientRepository.Create(patient);
               int result= await _unitOfWork.CommitAsync();
                return new ServiceResponse<Guid>() { Data = patient.Id, Success = result>0, Message = CultureHelper.GetResourceMessage(SystemResource.ResourceManager,result>0? nameof(SystemResource.Added): nameof(SystemResource.NotAdded)) };

            }
            catch (Exception ex)
            {
                return await LogError<Guid>(ex, Guid.Empty, model);
            }
        }
        public async Task<ServiceResponse<GetPatientDto>> GetPersonalData(Guid patientId)
        {
            try
            {
                if (patientId == Guid.Empty)
                    return new ServiceResponse<GetPatientDto>() { Data = null, Success = false, Message = CultureHelper.GetResourceMessage(SystemResource.ResourceManager, nameof(SystemResource.InValidField), "Patient") };

                var found = _unitOfWork.PatientRepository.FindByID(patientId);
                if (found == null)
                    return new ServiceResponse<GetPatientDto>() { Data = null, Success = false, Message = CultureHelper.GetResourceMessage(SystemResource.ResourceManager, nameof(SystemResource.NotFound)) };
                var patient = found.Convert();
                return new ServiceResponse<GetPatientDto>() { Data = patient, Success = true, Message = CultureHelper.GetResourceMessage(SystemResource.ResourceManager,nameof(SystemResource.GetAllSuccess)) };

            }
            catch (Exception ex)
            {
                return await LogError<GetPatientDto>(ex,null,patientId);
            }
        }
        #region Teeth chart
        public async Task<ServiceResponse<int>> AddTeethChartAsync(TeethChartAddEditDto model)
        {
            try
            {
                if(!Enum.TryParse(model.Code,out TeethCodeEnum code))
                    return new ServiceResponse<int>() { Data = -1, Success = false, Message = CultureHelper.GetResourceMessage(SystemResource.ResourceManager, nameof(SystemResource.InValidField),"Code") };
                if(model.PatientId==Guid.Empty)
                    return new ServiceResponse<int>() { Data = -1, Success = false, Message = CultureHelper.GetResourceMessage(SystemResource.ResourceManager, nameof(SystemResource.InValidField),"Patient") };

                var found = _unitOfWork.TeethChartRepository.Find(a => a.Patient_Id == model.PatientId && a.Code.Contains(model.Code));
                if(found==null)
                {
                    found = model.Convert();
                    _unitOfWork.TeethChartRepository.Create(found);

                }
                else
                {
                    found.Convert(model);
                }
                int result = await _unitOfWork.CommitAsync();
                return new ServiceResponse<int>() { Data = result, Success = result > 0, Message = CultureHelper.GetResourceMessage(SystemResource.ResourceManager, result > 0 ? nameof(SystemResource.Added) : nameof(SystemResource.NotAdded)) };

            }
            catch (Exception ex)
            {
                return await LogError<int>(ex, -1, model);
            }
        }
        public async Task<ServiceResponse<List<TeethChartViewDto>>> GetTeethChartAsync(Guid patientId)
        {
            try
            {
                if (patientId == Guid.Empty)
                    return new ServiceResponse<List<TeethChartViewDto>> () { Data =null, Success = false, Message = CultureHelper.GetResourceMessage(SystemResource.ResourceManager, nameof(SystemResource.InValidField), "Patient") };

                var foundCodes =await _unitOfWork.TeethChartRepository.GetAllAsync(a => a.Patient_Id == patientId);
                if (!foundCodes.Any())
                    return new ServiceResponse<List<TeethChartViewDto>> () { Data =null, Success = false, Message = CultureHelper.GetResourceMessage(SystemResource.ResourceManager, nameof(SystemResource.NotFound)) };
                var result = foundCodes.Convert();
               
                return new ServiceResponse<List<TeethChartViewDto>> () { Data = result, Success = result.Any(), Message = CultureHelper.GetResourceMessage(SystemResource.ResourceManager, result.Any()? nameof(SystemResource.GetAllSuccess) : nameof(SystemResource.NotFound)) };

            }
            catch (Exception ex)
            {
                return await LogError < List < TeethChartViewDto >> (ex, null, patientId);
            }
        }
        #endregion Teeth chart
        #region PatientsMedical
        public async Task<ServiceResponse<int>> AddPatientsMedicalAsync(PatientsMedicalAddEditDto model)
        {
            try
            {
                if (model.Id == Guid.Empty)
                    return new ServiceResponse<int>() { Data = -1, Success = false, Message = CultureHelper.GetResourceMessage(SystemResource.ResourceManager, nameof(SystemResource.InValidField), "Patient") };

                var patient = _unitOfWork.PatientRepository.FindByID(model.Id);
                if (patient == null)
                    return new ServiceResponse<int>() { Data = -1, Success = false, Message = CultureHelper.GetResourceMessage(SystemResource.ResourceManager, nameof(SystemResource.NotFound)) };
                if (patient.PatientMedical == null)
                {
                    var medicalInfo = model.Convert();
                    _unitOfWork.PatientsMedicalRepository.Create(medicalInfo);
                }
                else
                {
                    model.Convert(patient.PatientMedical);
                }
                int result = await _unitOfWork.CommitAsync();
                return new ServiceResponse<int>() { Data = result, Success = result > 0, Message = CultureHelper.GetResourceMessage(SystemResource.ResourceManager, result > 0 ? nameof(SystemResource.Added) : nameof(SystemResource.NotAdded)) };

            }
            catch (Exception ex)
            {
                return await LogError<int>(ex, -1, model);
            }
        }
        public async Task<ServiceResponse<PatientsMedicalViewFilterDto>> GetPatientsMedicalAsync(Guid patientId)
        {
            try
            {
                if (patientId == Guid.Empty)
                    return new ServiceResponse<PatientsMedicalViewFilterDto>() { Data = null, Success = false, Message = CultureHelper.GetResourceMessage(SystemResource.ResourceManager, nameof(SystemResource.InValidField), "Patient") };

                var found=  _unitOfWork.PatientsMedicalRepository.FindByID(patientId);
                if (found==null)
                    return new ServiceResponse<PatientsMedicalViewFilterDto>() { Data = null, Success = false, Message = CultureHelper.GetResourceMessage(SystemResource.ResourceManager, nameof(SystemResource.NotFound)) };
                var result = found.Convert();

                return new ServiceResponse<PatientsMedicalViewFilterDto>() { Data = result, Success = result!=null, Message = CultureHelper.GetResourceMessage(SystemResource.ResourceManager, result != null ? nameof(SystemResource.Added) : nameof(SystemResource.NotAdded)) };

            }
            catch (Exception ex)
            {
                return await LogError<PatientsMedicalViewFilterDto>(ex, null, patientId);
            }
        }
        #endregion PatientsMedical   
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

               // found.CostHistories.Add(new CostHistory()) = model.Cost;
                found.Paid = model.Paid;
                found.Toatal_Cost = model.Cost;
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
