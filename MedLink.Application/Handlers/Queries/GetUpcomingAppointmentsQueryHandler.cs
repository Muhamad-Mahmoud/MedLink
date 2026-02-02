using AutoMapper;
using MediatR;
using MedLink.Domain.Enums;
using MedLink_Application.Interfaces.Repositories;
using MedLink_Application.Queries;
using MedLink_Application.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedLink_Application.Handlers.Queries
{
    public class GetUpcomingAppointmentsQueryHandler : IRequestHandler<GetUpcomingAppointmentsQuery, List<AppointmentDto>>
    {
        private readonly IAppointmentRepository _repository;
        private readonly IMapper _mapper;

        public GetUpcomingAppointmentsQueryHandler(IAppointmentRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<List<AppointmentDto>> Handle(GetUpcomingAppointmentsQuery request, CancellationToken cancellationToken)
        {
            // Get all user appointments
            var appointments = await _repository.GetAppointmentsByUserAsync(request.UserId);

            // Filter upcoming appointments only (Pending or Confirmed status, and future dates)
            var upcomingAppointments = appointments
                .Where(a =>
                    (a.Status == AppointmentStatus.Pending ||
                     a.Status == AppointmentStatus.Confirmed) &&
                    a.Schedule.Date.Add(a.Schedule.StartTime) > DateTime.UtcNow)
                .OrderBy(a => a.Schedule.Date)
                .ThenBy(a => a.Schedule.StartTime)
                .ToList();

            return _mapper.Map<List<AppointmentDto>>(upcomingAppointments);
        }
    }
}
