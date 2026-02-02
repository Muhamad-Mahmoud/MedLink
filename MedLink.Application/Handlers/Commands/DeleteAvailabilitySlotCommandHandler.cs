using MediatR;
using MedLink.Application.Commands;
using MedLink_Application.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedLink.Application.Handlers.Commands
{

    public class DeleteAvailabilitySlotCommandHandler : IRequestHandler<DeleteAvailabilitySlotCommand, Unit>
    {
        private readonly IDoctorAvailabilityRepository _repository;

        public DeleteAvailabilitySlotCommandHandler(IDoctorAvailabilityRepository repository)
        {
            _repository = repository;
        }

        public async Task<Unit> Handle(DeleteAvailabilitySlotCommand request, CancellationToken cancellationToken)
        {
            // 1. Get the slot
            var slot = await _repository.GetByIdAsync(request.SlotId);
            if (slot == null)
                throw new KeyNotFoundException($"Availability slot with ID {request.SlotId} not found");

            // 2. Check if already booked
            if (slot.IsBooked)
                throw new InvalidOperationException("Cannot delete a booked slot");

            // 3. Soft delete
            await _repository.DeleteAsync(slot);

            return Unit.Value;
        }
    }

}
