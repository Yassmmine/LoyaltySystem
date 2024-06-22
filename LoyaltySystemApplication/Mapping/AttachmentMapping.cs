using DentalSystem.Domain.Dto;
using DentalSystem.Domain.Entities;
using DentalSystem.Domain.Enum;
using GrantingSys.Domain.Dto;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;

namespace DentalSystem.Application.Mapping
{
    public static class AttachmentMapping
    {
        public static AttachmentViewFilterDto Convert(this Domain.Entities.Attachment input,string path)
        {
            if (input == null)
                return null;
            ;
            return new AttachmentViewFilterDto()
            {
                Id = input.Id,
                Description = input.Description,
                HTTPFilePath =Path.Combine(path, input.HTTP_File_Path),
                CreateDate=input.Create_Date.ToString("dd/MM/yyyy"),
                Title = input.Title
            };
        }
        public static List<Domain.Dto.AttachmentViewFilterDto> Convert(this List<Domain.Entities.Attachment> input,string path)
        {
            if (input == null)
                return null;

            var result = new List<Domain.Dto.AttachmentViewFilterDto>();
            result.AddRange(input.Select(z => z.Convert(path)));
            return result;
        }

        //public static Attachment Convert(this AttachmentAddDto input)
        //{
        //    if (input == null)
        //        return null;

        //    return new Attachment()
        //    {
        //        Row_Id = input.RowId,
        //        Title = input.Title,
        //        Description = input.Description,
        //     //   File_Type_Master_Code = input.FileTypeMasterCode.ToString(),
        //       // Attach_Type_Master_Code = input.AttachTypeMasterCode.ToString(),
        //        //Folder_Name = input.FolderName,
        //        File_Extension = input.FileExtension,
        //        File_Size = input.FileSize,
        //       // FTP_File_Path = input.FTPFilePath,
        //        HTTP_File_Path = input.HTTPFilePath,
        //        Mime_Type = input.MimeType

        //    };
        //}
        //public static AttachmentFileDto Convert(IFormFile attach, AttachmentTypes attachmentType, bool Reaize = false)
        //{
        //    return new AttachmentFileDto()
        //    {
        //        Attachment = attach,
        //        AttachmentType = attachmentType,
        //        Description = string.Empty,
        //        Title = string.Empty,
        //        //IsImageResize = Reaize,
        //    };
        //}
    }
}
//    public static class AttachmentMapping
//    {
//        public static Domain.Dto.AttachmentViewFilterDto Convert(this Domain.Entities.Attachment input)
//        {
//            if (input == null)
//                return null;

//            return new Domain.Dto.AttachmentViewFilterDto()
//            {
//			 Id = input.Id,
//			 TableName = input.Table_Name,
//			 RowId = input.Row_Id,
//			 MasterFileTypeCode = input.Master_FileType_Code,
//			 FileSize = input.File_Size,
//			 FileExtension = input.File_Extension,
//			 HTTPFilePath = input.HTTP_File_Path,
//			 MimeType = input.Mime_Type,
//			 CreateDate = input.Create_Date,
//			 Type = input.Type,
//			 LastModifiedBy = input.Last_Modified_By,
//			 LastModifyDate = input.Last_Modify_Date,
//			 Title = input.Title,
//			 Description = input.Description,

//            };
//        }

//        public static List<Domain.Dto.AttachmentViewFilterDto> Convert(this List<Domain.Entities.Attachment> input)
//        {
//            if (input == null)
//                return null;

//            var result = new List<Domain.Dto.AttachmentViewFilterDto>();
//            result.AddRange(input.Select(z => z.Convert()));
//            return result;
//        }

//        public static Domain.Entities.Attachment Convert(this Domain.Dto.AttachmentViewFilterDto input)
//        {
//            if (input == null)
//                return null;

//            return new Domain.Entities.Attachment
//            {
//			 Id = input.Id,
//			 Table_Name = input.TableName,
//			 Row_Id = input.RowId,
//			 Master_FileType_Code = input.MasterFileTypeCode,
//			 File_Size = input.FileSize,
//			 File_Extension = input.FileExtension,
//			 HTTP_File_Path = input.HTTPFilePath,
//			 Mime_Type = input.MimeType,
//			 Create_Date = input.CreateDate,
//			// Is_Active = input.IsActive,
//			 Type = input.Type,
//			 Last_Modified_By = input.LastModifiedBy,
//			 Last_Modify_Date = input.LastModifyDate,
//			 Title = input.Title,
//			 Description = input.Description,

//            };
//        }

//        public static Domain.Dto.AttachmentAddEditDto ConvertToAddOrEdit(this Domain.Entities.Attachment input)
//        {
//            if (input == null)
//                return null;

//            return new Domain.Dto.AttachmentAddEditDto()
//            { 
//			 Id = input.Id,
//			 TableName = input.Table_Name,
//			 RowId = input.Row_Id,
//			 MasterFileTypeCode = input.Master_FileType_Code,
//			 FileSize = input.File_Size,
//			 FileExtension = input.File_Extension,
//			 HTTPFilePath = input.HTTP_File_Path,
//			 MimeType = input.Mime_Type,
//			 CreateDate = input.Create_Date,
//			// IsActive = input.Is_Active,
//			 Type = input.Type,
//			 LastModifiedBy = input.Last_Modified_By,
//			 LastModifyDate = input.Last_Modify_Date,
//			 Title = input.Title,
//			 Description = input.Description,

//            };
//        }
//    }
//}