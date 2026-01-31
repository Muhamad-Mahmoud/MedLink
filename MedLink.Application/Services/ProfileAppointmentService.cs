using MedLink.Domain.Entities.Appointments;
using MedLink.Domain.Enums;
using MedLink_Application.DTOs.UserProfile;
using MedLink_Application.Interfaces.Persistence;
using MedLink_Application.Interfaces.Services;
using MedLink_Application.Specifications;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MedLink.Infrastructure.Services
{
    public class ProfileAppointmentService : IProfileAppointmentService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProfileAppointmentService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task CancelAsync(string userId, int appointmentId, string? reason)
        {
            var appointment = await _unitOfWork
                .Repository<Appointment>()
                .GetByIdAsync(appointmentId);

            if (appointment == null)
                throw new Exception("Appointment not found");

            if (appointment.UserId != userId)
                throw new UnauthorizedAccessException();

            if (appointment.Status == AppointmentStatus.Cancelled)
                return;

            var isPast =
                appointment.Schedule.Date < DateTime.UtcNow.Date ||
                (
                    appointment.Schedule.Date == DateTime.UtcNow.Date &&
                    appointment.Schedule.StartTime < DateTime.UtcNow.TimeOfDay
                );

            if (isPast)
                throw new Exception("Cannot cancel past appointment");

            appointment.Status = AppointmentStatus.Cancelled;
            appointment.CancelledReason = reason;

            _unitOfWork.Repository<Appointment>().Update(appointment);
            await _unitOfWork.Complete();
        }


        public async Task<IReadOnlyList<AppointmentListItemDto>> GetPastAsync(string userId)
        {
            var spec = new PastAppointmentsByUserSpec(userId);

            var appointments = await _unitOfWork
                .Repository<Appointment>()
                .GetAllWithSpecAsync(spec);

            return appointments.Select(a => new AppointmentListItemDto
            {
                AppointmentId = a.Id,

                DoctorName = a.Doctor.Name,
                DoctorImageUrl = a.Doctor.ImageUrl ?? "",
                Specialization = a.Doctor.Specialization.Name,

                Date = a.Schedule.Date,
                StartTime = a.Schedule.StartTime,

                Status = a.Status,

                CanCancel = false
            })
            .ToList();
        }


        public async Task<IReadOnlyList<AppointmentListItemDto>> GetUpcomingAsync(string userId)
        {
            var spec = new UpcomingAppointmentsByUserSpec(userId);

            var appointments = await _unitOfWork
                .Repository<Appointment>()
                .GetAllWithSpecAsync(spec);

            return appointments.Select(a => new AppointmentListItemDto
            {
                AppointmentId = a.Id,

                DoctorName = a.Doctor.Name,
                DoctorImageUrl = a.Doctor.ImageUrl ?? "",
                Specialization = a.Doctor.Specialization.Name,

                Date = a.Schedule.Date,
                StartTime = a.Schedule.StartTime,

                Status = a.Status,

                CanCancel =
                    a.Status != AppointmentStatus.Cancelled &&
                    (
                        a.Schedule.Date > DateTime.UtcNow.Date ||
                        (
                            a.Schedule.Date == DateTime.UtcNow.Date &&
                            a.Schedule.StartTime > DateTime.UtcNow.TimeOfDay
                        )
                    ),
            })
            .ToList();
        }

    }
}