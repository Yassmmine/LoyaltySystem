using System;
using System.Threading.Tasks;
using System.Linq;
using System.Linq.Expressions;
using DentalSystem.Domain.Dto;
using DentalSystem.Application.Interfaces.Services;
using DentalSystem.Application.Mapping;
using DentalSystem.Domain.Entities;
using DentalSystem.Application.Interfaces;
using DentalSystemApplication.Interfaces;
using DentalSystem.Application.Common;
using DentalSystem.Application.Resources;
using System.Reflection;

namespace DentalSystem.Application.Services
{
    public class VisitsAndServviceService : ServiceBase, IVisitsAndServviceService
    {
        private readonly IUnitOfWork _unitOfWork;

        public VisitsAndServviceService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

      
        public async Task<ServiceResponse<CollectionResponse<VisitsAndServviceViewPatientDto>>> GetVisitsAndServvicesAsync(VisitsAndServvicePaginationForPatinentDto model)
        {
            try
            {
                if (model.PatientId == Guid.Empty )
                    return new ServiceResponse<CollectionResponse < VisitsAndServviceViewPatientDto>>() { Data = null, Success = false, Message = CultureHelper.GetResourceMessage(SystemResource.ResourceManager, nameof(SystemResource.InValidField), "Patient") };

                var patient = _unitOfWork.PatientRepository.FindByID(model.PatientId);
                if (patient == null)
                    return new ServiceResponse<CollectionResponse<VisitsAndServviceViewPatientDto>>() { Data = null, Success = false, Message = CultureHelper.GetResourceMessage(SystemResource.ResourceManager, nameof(SystemResource.NotFound)) };
                 int length = -1;
                var visits = patient.VisitsAndServvices;

                length = visits.Count();
                
                var data = visits.Skip((model.PageNumber - 1) * model.PageSize).Take(model.PageSize).ToList().ConvertViewPatient();
                
                
                return new ServiceResponse<CollectionResponse<VisitsAndServviceViewPatientDto>>
                { Data = new CollectionResponse<VisitsAndServviceViewPatientDto>(length,data), Success = true, Message = "" };
            }
            catch (Exception ex)
            {
                return await LogError < CollectionResponse <VisitsAndServviceViewPatientDto>> (ex, null, model);
            }
        }
        public async Task<ServiceResponse<CollectionResponse<VisitsAndServviceViewFilterDto>>> GetVisitsAndServvicesAsync(VisitsAndServvicePagingSortFilterDto model)
        {
            try
            {
                int length = -1;
                IQueryable<VisitsAndServvice> visits = null;

                Expression<Func<VisitsAndServvice, bool>> query = y => !y.Is_Deleted
                                                           && (string.IsNullOrEmpty(model.Search)
                                                                 || y.Patient.Name.Contains(model.Search)
                                                                 || y.Patient.Phone.Contains(model.Search))
                                                           && (!model.FromDate.HasValue
                                                                 || y.Date.Date >= model.FromDate.Value)
                                                           && (!model.ToDate.HasValue
                                                                 || y.Date.Date <= model.ToDate.Value);

                #region Sort

                visits = _unitOfWork.VisitsAndServviceRepository.GetAllDescendingAsync<DateTime>(z => z.Date, query);

                length = visits.Count();
                #endregion

                visits = visits.Skip((model.PageNumber - 1) * model.PageSize).Take(model.PageSize);

                var data = (await _unitOfWork.VisitsAndServviceRepository.ToListAsync(visits)).Convert();
                return new ServiceResponse<CollectionResponse<VisitsAndServviceViewFilterDto>>
                { Data = new CollectionResponse<VisitsAndServviceViewFilterDto>(length, data), Success = true, Message = "" };
            }
            catch (Exception ex)
            {
                return await LogError<CollectionResponse<VisitsAndServviceViewFilterDto>>(ex, null, model);
            }
        }
        public async Task<ServiceResponse<VisitsAndServviceDto>> GetVisitsAndServviceById(Guid id)
        {
            try
            {
                if (id == Guid.Empty)
                    return new ServiceResponse<VisitsAndServviceDto>() { Data = null, Success = false, Message = CultureHelper.GetResourceMessage(SystemResource.ResourceManager, nameof(SystemResource.InValidField), "Patient") };

                var found = _unitOfWork.VisitsAndServviceRepository.FindByID(id);
                if (found == null)
                    return new ServiceResponse<VisitsAndServviceDto>() { Data = null, Success = false, Message = CultureHelper.GetResourceMessage(SystemResource.ResourceManager, nameof(SystemResource.NotFound)) };

                var data=VisitsAndServviceMapping.ConvertToAddOrEdit(found);
                
                return new ServiceResponse<VisitsAndServviceDto>() { Data = data, Success =true, Message = CultureHelper.GetResourceMessage(SystemResource.ResourceManager, nameof(SystemResource.GetAllSuccess)) };

            }
            catch (Exception ex)
            {
                return await LogError<VisitsAndServviceDto>(ex, null, id);
            }
        }
        
