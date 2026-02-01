using MediatR;
using MedLink.Domain.Enums;
using MedLink_Application.Commands;
using MedLink_Application.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedLink_Application.Handlers.Commands
{

    public class CompleteAppointmentCommandHandler : IRequestHandler<CompleteAppointmentCommand, Unit>
    {
        private readonly IAppointmentRepository _appointmentRepository;

        public CompleteAppointmentCommandHandler(IAppointmentRepository appointmentRepository)
        {
            _appointmentRepository = appointmentRepository;
        }

        public async Task<Unit> Handle(CompleteAppointmentCommand request, CancellationToken cancellationToken)
        {
            // 1. Get the appointment
            var appointment = await _appointmentRepository.GetByIdAsync(request.AppointmentId);
            if (appointment == null)
                throw new KeyNotFoundException($"Appointment with ID {request.AppointmentId} not found");

            // 2. Verify appointment is Confirmed
            if (appointment.Status != AppointmentStatus.Confirmed)
                throw new InvalidOperationException($"Can only complete Confirmed appointments. Current status: {appointment.Status}");

            // 3. Update status
            appointment.Status = AppointmentStatus.Completed;
            appointment.UpdatedAt = DateTime.UtcNow;

            // 4. Save changes
            await _appointmentRepository.UpdateAsync(appointment);

            return Unit.Value;
        }
    }

}
