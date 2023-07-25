#pragma warning disable
using ArzonOL.Dtos.AuthDtos;
using ArzonOL.Entities;
using ArzonOL.Enums;
using ArzonOL.Services.AuthService.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ArzonOL.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly ILoginService _loginService;
        private readonly IRegisterService _registerService;
        private readonly UserManager<UserEntity> _userManager;
        private readonly IConfiguration _configuration;
        private readonly ISmsService _smsService;
        private readonly IMemoryCache _cache;
        private string AdditionalKeyword = "keyword";

        public AuthController(ILoginService loginService,
                              IRegisterService registerService,
                              UserManager<UserEntity> userManager,
                              IConfiguration configuration,
                              ISmsService smsService,
                              IMemoryCache cache)
        {
            _loginService = loginService;
            _registerService = registerService;
            _userManager = userManager;
            _configuration = configuration;
            _smsService = smsService;
            _cache = cache;
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync(LoginDto loginDto)
        {
            try
            {
                var token = await _loginService.LogInAsync(loginDto.UserName!, loginDto.Password!);

                if (string.IsNullOrEmpty(token))
                    return BadRequest("Username or password is incorrect");

                var cookieOptions = new CookieOptions
                {
                    Expires = DateTime.UtcNow.AddDays(7), // Set expiration date to 7 days from now
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Strict
                };

                Response.Cookies.Append("AuthToken", token, cookieOptions);

                return Ok(token);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        
        [HttpPost("sms")]
        public async Task<IActionResult> SendSmsAsync(SendSmsDto smsDto)
        {
            try
            {
                var isCorrectPhoneNumber = ValidatePhoneNumber(smsDto.PhoneNumber!);

                if (!isCorrectPhoneNumber.Item1)
                    return BadRequest(isCorrectPhoneNumber.Item2);
                    
                await _smsService.SendSmsAsync(smsDto.PhoneNumber!);

                var cacheEntryOptions = new MemoryCacheEntryOptions()
                                        .SetSlidingExpiration(TimeSpan.FromMinutes(3));

                if (_cache.TryGetValue(smsDto.PhoneNumber+AdditionalKeyword, out string? existingCode))
                {
                    _cache.Remove(smsDto.PhoneNumber+AdditionalKeyword);
                }
                var passwordAndUserName = smsDto.Password+"#"+smsDto.UserName;

                _cache.Set(smsDto.PhoneNumber+AdditionalKeyword, passwordAndUserName, cacheEntryOptions);

                return Ok($"Sent Sms to {smsDto.PhoneNumber} number");
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        [HttpPost("smsReciver")]
        public async Task<IActionResult> SmsReciver(SmsReciverDto smsReciverDto)
        {
            Console.WriteLine("Startted Reciving Code");

            if (!int.TryParse(smsReciverDto.Code, out int result))
            return BadRequest("Code only can be number");

            var validatedPhoneResult = ValidatePhoneNumber(smsReciverDto.PhoneNumber);

            if(!validatedPhoneResult.Item1)
            return BadRequest(validatedPhoneResult.Item2);

            var validatedCode =  _smsService.ValidateSmsCode(smsReciverDto.PhoneNumber, smsReciverDto.Code);

            if(!validatedCode)
            return BadRequest("This code is false");
            
            if (_cache.TryGetValue(smsReciverDto.PhoneNumber+AdditionalKeyword, out string? passwordAndUserName))
            {
                var splittedString =  StringSplitter(passwordAndUserName);

                var registerResult = await _registerService.RegisterAsync(splittedString[1], splittedString[0], "User", "email");

                if (!registerResult.Succeeded)
                    return BadRequest(registerResult.Errors);
                
                return Ok(await LoginAsync(
                    new LoginDto
                    {
                        UserName = splittedString[1],
                        Password = splittedString[0]
                    }
                ));
            }

            return BadRequest("Something went wrong");
        }

        private (bool, string) ValidatePhoneNumber(string phoneNumber)
        {
            if (!float.TryParse(phoneNumber, out float result))
            return (false, "Phone number can't be digit or simvoll");

            if (phoneNumber.Length != 9)
            {
                return (false, "Phone number must be 9 digits");
            }
            if (string.IsNullOrEmpty(phoneNumber))
            {
                return (false, "Phone number can't be empty");
            }

            return (true, "Phone number is correct");
        }
        private IList<string> StringSplitter(string names)
        {
            try
            {
            string[] result = names.Split('#');

            return result;
            }
            catch (System.Exception)
            {
                
                throw;
            }
        }
    }


}
