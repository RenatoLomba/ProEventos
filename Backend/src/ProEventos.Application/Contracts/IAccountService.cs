using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using ProEventos.Application.Dtos;

namespace ProEventos.Application.Contracts
{
    public interface IAccountService
    {
        Task<bool> UserExists(string username);
        Task<UserUpdateDto> GetUserByUsername(string username);
        Task<SignInResult> CheckUserPassword(UserUpdateDto userDto, string password);
        Task<UserUpdateDto> CreateAccount(UserDto userDto);
        Task<UserUpdateDto> UpdateAccount(UserUpdateDto userDto);

        IEnumerable<string> GetAvailableTitles();
        IEnumerable<string> GetAvailableFunctions();
    }
}
