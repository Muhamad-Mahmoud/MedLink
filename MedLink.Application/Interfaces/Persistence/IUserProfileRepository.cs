using MedLink.Domain.Entities.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedLink_Application.Interfaces.Persistence
{
    public interface IUserProfileRepository
    {
        Task<UserProfile?> GetByUserIdAsync(int userId);
        Task AddAsync(UserProfile profile);
        void Update(UserProfile profile);
    }
}
