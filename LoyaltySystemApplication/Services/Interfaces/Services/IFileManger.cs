using DentalSystem.Domain.Dto;
using DentalSystem.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalSystem.Application.Interfaces.Services
{
    public interface IFileManager
    {
        Task<bool> UploadFiles(string folder, string PatientName, List<AttachmentFileDto> AttachmentFileDto);
     //   void DeleteBothFile(List<Guid> rowids, AttachmentTypes attachmentType);
       // void DeleteFTPFile(List<Guid> rowids, AttachmentTypes attachmentType);
       // void DeleteBothFileByFilesId(List<Guid> FilesIds);
        void DeleteHttpFile(List<Guid> rowids, AttachmentTypes attachmentType);
      //  Task<DownLoadDto> EncodeFileToBase64ByAttachId(Guid AttachId);
      //  Task<DownLoadDto> EncodeFileToBase64ByRowId(Guid RowId);
        Byte[] EncodeFileToByte(Guid VideoId);
    }
}
