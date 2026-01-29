using MedLink.Domain.Entities.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedLink_Application.Specifications
{
    public class UserProfileSpec:BaseSpecification<UserProfile>
    {
        public UserProfileSpec(int pageIndex, int pageSize)
        {
            ApplyPagination(
                skip: (pageIndex - 1) * pageSize,
                take: pageSize
            );

            AddOrderBy(x => x.Id);
        }
    }
}
