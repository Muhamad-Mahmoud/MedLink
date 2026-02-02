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

    public class RescheduleAppointmentCommandHandler : IRequestHandler<RescheduleAppointmentCommand, AppointmentDto>
    {
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly IDoctorAvailabilityRepository _availabilityRepository;
        private readonly IMapper _mapper;

        public RescheduleAppointmentCommandHandler(
            IAppointmentRepository appointmentRepository,
            IDoctorAvailabilityRepository availabilityRepository,
            IMapper mapper)
        {
            _appointmentRepository = appointmentRepository;
            _availabilityRepository = availabilityRepository;
            _mapper = mapper;
        }

        public async Task<AppointmentDto> Handle(RescheduleAppointmentCommand request, CancellationToken cancellationToken)
        {
            // 1. Get the appointment
            var appointment = await _appointmentRepository.GetByIdWithDetailsAsync(request.AppointmentId);
            if (appointment == null)
                throw new KeyNotFoundException($"Appointment with ID {request.AppointmentId} not found");

            // 2. Verify ownership
            if (appointment.BookedByUserId != request.RescheduledByUserId)
                throw new UnauthorizedAccessException("You are not authorized to reschedule this appointment");

            // 3. Verify appointment can be rescheduled (not Cancelled or Completed)
            if (appointment.Status == AppointmentStatus.Cancelled || appointment.Status == AppointmentStatus.Completed)
                throw new InvalidOperationException($"Cannot reschedule appointment with status {appointment.Status}");

            // 4. Get old schedule
            var oldSchedule = await _availabilityRepository.GetByIdAsync(appointment.ScheduleId);

            // 5. Get new schedule
            var newSchedule = await _availabilityRepository.GetByIdAsync(request.NewScheduleId);
            if (newSchedule == null)
                throw new KeyNotFoundException($"Schedule with ID {request.NewScheduleId} not found");

            // 6. Verify new schedule is available
            if (newSchedule.IsBooked)
                throw new InvalidOperationException("The new time slot is already booked");

            // 7. Verify new schedule is in the future
            var newScheduleDateTime = newSchedule.Date.Add(newSchedule.StartTime);
            if (newScheduleDateTime <= DateTime.UtcNow)
                throw new InvalidOperationException("Cannot reschedule to a past time");

            // 8. Verify same doctor
            if (newSchedule.DoctorId != appointment.DoctorId)
                throw new InvalidOperationException("New schedule must be for the same doctor");

            // 9. Release old schedule
            if (oldSchedule != null)
            {
                oldSchedule.IsBooked = false;
                await _availabilityRepository.UpdateAsync(oldSchedule);
            }

            // 10. Book new schedule
            newSchedule.IsBooked = true;
            await _availabilityRepository.UpdateAsync(newSchedule);

            // 11. Update appointment
            appointment.ScheduleId = request.NewScheduleId;
            appointment.UpdatedAt = DateTime.UtcNow;

            var updatedAppointment = await _appointmentRepository.UpdateAsync(appointment);

            // 12. Return DTO
            return _mapper.Map<AppointmentDto>(updatedAppointment);
        }
    }
}
