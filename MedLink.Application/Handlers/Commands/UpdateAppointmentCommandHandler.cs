using AutoMapper;
using MediatR;
using MedLink.Domain.Enums;
using MedLink_Application.Commands;
using MedLink_Application.Interfaces.Repositories;
using MedLink_Application.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedLink_Application.Handlers.Commands
{

    public class UpdateAppointmentCommandHandler : IRequestHandler<UpdateAppointmentCommand, AppointmentDto>
    {
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly IMapper _mapper;

        public UpdateAppointmentCommandHandler(
            IAppointmentRepository appointmentRepository,
            IMapper mapper)
        {
            _appointmentRepository = appointmentRepository;
            _mapper = mapper;
        }

        public async Task<AppointmentDto> Handle(UpdateAppointmentCommand request, CancellationToken cancellationToken)
        {
            // 1. Get the appointment
            var appointment = await _appointmentRepository.GetByIdWithDetailsAsync(request.AppointmentId);
            if (appointment == null)
                throw new KeyNotFoundException($"Appointment with ID {request.AppointmentId} not found");

            // 2. Verify ownership
            if (appointment.BookedByUserId != request.UpdatedByUserId)
                throw new UnauthorizedAccessException("You are not authorized to update this appointment");

            // 3. Verify appointment can be updated (only Pending appointments)
            if (appointment.Status != AppointmentStatus.Pending)
                throw new InvalidOperationException($"Cannot update appointment with status {appointment.Status}");

            // 4. Update fields
            appointment.PatientName = request.PatientName;
            appointment.PatientPhone = request.PatientPhone;
            appointment.PatientEmail = request.PatientEmail;
            appointment.Notes = request.Notes;
            appointment.UpdatedAt = DateTime.UtcNow;

            // 5. Save changes
            var updatedAppointment = await _appointmentRepository.UpdateAsync(appointment);

            // 6. Return DTO
            return _mapper.Map<AppointmentDto>(updatedAppointment);
        }
    }
}
