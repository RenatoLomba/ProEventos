using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ProEventos.Application.Contracts;
using ProEventos.Application.Dtos;
using ProEventos.Domain.Enum;
using ProEventos.Domain.Identity;
using ProEventos.Persistence.Contracts;

namespace ProEventos.Application.Implementations
{
    public class AccountService : IAccountService
    {
        private UserManager<User> UserManager;
        private IUserPersist UserPersist;
        private IMapper Mapper;
        private SignInManager<User> SignInManager;

        public AccountService(UserManager<User> userManager, IMapper mapper,
            IUserPersist userPersist, SignInManager<User> signInManager)
        {
            this.SignInManager = signInManager;
            this.UserPersist = userPersist;
            this.UserManager = userManager;
            this.Mapper = mapper;
        }

        public async Task<bool> UserExists(string username)
        {
            try
            {
                return await this.UserManager.Users
                    .AnyAsync(user => user.UserName.Equals(username));
            }
            catch (Exception ex)
            {
                throw new Exception($"Error Details: {ex.Message}");
            }
        }

        public async Task<UserUpdateDto> GetUserByUsername(string username)
        {
            try
            {
                var user = await this.UserPersist.GetUserByUsernameAsync(username);
                if (user == null) return null;

                return this.Mapper.Map<UserUpdateDto>(user);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error Details: {ex.Message}");
            }
        }

        public async Task<SignInResult> CheckUserPassword(UserUpdateDto userDto, string password)
        {
            try
            {
                var user = await this.UserManager.Users
                    .FirstOrDefaultAsync(user => user.UserName
                        .Equals(userDto.UserName));

                return await this.SignInManager
                    .CheckPasswordSignInAsync(user, password, false);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error Details: {ex.Message}");
            }
        }

        public async Task<UserUpdateDto> CreateAccount(UserDto userDto)
        {
            try
            {
                var user = this.Mapper.Map<User>(userDto);
                var result = await this.UserManager.CreateAsync(user, userDto.Password);

                if (result.Succeeded)
                {
                    var userToReturn = this.Mapper.Map<UserUpdateDto>(user);
                    return userToReturn;
                }

                return null;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error Details: {ex.Message}");
            }
        }

        public async Task<UserUpdateDto> UpdateAccount(UserUpdateDto userDto)
        {
            try
            {
                var user = await this.UserPersist
                    .GetUserByUsernameAsync(userDto.UserName);
                if (user == null) return null;

                userDto.Id = user.Id;

                this.Mapper.Map(userDto, user);

                if(!String.IsNullOrEmpty(userDto.Password)) {
                    var token = await this.UserManager
                        .GeneratePasswordResetTokenAsync(user);

                    await this.UserManager
                        .ResetPasswordAsync(user, token, userDto.Password);
                }

                this.UserPersist.Update<User>(user);

                if (await this.UserPersist.SaveChangesAsync())
                {
                    var userReturn = await this.UserPersist.GetUserByIdAsync(user.Id);
                    return this.Mapper.Map<UserUpdateDto>(userReturn);
                }

                return null;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error Details: {ex.Message}");
            }
        }

        public IEnumerable<string> GetAvailableTitles()
        {
            return Enum.GetNames(typeof (Title)).ToList();
        }

        public IEnumerable<string> GetAvailableFunctions()
        {
            return Enum.GetNames(typeof (Function)).ToList();
        }
    }
}
