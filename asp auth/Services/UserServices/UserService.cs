using asp_auth.Models.Constants;
using asp_auth.Models.DTOs;
using asp_auth.Models.Entities;
using asp_auth.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Policy;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace asp_auth.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<User> _userManager;
        //private UserManager<User> _userManager;
        private readonly IRepositoryWrapper _repository;

        public UserService(UserManager<User> userManager, IRepositoryWrapper repository)
        {
            _userManager = userManager;
            _repository = repository;
        }

        public async Task<string> RegisterUserAsync(RegisterUserDTO dto)
        {
            var registerUser = new User();
            registerUser.Email = dto.Email;
            registerUser.UserName = dto.Username;

            // email validation check
            string email = dto.Email;
            string pattern = @"^[a-z][a-z|0-9|]*([_][a-z|0-9]+)*([.][a-z|0-9]+([_][a-z|0-9]+)*)?@[a-z][a-z|0-9|]*\.([a-z][a-z|0-9]*(\.[a-z][a-z|0-9]*)?)$";
            Match match = Regex.Match(email.Trim(), pattern, RegexOptions.IgnoreCase);

            if (!match.Success)
            {
                return "Please enter a valid email.";
            }

            var user = await _repository.User.GetUserByEmail(dto.Email);
            if (user != null)
            {
                Console.WriteLine("email taken by user " + user);
                return "Email already used.";
            }

            var result = await _userManager.CreateAsync(registerUser, dto.Password);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(registerUser, UserRoleType.User);
                //await _userManager.AddToRoleAsync(registerUser, UserRoleType.Admin);
                
                return "true";
            }
            else if (result.Errors.Count() > 0)
            {
                var err = result.Errors.First();
                return err.Description;
            }
            return "false";
        }

        public async Task<string> LoginUser(LoginUserDTO dto)
        {
            User user = await _userManager.FindByNameAsync(dto.Username);
            
            if (user != null)
            {
                if (GlobalConstants.sendEmailConfirmation)
                {
                    if (user.EmailConfirmed == false)
                        return "Please confirm your email.";
                }

                var passwordMatch = await _userManager.CheckPasswordAsync(user, dto.Password);
                if (passwordMatch == false)
                {
                    return "Wrong password.";
                }

                user = await _repository.User.GetByIdWithRoles(user.Id);

                List<string> roles = user.UserRoles.Select(ur => ur.Role.Name).ToList();

                var newJti = Guid.NewGuid().ToString();

                var tokenHandler = new JwtSecurityTokenHandler();

                var signinKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("cheia magica care aparent trebuie sa aiba o dimensiune mai mare hahahaha"));


                var token = GenerateJwtToken(signinKey, user, roles, tokenHandler, newJti);

                _repository.SessionToken.Create(new SessionToken(newJti, user.Id, token.ValidTo));
                await _repository.SaveAsync();

                return tokenHandler.WriteToken(token);
            }

            return "User doesnt exist.";
        }

        private SecurityToken GenerateJwtToken(SymmetricSecurityKey signinKey, User user, List<string> roles, JwtSecurityTokenHandler tokenHandler, string jti)
        {
            var subject = new ClaimsIdentity(new Claim[]{
                    new Claim(ClaimTypes.Email, user.Email),
                    //new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(ClaimTypes.Name, user.Id.ToString()),
                    new Claim(ClaimTypes.NameIdentifier, user.UserName),
                    //new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(JwtRegisteredClaimNames.Jti, jti)
                });

            foreach (var role in roles)
            {
                subject.AddClaim(new Claim(ClaimTypes.Role, role));
            }

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = subject,
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = new SigningCredentials(signinKey, SecurityAlgorithms.HmacSha256)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return token;
        }

        public string tokenToJWT(string token)
        {
            var newJti = Guid.NewGuid().ToString();

            var tokenHandler = new JwtSecurityTokenHandler();

            var signinKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("cheia magica care aparent trebuie sa aiba o dimensiune mai mare hahahaha"));

            var subject = new ClaimsIdentity(new Claim[]{
                    new Claim(ClaimTypes.Name, token),
                    new Claim(JwtRegisteredClaimNames.Jti, newJti)
                });

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = subject,
                Expires = DateTime.Now.AddHours(1),
                SigningCredentials = new SigningCredentials(signinKey, SecurityAlgorithms.HmacSha256)
            };

            var jwt_token = tokenHandler.CreateToken(tokenDescriptor);
            Console.WriteLine("original token before " + jwt_token);
            return tokenHandler.WriteToken(jwt_token); ;
        }

        public async Task<string> ConfirmEmail(string username, string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            var original_jwt_token = tokenHandler.ReadToken(token);
            var original_jwt_token_str = original_jwt_token.ToString();
            Console.WriteLine("\ntoken after reading from param " + original_jwt_token_str);
            original_jwt_token_str = original_jwt_token_str.Split(new char[] { '.' })[1];
            JObject json = JObject.Parse(original_jwt_token_str);
            Console.WriteLine("\njson " + json);
            string original_token = (string)json["unique_name"];
            Console.WriteLine("\noriginal token after " + original_token);
            //original_token.

            var user = await _userManager.FindByNameAsync(username);

            if (user == null)
                return "rip";
            Console.WriteLine("\n user " + user);
            //_userManager.ConfirmEmailAsync()
            try
            {
                await _userManager.ConfirmEmailAsync(user, original_token);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return "done";
        }
    }
}
