using DentalSystem.Application.Common;
using DentalSystem.Application.Interfaces.Services;
using DentalSystem.Application.Mapping;
using DentalSystem.Application.Resources;
using DentalSystem.Domain.Dto;
using DentalSystem.Domain.Entities;
using DentalSystemApplication.Interfaces;
using System.Linq.Expressions;

namespace DentalSystem.Application.Services
{
    public class OutPutService : ServiceBase, IOutPutService
    {
        private readonly IUnitOfWork _unitOfWork;

        public OutPutService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        public async Task<ServiceResponse<CollectionWithStatsics<OutPutViewFilterDto, OutPutStatisticsDto>>> GetOutPutsAsync(OutPutPagingSortFilterDto model)
        {
            try
            {
                int length = -1;
                IQueryable<OutPut> outputs = null;

                Expression<Func<OutPut, bool>> query = y => !y.Is_Deleted
                                                           && (!model.FromDate.HasValue
                                                                 || y.Date.Date >= model.FromDate.Value)
                                                           && (!model.ToDate.HasValue
                                                                 || y.Date.Date <= model.ToDate.Value);

                #region Sort

                outputs = _unitOfWork.OutPutRepository.GetAllDescendingAsync<DateTime>(z => z.Date, query);

                length = outputs.Count();
                #endregion
                OutPutStatisticsDto outPutStatisticsDto = new()
                {
                    TotalCost = outputs.Sum(a => a.Toatal_Cost),
                    Paid = outputs.Sum(a => a.Paid)
                };
                outputs = outputs.Skip((model.PageNumber - 1) * model.PageSize).Take(model.PageSize);

                var data = (await _unitOfWork.OutPutRepository.ToListAsync(outputs)).Convert();
                return new ServiceResponse<CollectionWithStatsics<OutPutViewFilterDto, OutPutStatisticsDto>>
                { Data = new CollectionWithStatsics<OutPutViewFilterDto, OutPutStatisticsDto>(length, data,outPutStatisticsDto), Success = true, Message = "" };
            }
            catch (Exception ex)
            {
                return await LogError<CollectionWithStatsics<OutPutViewFilterDto,OutPutStatisticsDto>>(ex, null, model);
            }
        }
        public async Task<ServiceResponse<OutPutDto>> GetOutPutById(Guid id)
        {
            try
            {
                if (id == Guid.Empty)
                    return new ServiceResponse<OutPutDto>() { Data = null, Success = false, Message = CultureHelper.GetResourceMessage(SystemResource.ResourceManager, nameof(SystemResource.InValidField), "Patient") };

                var found = _unitOfWork.OutPutRepository.FindByID(id);
                if (found == null)
                    return new ServiceResponse<OutPutDto>() { Data = null, Success = false, Message = CultureHelper.GetResourceMessage(SystemResource.ResourceManager, nameof(SystemResource.NotFound)) };

                var data = OutPutMapping.ConvertToAddOrEdit(found);

                return new ServiceResponse<OutPutDto>() { Data = data, Success = true, Message = CultureHelper.GetResourceMessage(SystemResource.ResourceManager, nameof(SystemResource.GetAllSuccess)) };

            }
            catch (Exception ex)
            {
                return await LogError<OutPutDto>(ex, null, id);
            }
        }

        public async Task<ServiceResponse<int>> UpdateOutPutAsync(OutPutAddEditDto model)
        {
            try
            {
                if (model.Id == Guid.Empty)
                    return new ServiceResponse<int>() { Data = -1, Success = false, Message = CultureHelper.GetResourceMessage(SystemResource.ResourceManager, nameof(SystemResource.InValidField), "Id") };

                var found = _unitOfWork.OutPutRepository.FindByID(model.Id);
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

        public async Task<ServiceResponse<int>> AddOutPutAsync(OutPutAddEditDto model)
        {
            try
            {
                var visit = model.Convert();
                _unitOfWork.OutPutRepository.Create(visit);
                int result = await _unitOfWork.CommitAsync();
                return new ServiceResponse<int>() { Data = result, Success = result > 0, Message = CultureHelper.GetResourceMessage(SystemResource.ResourceManager, result > 0 ? nameof(SystemResource.Added) : nameof(SystemResource.NotAdded)) };

            }
            catch (Exception ex)
            {
                return await LogError<int>(ex, 0, model);
            }
        }



        public async Task<ServiceResponse<int>> DeleteOutPutAsync(Guid[] ids)
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
