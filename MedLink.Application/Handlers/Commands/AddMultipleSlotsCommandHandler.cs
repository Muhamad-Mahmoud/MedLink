using AutoMapper;
using MediatR;
using MedLink.Application.Commands;
using MedLink_Application.Interfaces.Repositories;
using MedLink_Application.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedLink.Application.Handlers.Commands
{
    public class AddMultipleSlotsCommandHandler : IRequestHandler<AddMultipleSlotsCommand, List<DoctorAvailabilityDto>>
    {
        private readonly IDoctorAvailabilityRepository _repository;
        private readonly IMapper _mapper;

        public AddMultipleSlotsCommandHandler(
            IDoctorAvailabilityRepository repository,
            IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<List<DoctorAvailabilityDto>> Handle(AddMultipleSlotsCommand request, CancellationToken cancellationToken)
        {
            // 1. Validation
            if (request.StartDate.Date < DateTime.UtcNow.Date)
                throw new InvalidOperationException("Cannot add slots for past dates");

            if (request.StartDate > request.EndDate)
                throw new InvalidOperationException("Start date must be before end date");

            if (!request.WorkingDays.Any())
                throw new InvalidOperationException("At least one working day must be specified");

            if (!request.TimeSlots.Any())
                throw new InvalidOperationException("At least one time slot must be specified");

            // 2. Generate all slots
            var availabilities = new List<Domain.Entities.Appointments.DoctorAvailability>();
            var currentDate = request.StartDate.Date;

            while (currentDate <= request.EndDate.Date)
            {
                // Check if this day is a working day
                if (request.WorkingDays.Contains(currentDate.DayOfWeek))
                {
                    // Add all time slots for this day
                    foreach (var timeSlot in request.TimeSlots)
                    {
                        var availability = new Domain.Entities.Appointments.DoctorAvailability
                        {
                            DoctorId = request.DoctorId,
                            Date = currentDate,
                            StartTime = timeSlot.StartTime,
                            EndTime = timeSlot.EndTime,
                            IsBooked = false,
                            CreatedAt = DateTime.UtcNow,
                            IsDeleted = false
                        };

                        availabilities.Add(availability);
                    }
                }

                currentDate = currentDate.AddDays(1);
            }

            // 3. Save all slots
            var created = await _repository.AddRangeAsync(availabilities);

            // 4. Return DTOs
            return _mapper.Map<List<DoctorAvailabilityDto>>(created);
        }
    }
}
