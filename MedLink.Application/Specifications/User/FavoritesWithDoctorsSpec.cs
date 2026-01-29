using MedLink.Domain.Entities.User;
using MedLink_Application.Specifications;

namespace MedLink.Application.Specifications.User
{
    public class FavoritesWithDoctorsSpec : BaseSpecification<Favorite>
    {
        public FavoritesWithDoctorsSpec(string userId)
            : base(f => f.UserId == userId)
        {
            AddIncludes(f => f.Doctor);
            AddIncludes(f => f.Doctor.Specialization);
            AddIncludes(f => f.Doctor.Reviews);
        }

        public FavoritesWithDoctorsSpec(string userId, int doctorId)
            : base(f => f.UserId == userId && f.DoctorId == doctorId)
        {
        }
    }
}
