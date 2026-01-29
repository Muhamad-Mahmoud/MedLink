using System.Linq.Expressions;
using MedLink.Domain.Entities.Medical;
using MedLink_Application.DTOs.Doctors;
using MedLink_Application.Specifications;

namespace MedLink.Application.Specifications.Doctors
{
    public class DoctorSearchSpec : BaseSpecification<Doctor>
    {
        private const double KilometersPerDegree = 111.0;

        public DoctorSearchSpec(DoctorSearchParams searchParams)
            : base(BuildCriteria(searchParams))
        {
            AddIncludes(d => d.Specialization);
            AddIncludes(d => d.Reviews);
            AddIncludes(d => d.Availabilities);
            AddOrderBy(d => d.Name);

            var pageIndex = Math.Max(searchParams.PageIndex, 1);
            var pageSize = Math.Max(searchParams.PageSize, 1);

            ApplyPagination(
                (pageIndex - 1) * pageSize,
                pageSize
            );
        }

        private static Expression<Func<Doctor, bool>> BuildCriteria(DoctorSearchParams p)
        {
            // Prepare inputs
            var keyword = p.Keyword?.Trim().ToLower();
            var hasKeyword = !string.IsNullOrEmpty(keyword);

            var city = p.City?.Trim().ToLower();
            var hasCity = !string.IsNullOrEmpty(city);

            var hasLocation = p.Latitude.HasValue && p.Longitude.HasValue;

            // Calculate geographic boundaries => Square Bounding Box
            double minLat = 0, maxLat = 0, minLng = 0, maxLng = 0;
            if (hasLocation)
            {
                var radiusDeg = p.RadiusInKm / KilometersPerDegree;
                minLat = p.Latitude!.Value - radiusDeg;
                maxLat = p.Latitude.Value + radiusDeg;
                minLng = p.Longitude!.Value - radiusDeg;
                maxLng = p.Longitude.Value + radiusDeg;
            }

            // Date Range Optimization 
            var availableToday = p.AvailableToday;
            var searchDateStart = p.SearchDate.Date;
            var searchDateEnd = searchDateStart.AddDays(1);

            var gender = p.Gender;
            var hasGender = p.Gender.HasValue;

            var specialtyId = p.SpecialtyId;
            var hasSpecialty = p.SpecialtyId.HasValue;

            return d =>
                //  Location Logic 
                (
                     hasCity ? d.City.ToLower() == city :
                     hasLocation ?
                     (d.Latitude >= minLat && d.Latitude <= maxLat &&
                                    d.Longitude >= minLng && d.Longitude <= maxLng) :
                     true
                )

                //  Keyword 
                && (!hasKeyword || (
                    d.Name.Contains(keyword!) ||
                    (d.Specialization != null && d.Specialization.Name.ToLower().Contains(keyword!))
                ))

                //  Filters 
                && (!hasGender || d.Gender == gender)
                && (!hasSpecialty || d.SpecialtyId == specialtyId)
                && (!availableToday || d.Availabilities.Any(a =>
                       a.Date >= searchDateStart && a.Date < searchDateEnd));
        }
    }
}
