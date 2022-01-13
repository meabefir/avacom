using asp_auth.Models.DTOs;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace asp_auth.Services.UserServices
{
    public interface IUserService
    {
        Task<string> RegisterUserAsync(RegisterUserDTO dto);
        Task<String> LoginUser(LoginUserDTO dto);

        string tokenToJWT(string token);

        Task<string> ConfirmEmail(string username, string token);
    }
}
