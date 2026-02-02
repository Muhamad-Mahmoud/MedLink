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

    public class AddAvailabilitySlotCommandHandler : IRequestHandler<AddAvailabilitySlotCommand, DoctorAvailabilityDto>
    {
        private readonly IDoctorAvailabilityRepository _repository;
        private readonly IMapper _mapper;

        public AddAvailabilitySlotCommandHandler(
            IDoctorAvailabilityRepository repository,
            IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<DoctorAvailabilityDto> Handle(AddAvailabilitySlotCommand request, CancellationToken cancellationToken)
        {
            // 1. Validation
            if (request.Date.Date < DateTime.UtcNow.Date)
                throw new InvalidOperationException("Cannot add slots for past dates");

            if (request.StartTime >= request.EndTime)
                throw new InvalidOperationException("Start time must be before end time");

            // 2. Create availability entity
            var availability = new Domain.Entities.Appointments.DoctorAvailability
            {
                DoctorId = request.DoctorId,
                Date = request.Date.Date,
                StartTime = request.StartTime,
                EndTime = request.EndTime,
                IsBooked = false,
                CreatedAt = DateTime.UtcNow,
                IsDeleted = false
            };

            // 3. Save
            var created = await _repository.AddAsync(availability);

            // 4. Return DTO
            return _mapper.Map<DoctorAvailabilityDto>(created);
        }
    }
}