        public async Task<ServiceResponse<int>> UpdateVisitsAndServviceAsync(VisitsAndServviceAddEditDto model)
        {
            try
            {
                if (model.PatientId == Guid.Empty || model.Id==Guid.Empty)
                    return new ServiceResponse<int>() { Data = -1, Success = false, Message = CultureHelper.GetResourceMessage(SystemResource.ResourceManager, nameof(SystemResource.InValidField), "Patient") };

                var patient = _unitOfWork.PatientRepository.FindByID(model.PatientId);
                if (patient == null)
                    return new ServiceResponse<int>() { Data = -1, Success = false, Message = CultureHelper.GetResourceMessage(SystemResource.ResourceManager, nameof(SystemResource.NotFound)) };
                var found = _unitOfWork.VisitsAndServviceRepository.FindByID(model.Id);
                if (found == null)
                    return new ServiceResponse<int>() { Data = -1, Success = false, Message = CultureHelper.GetResourceMessage(SystemResource.ResourceManager, nameof(SystemResource.NotFound)) };

                 model.Convert(found);
                int result = await _unitOfWork.CommitAsync();
                return new ServiceResponse<int>() { Data = result, Success = result > 0, Message = CultureHelper.GetResourceMessage(SystemResource.ResourceManager, result > 0 ? nameof(SystemResource.Added) : nameof(SystemResource.NotAdded)) };

            }
            catch (Exception ex)
            {
                return await LogError<int>(ex, 0, model);
            }
        }
        
        public async Task<ServiceResponse<int>> AddVisitsAndServviceAsync(VisitsAndServviceAddEditDto model)
        {
            try
            {
                if(model.PatientId==Guid.Empty)
                    return new ServiceResponse<int>() { Data = -1, Success = false, Message = CultureHelper.GetResourceMessage(SystemResource.ResourceManager, nameof(SystemResource.InValidField), "Patient") };

                var found = _unitOfWork.PatientRepository.FindByID(model.PatientId);
                if (found == null)
                    return new ServiceResponse<int>() { Data = -1, Success = false, Message = CultureHelper.GetResourceMessage(SystemResource.ResourceManager, nameof(SystemResource.NotFound)) };
                var visit = model.Convert();
                _unitOfWork.VisitsAndServviceRepository.Create(visit);
                int result = await _unitOfWork.CommitAsync();
                return new ServiceResponse<int>() { Data = result, Success = result > 0, Message = CultureHelper.GetResourceMessage(SystemResource.ResourceManager, result > 0 ? nameof(SystemResource.Added) : nameof(SystemResource.NotAdded)) };

            }
            catch (Exception ex)
            {
                return await LogError<int>(ex, 0, model);
            }
        }
        
     
        
        public async Task<ServiceResponse<int>> DeleteVisitsAndServviceAsync(Guid[] ids)
        {
            try
            {
                throw new NotImplementedException();
            }
            catch (Exception ex)
            {
                return await LogError<int>(ex, 0, ids);
            }
        }
    }
}
