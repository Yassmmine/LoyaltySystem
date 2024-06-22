
using DentalSystem.Domain.Dto;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DentalSystem.Application.Mapping
{
    public static class UserMapping
    {
        public static TokenDto Convert(this TokenDto input)
        {
            if (input == null)
                return null;

            return new TokenDto()
            {
                CurrentUser = input.CurrentUser,
                Expiration = input.Expiration,
                Token = input.Token,
            };
        }
        public static UserModel ConvetToUserModel(this IdentityUser input)
        {
            if (input == null)
                return null;

           
           // var userRole = input.UserRoles.FirstOrDefault();
            return new UserModel()
            {
                Email = input.Email,
                //FullName = input.Full_Name,
                Id =Guid.Parse(input.Id),
                PhoneNumber = input.PhoneNumber,
                UserName = input.UserName,
               // UserTypeCode = (Pioneers.Bases.UserTypes)Enum.Parse(typeof(Pioneers.Bases.UserTypes), input.User_Type_Code),
               // FeatureCodes = userRole != null ? userRole.Role.RoleFeatures.Select(a => a.Feature.Feature_Code).ToList() : new List<string>()

            };
        }

        public static Domain.Dto.UserViewFilterDto Convert(this IdentityUser input)
        {
            return new Domain.Dto.UserViewFilterDto()
            {
                Id = input.Id,
                UserName = input.UserName,
                Email = input.Email,
                PhoneNumber = input.PhoneNumber,

            };
        }

        public static List<Domain.Dto.UserViewFilterDto> Convert(this List<IdentityUser> input)
        {
            var result = new List<Domain.Dto.UserViewFilterDto>();
            result.AddRange(input.Select(z => z.Convert()));
            return result;
        }

        public static IdentityUser Convert(this Domain.Dto.UserAddEditDto input)
        {
            return new IdentityUser()
            {
			 UserName = input.UserName,
			 Email = input.Email,
			 PhoneNumber = input.PhoneNumber,

            };
        }

    //    public static Domain.Dto.UserAddEditDto ConvertToAddOrEdit(this Domain.Entities.User input)
    //    {
    //        if (input == null)
    //            return null;

    //        return new Domain.Dto.UserAddEditDto()
    //        { 
			 //Id = input.Id,
			 //UserName = input.UserName,
			 //NormalizedUserName = input.NormalizedUserName,
			 //Email = input.Email,
			 //NormalizedEmail = input.NormalizedEmail,
			 //EmailConfirmed = input.EmailConfirmed,
			 //PasswordHash = input.PasswordHash,
			 //SecurityStamp = input.SecurityStamp,
			 //ConcurrencyStamp = input.ConcurrencyStamp,
			 //PhoneNumber = input.PhoneNumber,
			 //PhoneNumberConfirmed = input.PhoneNumberConfirmed,
			 //TwoFactorEnabled = input.TwoFactorEnabled,
			 //LockoutEnd = input.LockoutEnd,
			 //LockoutEnabled = input.LockoutEnabled,
			 //AccessFailedCount = input.AccessFailedCount,

    //        };
    //    }
    }
}