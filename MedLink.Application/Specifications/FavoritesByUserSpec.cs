using MedLink.Domain.Entities.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedLink_Application.Specifications
{
    public class FavoritesByUserSpec : BaseSpecification<Favorite>
    {
        public FavoritesByUserSpec(string userId)
            : base(f => f.UserId == userId)
        {
        }
    }

}
