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
using Microsoft.AspNetCore.Identity;
using static DentalSystem.Domain.RouteClass;

namespace DentalSystem.Application.Services
{
    public class UsersService : ServiceBase, IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly TokenSettings _TokenSettings;

        public UsersService(IUnitOfWork unitOfWork,TokenSettings tokenSettings)
        {
            _unitOfWork = unitOfWork;
            _TokenSettings = tokenSettings;

        }

        public async Task<ServiceResponse<UsersListViewDto>> GetUsersAsync(UserPagingSortFilterDto model)
        {
            try
            {
                int length = -1;
                IQueryable<IdentityUser> users = null;

                Expression<Func<IdentityUser, bool>> query = y => string.IsNullOrEmpty(model.Search)
                                                                || (y.UserName.Contains(model.Search)
                                                                    || y.PhoneNumber.Contains(model.Search)
                                                                    || y.Email.Contains(model.Search)
                                                                    ) ;

                #region Sort
                users = _unitOfWork.UserRepository.GetAllDescendingAsync<string>(z => z.Id, query);

                length = users.Count();
                #endregion
                
                users = users.Skip((model.PageNumber - 1) * model.PageSize).Take(model.PageSize);
                
                var data = users.ToList().Convert();
                return new ServiceResponse<UsersListViewDto> { Data = new UsersListViewDto 
                { Users = data, Count = length }, Success = true, Message = "" };
            }
            catch (Exception ex)
            {
                return await LogError<UsersListViewDto>(ex, null, model);
            }
        }
        
        public async Task<ServiceResponse<UserViewFilterDto>> GetUserById(string id)
        {
            try
            {
                if(string.IsNullOrEmpty(id))
                    return new ServiceResponse<UserViewFilterDto>() { Data = null, Success = false, Message = CultureHelper.GetResourceMessage(SystemResource.ResourceManager, nameof(SystemResource.InValidField), "Id") };

                var found = _unitOfWork.UserRepository.FindByID(id);
                if (found == null)
                    return new ServiceResponse<UserViewFilterDto>() { Data = null, Success = false, Message = CultureHelper.GetResourceMessage(SystemResource.ResourceManager, nameof(SystemResource.NotFound)) };
                var user = found.Convert();
                return new ServiceResponse<UserViewFilterDto>() { Data = user, Success = true, Message = CultureHelper.GetResourceMessage(SystemResource.ResourceManager, nameof(SystemResource.GetAllSuccess)) };

            }
            catch (Exception ex)
            {
                return await LogError<UserViewFilterDto>(ex, null, id);
            }
        }
        
        public async Task<ServiceResponse<int>> UpdateUserAsync(UserAddEditDto model)
        {
            try
            {
                throw new NotImplementedException();
            }
            catch (Exception ex)
            {
                return await LogError<int>(ex, 0, model);
            }
        }
        
        public async Task<ServiceResponse<int>> AddUserAsync(UserAddEditDto model)
        {
            try
            {
                var invalidName = _unitOfWork.UserRepository.HasAny(a => a.UserName.Trim() == model.UserName.Trim());
                if (invalidName)
                    return new ServiceResponse<int>() { Data = -1, Success = false, Message = CultureHelper.GetResourceMessage(SystemResource.ResourceManager, nameof(SystemResource.DublicatedName)) };
                 var invalidPhone = _unitOfWork.UserRepository.HasAny(a => a.PhoneNumber.Trim() == model.PhoneNumber.Trim());
                if (invalidPhone)
                    return new ServiceResponse<int>() { Data = -1, Success = false, Message = CultureHelper.GetResourceMessage(SystemResource.ResourceManager, nameof(SystemResource.DublicatedPhone)) };
                 var invalidEmail = _unitOfWork.UserRepository.HasAny(a => a.Email.Trim() == model.Email.Trim());
                 if (invalidEmail)
                    return new ServiceResponse<int>() { Data = -1, Success = false, Message = CultureHelper.GetResourceMessage(SystemResource.ResourceManager, nameof(SystemResource.DublicatedEmail)) };
                var user = model.Convert();
                IdentityResult result=await _unitOfWork.UserRepository.Create(user,model.Password);
                if(result.Succeeded) 
                    return new ServiceResponse<int>() { Data = 1, Success = true, Message = CultureHelper.GetResourceMessage(SystemResource.ResourceManager, nameof(SystemResource.Added)) };
                else 
                    return new ServiceResponse<int>() { Data = -1, Success = false, Message = result.Errors.FirstOrDefault().Description };
            }
            catch (Exception ex)
            {
                return await LogError<int>(ex, 0, model);
            }
        }
        
    
        public async Task<ServiceResponse<int>> DeleteUserAsync(string[] ids)
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


