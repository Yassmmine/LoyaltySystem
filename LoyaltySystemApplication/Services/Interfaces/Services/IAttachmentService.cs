using System;
using System.Threading.Tasks;
using DentalSystem.Domain.Dto;


namespace DentalSystem.Application.Interfaces.Services
{
    public interface IAttachmentService
    {
        Task<ServiceResponse<CollectionResponse<AttachmentViewFilterDto>>> GetAttatchmentsAsync(AttachmentPagingSortFilterDto model);
        Task<ServiceResponse<AttachmentViewFilterDto>> GetAttachmentById(Guid id);

        Task<ServiceResponse<AttachmentViewFilterDto>> GetAttachmentByRowId(Guid? RowID);
        Task<ServiceResponse<AttachmentViewFilterDto>> GetAttachmentByRowIdAndAttachmentType(Guid? RowID, string FolderType);
        Task<ServiceResponse<int>> AddAttachmentAsync(AttachmentFileDto model);
        //Task<ServiceResponse<AttachmentsListViewDto>> GetAttachmentsAsync(AttachmentViewFilterDto model);

        //Task<ServiceResponse<AttachmentsListViewDto>> GetAttachmentsAsync(AttachmentPagingSortFilterDto model);

        //Task<ServiceResponse<AttachmentViewFilterDto>> GetLookupById(Guid id);

        //Task<ServiceResponse<int>> UpdateLookupAsync(AttachmentAddEditDto model);

        //Task<ServiceResponse<int>> AddLookupAsync(AttachmentAddEditDto model);

        //Task<ServiceResponse<int>> ActivateAttachmentAsync(Guid[] ids);

        //Task<ServiceResponse<int>> DectivateAttachmentAsync(Guid[] ids);

        //Task<ServiceResponse<int>> DeleteAttachmentAsync(Guid[] ids);
    }
}