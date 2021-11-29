using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProEventos.API.Extensions;
using ProEventos.Application.Contracts;
using ProEventos.Application.Dtos;

namespace ProEventos.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly ITokenService _tokenService;

        public AccountController(IAccountService accountService, 
            ITokenService tokenService)
        {
            this._accountService = accountService;
            this._tokenService = tokenService;
        }

        [HttpGet]
        public async Task<IActionResult> GetUser(){
            try
            {
                string username = this.User.GetUserName();
                var user = await this._accountService.GetUserByUsername(username);
                return Ok(user);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    ex.Message);
            }
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(UserLoginDto userLoginDto){
            try
            {
                var user = await this._accountService
                    .GetUserByUsername(userLoginDto.UserName);

                if(user == null) return Unauthorized($"Invalid User.");

                var result = await this._accountService
                    .CheckUserPassword(user, userLoginDto.Password);

                if(!result.Succeeded) return Unauthorized($"Invalid Password.");

                return Ok(new {
                    userName = user.UserName,
                    firstName = user.FirstName,
                    token = this._tokenService.CreateToken(user).Result
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    ex.Message);
            }
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Register(UserDto userDto) {
            try
            {
                if(await this._accountService.UserExists(userDto.UserName)) {
                    return BadRequest($"User {userDto.UserName} already exists");
                }

                var user = await this._accountService.CreateAccount(userDto);

                if(user != null) {
                    return StatusCode(StatusCodes.Status201Created, new {
                        userName = user.UserName,
                        firstName = user.FirstName,
                        token = this._tokenService.CreateToken(user).Result
                    });
                }

                return BadRequest($"User not created, try again.");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    ex.Message);
            }
        }

        [HttpPut]
        public async Task<IActionResult> UpdateUser(UserUpdateDto userUpdateDto) {
            try
            {
                string username = User.GetUserName();

                if(userUpdateDto.UserName != username) {
                    return Unauthorized("Invalid User");
                }

                var user = await this._accountService
                    .GetUserByUsername(username);

                if(user == null) return BadRequest($"Invalid User.");

                var userReturn = await this._accountService
                    .UpdateAccount(userUpdateDto);

                if(userReturn == null) return NoContent();

                return Ok(new {
                    userName = userReturn.UserName,
                    firstName = userReturn.FirstName,
                    token = this._tokenService.CreateToken(userReturn).Result
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    ex.Message);
            }
        }

        [HttpGet]
        public IActionResult GetTitles() {
            var titlesList = _accountService.GetAvailableTitles();
            return Ok(titlesList);
        }

        [HttpGet]
        public IActionResult GetFunctions() {
            var functionsList = _accountService.GetAvailableFunctions();
            return Ok(functionsList);
        }
    }
}