        //-----login and forget pass
        public async Task<ServiceResponse<TokenDto>> Token(LoginDto model)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(model.UserName) || string.IsNullOrWhiteSpace(model.Password))
                    return new ServiceResponse<TokenDto> { Success = false, Data = null, Message = CultureHelper.GetResourceMessage(SystemResource.ResourceManager, nameof(SystemResource.InValidField)) };
                (var signIn, var User )= await _unitOfWork.ApplicationUserRepository.FindByNameAsync(model.UserName,model.Password);
                if (User == null||!signIn.Succeeded)
                    return new ServiceResponse<TokenDto> { Success = false, Data = null, Message = CultureHelper.GetResourceMessage(SystemResource.ResourceManager, nameof(SystemResource.NotAuthorized)) };
                
                var token = await Token(model, User);
                if (token.Data == null)
                    return new ServiceResponse<TokenDto> { Success = false, Data = null, Message = CultureHelper.GetResourceMessage(SystemResource.ResourceManager, nameof(SystemResource.NotAuthorized)) };
                return token;
            }
            catch (Exception ex)
            {
                return await LogError<TokenDto>(ex, null, model);
            }
        }
        private async Task<ServiceResponse<TokenDto>> Token(LoginDto model, IdentityUser user)
        {
            var token = await _unitOfWork.ApplicationUserRepository.GetToken(user, model.Password, _TokenSettings.SecretKey, _TokenSettings.Issuer, _TokenSettings.Audience, false);
            if (token != null)
            {
                var tokenModel = token.Convert();
                return new ServiceResponse<TokenDto> { Success = true, Data = tokenModel };
            }
            else
                return new ServiceResponse<TokenDto> { Success = false, Data = null, Message = CultureHelper.GetResourceMessage(SystemResource.ResourceManager, nameof(SystemResource.NotAuthorized)) };
        }
        //public async Task<ServiceResponse<int>> ForgetPassword(ForgetPasswordDto forgetPasswordDto)
        //{
        //    try
        //    {
        //        var userExists = await _userManager.FindByEmailAsync(forgetPasswordDto.Email);
        //        if (userExists != null)
        //        {
        //            var token = await _userManager.GeneratePasswordResetTokenAsync(userExists);

        //            EmailMessageForOneDto message = EmailBody.ForgetPasswordEmailMail(forgetPasswordDto, token, userExists);
        //            var res = _emailSender.SendEmail(message, CultureHelper.GetResourceMessage(SendEmailResource.ResourceManager, nameof(SendEmailResource.EmailLayout)));
        //            return new ServiceResponse<int>
        //            {
        //                Data = 1,
        //                Success = true,
        //                Message = CultureHelper.GetResourceMessage(SystemResource.ResourceManager, res ? nameof(SystemResource.Sent) : nameof(SystemResource.NotSent))
        //            };
        //        }
        //        else
        //        {
        //            return new ServiceResponse<int>
        //            {
        //                Data = 0,
        //                Message = CultureHelper.GetResourceMessage(SystemResource.ResourceManager, nameof(SystemResource.EmailNotExist)),
        //                Success = false
        //            };
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        return new ServiceResponse<int>
        //        {
        //            Data = 0,
        //            Message = ex.Message,
        //            Success = false
        //        };
        //    }
        //}
        //public async Task<ServiceResponse<int>> Resetpassword(RestPasswordDto RestPasswordForAnonymousUserDto)
        //{
        //    try
        //    {
        //        var user = await _userManager.FindByEmailAsync(RestPasswordForAnonymousUserDto.Email);
        //        if (user != null)
        //        {
        //            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
        //            var result = await _userManager.ResetPasswordAsync(user, token, RestPasswordForAnonymousUserDto.Password.Trim());

        //            if (!result.Succeeded)
        //            {
        //                return new ServiceResponse<int>
        //                {
        //                    Data = 0,
        //                    Message = CultureHelper.GetResourceMessage(SystemResource.ResourceManager, nameof(SystemResource.NotUpdated)),
        //                    Success = false
        //                };
        //            }
        //            else
        //            {
        //                return new ServiceResponse<int>
        //                {
        //                    Data = 1,
        //                    Message = CultureHelper.GetResourceMessage(SystemResource.ResourceManager, nameof(SystemResource.Updated)),
        //                    Success = true
        //                };
        //            }
        //        }
        //        else
        //        {

        //            return new ServiceResponse<int>
        //            {
        //                Data = 0,
        //                Message = CultureHelper.GetResourceMessage(SystemResource.ResourceManager, nameof(SystemResource.NotAdded)),
        //                Success = false
        //            };
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        return new ServiceResponse<int>
        //        {
        //            Data = 0,
        //            Message = ex.Message,
        //            Success = false
        //        };
        //    }
        //}

    }
}
