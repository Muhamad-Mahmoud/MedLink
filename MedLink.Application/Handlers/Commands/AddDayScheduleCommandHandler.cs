using AutoMapper;
using MediatR;
using MedLink.Application.Commands;
using MedLink.Application.Responses;
using MedLink_Application.Interfaces.Repositories;
using MedLink_Application.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedLink.Application.Handlers.Commands
{
    public class AddDayScheduleCommandHandler : IRequestHandler<AddDayScheduleCommand, DayScheduleResponse>
    {
        private readonly IDoctorAvailabilityRepository _repository;
        private readonly IMapper _mapper;

        public AddDayScheduleCommandHandler(
            IDoctorAvailabilityRepository repository,
            IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<DayScheduleResponse> Handle(AddDayScheduleCommand request, CancellationToken cancellationToken)
        {
            // 1. Validation
            if (request.Date.Date < DateTime.UtcNow.Date)
                throw new InvalidOperationException("Cannot add slots for past dates");

            if (request.SlotDurationMinutes <= 0 || request.SlotDurationMinutes > 240)
                throw new InvalidOperationException("Slot duration must be between 1 and 240 minutes");

            // 2. Parse times
            var startTime = TimeSpan.Parse(request.StartTime);
            var endTime = TimeSpan.Parse(request.EndTime);

            if (startTime >= endTime)
                throw new InvalidOperationException("Start time must be before end time");

            // 3. Generate all time slots
            var availabilities = new List<Domain.Entities.Appointments.DoctorAvailability>();
            var currentTime = startTime;

            while (currentTime < endTime)
            {
                var slotEndTime = currentTime.Add(TimeSpan.FromMinutes(request.SlotDurationMinutes));

                // Don't create a slot that extends beyond the end time
                if (slotEndTime > endTime)
                    break;

                var availability = new Domain.Entities.Appointments.DoctorAvailability
                {
                    DoctorId = request.DoctorId,
                    Date = request.Date.Date,
                    StartTime = currentTime,
                    EndTime = slotEndTime,
                    IsBooked = false,
                    CreatedAt = DateTime.UtcNow,
                    IsDeleted = false
                };

                availabilities.Add(availability);
                currentTime = slotEndTime; // Move to next slot
            }

            // 4. Save all slots
            var created = await _repository.AddRangeAsync(availabilities);

            // 5. Return response
            return new DayScheduleResponse
            {
                Success = true,
                Date = request.Date.Date,
                TotalSlotsCreated = created.Count,
                Message = $"Successfully created {created.Count} slots for {request.Date:yyyy-MM-dd} from {request.StartTime} to {request.EndTime} ({request.SlotDurationMinutes} min each)",
                Slots = _mapper.Map<List<DoctorAvailabilityDto>>(created)
            };
        }
    }
}
