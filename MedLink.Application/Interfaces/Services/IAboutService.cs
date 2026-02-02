using MedLink.Application.Interfaces.Specifications;
using MedLink.Domain.Entities.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedLink.Application.Interfaces.Services
{
    public interface IAboutService
    {
        Task<About?> GetAboutByIdAsync(int id);
        
        Task<About> AddAboutAsync(About about);
        Task UpdateAboutAsync(About about);
        Task DeleteAboutAsync(int id);
    }
}
