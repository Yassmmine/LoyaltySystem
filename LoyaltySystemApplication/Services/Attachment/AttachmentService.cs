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
using DentalSystem.Domain.Enum;
using DentalSystem.Application.Resources;
using static DentalSystem.Domain.RouteClass;
using System.Net.Mail;
using DentalSystem.Application.Interfaces.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http;

namespace DentalSystem.Application.Services
{
    public class AttachmentService : ServiceBase, IAttachmentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IFileManager _fileManager;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AttachmentService(IUnitOfWork unitOfWork, IFileManager fileManager, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _fileManager = fileManager;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<ServiceResponse<CollectionResponse<AttachmentViewFilterDto>>> GetAttatchmentsAsync(AttachmentPagingSortFilterDto model)
        {
            try
            {
                int length = -1;
                IQueryable<Domain.Entities.Attachment> attatchment = null;

                Expression<Func<Domain.Entities.Attachment, bool>> query = y => !y.Is_Deleted
                                                          && y.Row_Id == model.PatientId;

                #region search

                attatchment = _unitOfWork.AttachmentRepository.GetAllDescendingAsync<DateTime>(z => z.Create_Date, query);

                length = attatchment.Count();
                #endregion

                attatchment = attatchment.Skip((model.PageNumber - 1) * model.PageSize).Take(model.PageSize);
                string host = _httpContextAccessor.HttpContext.Request.Host.ToString() + '/';
                string path= "http://"+ host +_configuration["wwwrootPath"];
                var data = (await _unitOfWork.AttachmentRepository.ToListAsync(attatchment)).Convert(path);
                return new ServiceResponse<CollectionResponse<AttachmentViewFilterDto>>
                {
                    Data = new CollectionResponse<AttachmentViewFilterDto>(length, data),
                    Success = length > 0,
                    Message = CultureHelper.GetResourceMessage(SystemResource.ResourceManager, length > 0 ? nameof(SystemResource.GetAllSuccess) : nameof(SystemResource.NotFound))
                };
            }
            catch (Exception ex)
            {
                return await LogError<CollectionResponse<AttachmentViewFilterDto>>(ex, null, model);
            }
        }

        public async Task<ServiceResponse<AttachmentViewFilterDto>> GetAttachmentById(Guid id)
        {
            try
            {
                if ( id == Guid.Empty) return new ServiceResponse<AttachmentViewFilterDto> { Data = null, Success = false, Message = CultureHelper.GetResourceMessage(SystemResource.ResourceManager, nameof(SystemResource.NotFound)) };
                var Attachment = _unitOfWork.AttachmentRepository.FindByID(id);
                if (Attachment == null) return new ServiceResponse<AttachmentViewFilterDto>
                {
                    Data = null,
                    Success = false
                };
                string host = _httpContextAccessor.HttpContext.Request.Host.ToString() + '/';
                string path = "http://" + host + _configuration["wwwrootPath"];
                AttachmentViewFilterDto attachmentViewFilter = AttachmentMapping.Convert(Attachment,path);
                return new ServiceResponse<AttachmentViewFilterDto>
                {
                    Data = attachmentViewFilter,
                    Success = attachmentViewFilter != null
                };
            }
            catch (Exception ex)
            {
                return await LogError<AttachmentViewFilterDto>(ex, null, id);
            }
        }
        public async Task<ServiceResponse<AttachmentViewFilterDto>> GetAttachmentByRowId(Guid? RowID)
        {
            try
            {
                if (RowID == null || RowID == Guid.Empty) return new ServiceResponse<AttachmentViewFilterDto> { Data = null, Success = false, Message = CultureHelper.GetResourceMessage(SystemResource.ResourceManager, nameof(SystemResource.NotFound)) };
                var Attachment = _unitOfWork.AttachmentRepository.Find(a => a.Row_Id == RowID.Value);
                string host = _httpContextAccessor.HttpContext.Request.Host.ToString() + '/';
                string path = "http://" + host + _configuration["wwwrootPath"];
                AttachmentViewFilterDto attachmentViewFilter = AttachmentMapping.Convert(Attachment,path);
                return new ServiceResponse<AttachmentViewFilterDto>
                {
                    Data = attachmentViewFilter,
                    Success = true
                };
            }
            catch (Exception ex)
            {
                return await LogError<AttachmentViewFilterDto>(ex, null, RowID);
            }
        }

        public async Task<ServiceResponse<AttachmentViewFilterDto>> GetAttachmentByRowIdAndAttachmentType(Guid? RowID, string FolderType)
        {
            try
            {
                if (RowID == null || RowID == Guid.Empty)
                    return new ServiceResponse<AttachmentViewFilterDto> { Data = null, Success = false, Message = CultureHelper.GetResourceMessage(SystemResource.ResourceManager, nameof(SystemResource.NotFound)) };
                if (!Enum.TryParse(FolderType, out AttachmentTypes status))
                    return new ServiceResponse<AttachmentViewFilterDto> { Data = null, Success = false, Message = CultureHelper.GetResourceMessage(SystemResource.ResourceManager, nameof(SystemResource.InValidField)) };
                var Attachment = _unitOfWork.AttachmentRepository.Find(a => a.Row_Id == RowID.Value);
                string host = _httpContextAccessor.HttpContext.Request.Host.ToString() + '/';
                string path = "http://" + host + _configuration["wwwrootPath"];
                AttachmentViewFilterDto attachmentViewFilter = AttachmentMapping.Convert(Attachment, path);
                return new ServiceResponse<AttachmentViewFilterDto>
                {
                    Data = attachmentViewFilter,
                    Success = true,
                    Message = CultureHelper.GetResourceMessage(SystemResource.ResourceManager,attachmentViewFilter == null ? nameof(SystemResource.NotFound):nameof(SystemResource.GetAllSuccess))
                };
            }
            catch (Exception ex)
            {
                return await LogError<AttachmentViewFilterDto>(ex, null, RowID);
            }
        }


        //public async Task<ServiceResponse<int>> UpdateAttachmentAsync(AttachmentAddEditDto model)
        //{
        //    try
        //    {
        //        throw new NotImplementedException();
        //    }
        //    catch (Exception ex)
        //    {
        //        return await LogError<int>(ex, 0, model);
        //    }
        //}

        public async Task<ServiceResponse<int>> AddAttachmentAsync(AttachmentFileDto model)
        {
            try
            {
                if(model.Attachment==null)
                    return new ServiceResponse<int> {Data=-1,Success=false,Message = CultureHelper.GetResourceMessage(SystemResource.ResourceManager, nameof(SystemResource.InValidField),"File") };
                if (model.PatientId == Guid.Empty)
                    return new ServiceResponse<int>() { Data = -1, Success = false, Message = CultureHelper.GetResourceMessage(SystemResource.ResourceManager, nameof(SystemResource.InValidField), "Patient") };

                var found = _unitOfWork.PatientRepository.FindByID( model.PatientId);
                if (found == null)
                    return new ServiceResponse<int>() { Data = -1, Success = false, Message = CultureHelper.GetResourceMessage(SystemResource.ResourceManager, nameof(SystemResource.NotFound)) };

                List<AttachmentFileDto> fileDtos = new();
                fileDtos.Add(model);
                //uplod in both
                await _fileManager.UploadFiles(AttachmentFolders.Patients.ToString(),found.Name, fileDtos);
                int result = await _unitOfWork.CommitAsync();
                return new ServiceResponse<int> { Data = result, Success = result > 0, Message = CultureHelper.GetResourceMessage(SystemResource.ResourceManager, result > 0 ? nameof(SystemResource.Added) : nameof(SystemResource.NotAdded)) };

            }
            catch (Exception ex)
            {
                return await LogError<int>(ex, 0, model);
            }
        }

    }
}
