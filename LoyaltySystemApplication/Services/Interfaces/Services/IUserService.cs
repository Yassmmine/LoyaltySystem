using System;
using System.Threading.Tasks;
using DentalSystem.Domain.Dto;

namespace DentalSystem.Application.Interfaces.Services
{
    public interface IUserService
    {
        Task<ServiceResponse<int>> AddUserAsync(UserAddEditDto model);
        Task<ServiceResponse<UserViewFilterDto>> GetUserById(string id);
        Task<ServiceResponse<UsersListViewDto>> GetUsersAsync(UserPagingSortFilterDto model);

        //Task<ServiceResponse<AspNetUsersListViewDto>> GetAspNetUsersAsync(AspNetUserPagingSortFilterDto model);

        //Task<ServiceResponse<AspNetUserViewFilterDto>> GetLookupById(string id);

        //Task<ServiceResponse<int>> UpdateLookupAsync(AspNetUserAddEditDto model);

        //Task<ServiceResponse<int>> AddLookupAsync(AspNetUserAddEditDto model);

        //Task<ServiceResponse<int>> ActivateAspNetUserAsync(string[] ids);

        //Task<ServiceResponse<int>> DectivateAspNetUserAsync(string[] ids);

        //Task<ServiceResponse<int>> DeleteAspNetUserAsync(string[] ids);
        Task<ServiceResponse<TokenDto>> Token(LoginDto model);
    }
}