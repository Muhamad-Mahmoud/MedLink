using MedLink.Domain.Entities.Medical;
using MedLink_Application.Specifications;

namespace MedLink.Application.Specifications.Medical
{
    public class GetAllSpecialtiesSpec : BaseSpecification<Specialization>
    {
        public GetAllSpecialtiesSpec(int? count = null)
        {
            AddOrderBy(s => s.Name);

            if (count.HasValue)
            {
                ApplyPagination(0, count.Value);
            }
        }
    }
}
