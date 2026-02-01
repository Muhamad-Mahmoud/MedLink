using AutoMapper;
using MediatR;
using MedLink.Domain.Entities.Appointments;
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

    public class CreateAppointmentCommandHandler : IRequestHandler<CreateAppointmentCommand, AppointmentDto>
    {
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly IDoctorAvailabilityRepository _availabilityRepository;
        private readonly IMapper _mapper;

        public CreateAppointmentCommandHandler(
            IAppointmentRepository appointmentRepository,
            IDoctorAvailabilityRepository availabilityRepository,
            IMapper mapper)
        {
            _appointmentRepository = appointmentRepository;
            _availabilityRepository = availabilityRepository;
            _mapper = mapper;
        }

        public async Task<AppointmentDto> Handle(CreateAppointmentCommand request, CancellationToken cancellationToken)
        {
            
            var schedule = await _availabilityRepository.GetByIdAsync(request.ScheduleId);
            if (schedule == null)
                throw new KeyNotFoundException($"Schedule with ID {request.ScheduleId} not found");

          
            if (schedule.IsBooked)
                throw new InvalidOperationException("This time slot is already booked");

            var scheduleDateTime = schedule.Date.Add(schedule.StartTime);
            if (scheduleDateTime <= DateTime.UtcNow)
                throw new InvalidOperationException("Cannot book appointments in the past");

            decimal fee = schedule.Doctor?.ConsultationFee ?? 0;
            if (fee <= 0)
                throw new InvalidOperationException("Doctor consultation fee not set");

            var appointment = new Appointment
            {
                DoctorId = schedule.DoctorId,
                ScheduleId = request.ScheduleId,
                PatientName = request.PatientName,
                PatientPhone = request.PatientPhone,
                PatientEmail = request.PatientEmail,
                Notes = request.Notes,
                BookedByUserId = request.BookedByUserId,
                Status = AppointmentStatus.Pending,    
                Fee = fee,
                CreatedAt = DateTime.UtcNow
            };

            schedule.IsBooked = true;
            await _availabilityRepository.UpdateAsync(schedule);

            var savedAppointment = await _appointmentRepository.AddAsync(appointment);

            var appointmentDto = _mapper.Map<AppointmentDto>(savedAppointment);

            appointmentDto.Date = schedule.Date;
            appointmentDto.StartTime = schedule.StartTime;
            appointmentDto.EndTime = schedule.EndTime;

            return appointmentDto;
        }
    }
}
