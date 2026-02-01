using AutoMapper;
using MedLink.Domain.Identity;
using MedLink_Application.Common.JWT;
using MedLink_Application.DTOs.Identity;
using MedLink_Application.Interfaces.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MedLink_Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly Jwt _jwt;
        private readonly IMapper _mapper;
        private readonly IProfileService _userProfileService;

        public AuthService(UserManager<ApplicationUser> userManager, IOptions<Jwt> jwt, IMapper mapper, IProfileService userProfileService)
        {
            _userManager = userManager;
            _jwt = jwt.Value;
            _mapper = mapper;
            _userProfileService = userProfileService;
        }


        public async Task<AuthModel> RegisterAsync(RegisterModel model)
        {

            var isEmail = new EmailAddressAttribute().IsValid(model.Email);

            var existingUser = await _userManager.FindByEmailAsync(model.Email);


            if (existingUser is not null)
                return new AuthModel { Message = "User already registered" };

            var user = _mapper.Map<ApplicationUser>(model);

            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                return new AuthModel { Message = errors };
            }

            await _userProfileService.CreateAsync(user.Id, model.FullName);


            await _userManager.AddToRoleAsync(user, "User");

            var token = await CreateJwtToken(user);

            return new AuthModel
            {
                Message = "Email was registered successfully",
                Email = user.Email,
                Username = model.Email.Split('@')[0],
                IsAuthenticated = true,
                ExpiresOn = token.ValidTo,
                Roles = new List<string> { "User" },
                Token = new JwtSecurityTokenHandler().WriteToken(token)
            };


        }



        public async Task<AuthModel> GetTokenAsync(RequestTokenModel model)
        {
            var authModel = new AuthModel();

            var user =  await _userManager.FindByEmailAsync(model.Email);

            if (user is null || !await _userManager.CheckPasswordAsync(user, model.Password))
            {
                authModel.Message = "Email/Phone or Password is incorrect!";
                return authModel;
            }

            var token = await CreateJwtToken(user);
            authModel.Message = "Token created successfully";
            authModel.IsAuthenticated = true;
            authModel.Email = user.Email;
            authModel.Username = user.UserName;
            authModel.Roles = (await _userManager.GetRolesAsync(user)).ToList();
            authModel.ExpiresOn = token.ValidTo;
            authModel.Token = new JwtSecurityTokenHandler().WriteToken(token);

            return authModel;
        }

        private async Task<JwtSecurityToken> CreateJwtToken(ApplicationUser user)
        {
            var userClaims = await _userManager.GetClaimsAsync(user);
            var roles = await _userManager.GetRolesAsync(user);
            var roleClaims = new List<Claim>();

            foreach (var role in roles)
            {
                roleClaims.Add(new Claim(ClaimTypes.Role, role));
            }

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub,user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
                new Claim("uid",user.Id)
            };

            if (!string.IsNullOrWhiteSpace(user.Email))
                claims.Add(new Claim(JwtRegisteredClaimNames.Email, user.Email));

            if (!string.IsNullOrWhiteSpace(user.PhoneNumber))
                claims.Add(new Claim("phone_number", user.PhoneNumber));

            claims.AddRange(userClaims);
            claims.AddRange(roleClaims);


            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _jwt.Issuer,
                audience: _jwt.Audience,
                claims: claims,
                expires: DateTime.Now.AddDays(Convert.ToDouble(_jwt.DurationInDays)),
                signingCredentials: signingCredentials
            );

            return jwtSecurityToken;
        }


    }
}