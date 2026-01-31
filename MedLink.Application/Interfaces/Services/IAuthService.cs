using MedLink.Application.DTOs.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedLink.Application.Interfaces.Services
{
    public interface IAuthService
    {
        Task<AuthModel> RegisterAsync(RegisterModel registerModel);
        Task<AuthModel> GetTokenAsync(RequestTokenModel model);

    }
}
