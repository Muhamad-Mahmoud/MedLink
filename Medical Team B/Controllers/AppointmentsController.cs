using MediatR;
using MedLink_Application.Commands;
using MedLink_Application.Queries;
using MedLink_Application.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace Medical_Team_B.Controllers
{
    //  [Authorize]
    [AllowAnonymous]
    public class AppointmentsController : BaseApiController
    {
        private readonly IMediator _mediator;

        public AppointmentsController(IMediator mediator)
        {
            _mediator = mediator;
        }

      
        // 1. Get Available Slots
        // GET: api/appointments/available-slots?doctorId=1&date=2026-02-01
        
        [HttpGet("available-slots")]
        [AllowAnonymous]
        public async Task<ActionResult<List<DoctorAvailabilityDto>>> GetAvailableSlots(
            [FromQuery] int doctorId,
            [FromQuery] DateTime date)
        {
            var query = new GetAvailableSlotsQuery(doctorId, date);
            var slots = await _mediator.Send(query);
            return Ok(slots);
        }

       
        // 2. Create Appointment
        // POST: api/appointments
       
        [HttpPost]
        public async Task<ActionResult<AppointmentDto>> CreateAppointment([FromBody] CreateAppointmentCommand command)
        {
            var userId = User.FindFirst("sub")?.Value ?? User.FindFirst("id")?.Value;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized("User not authenticated");

            command.BookedByUserId = userId;

            var appointment = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetAppointmentById), new { id = appointment.Id }, appointment);
        }

      
        /// 3. Get Appointment By ID
        /// GET: api/appointments/{id}
   
        [HttpGet("{id}")]
        public async Task<ActionResult<AppointmentDto>> GetAppointmentById(int id)
        {
            try
            {
                var query = new GetAppointmentByIdQuery(id);
                var appointment = await _mediator.Send(query);
                return Ok(appointment);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        /// 4. Update Appointment (NEW!)
        /// PUT: api/appointments/{id}
        [HttpPut("{id}")]
        public async Task<ActionResult<AppointmentDto>> UpdateAppointment(
            int id,
            [FromBody] UpdateAppointmentRequest request)
        {
            try
            {
                var userId = User.FindFirst("sub")?.Value ?? User.FindFirst("id")?.Value;
                if (string.IsNullOrEmpty(userId))
                    return Unauthorized("User not authenticated");

                var command = new UpdateAppointmentCommand
                {
                    AppointmentId = id,
                    PatientName = request.PatientName,
                    PatientPhone = request.PatientPhone,
                    PatientEmail = request.PatientEmail,
                    Notes = request.Notes,
                    UpdatedByUserId = userId
                };

                var appointment = await _mediator.Send(command);
                return Ok(appointment);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Forbid(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

      
        // 5. Get Appointments By Doctor
        // GET: api/appointments/doctor/{doctorId}?date=2026-02-01
        
        [HttpGet("doctor/{doctorId}")]
        [AllowAnonymous]
        public async Task<ActionResult<List<AppointmentDto>>> GetAppointmentsByDoctor(
            int doctorId,
            [FromQuery] DateTime? date = null)
        {
            var query = new GetAppointmentsByDoctorQuery(doctorId, date);
            var appointments = await _mediator.Send(query);
            return Ok(appointments);
        }

    
        /// 6. Get My Appointments
        /// GET: api/appointments/my-appointments
      
        [HttpGet("my-appointments")]
        public async Task<ActionResult<List<AppointmentDto>>> GetMyAppointments()
        {
            var userId = User.FindFirst("sub")?.Value ?? User.FindFirst("id")?.Value;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized("User not authenticated");

            var query = new GetAppointmentsByUserQuery(userId);
            var appointments = await _mediator.Send(query);
            return Ok(appointments);
        }

      
        /// 7. Get Upcoming Appointments (NEW!)
        /// GET: api/appointments/upcoming
        [HttpGet("upcoming")]
        public async Task<ActionResult<List<AppointmentDto>>> GetUpcomingAppointments()
        {
            var userId = User.FindFirst("sub")?.Value ?? User.FindFirst("id")?.Value;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized("User not authenticated");

            var query = new GetUpcomingAppointmentsQuery(userId);
            var appointments = await _mediator.Send(query);
            return Ok(appointments);
        }

       
        /// 8. Reschedule Appointment (NEW!)
        /// POST: api/appointments/{id}/reschedule
       
        [HttpPost("{id}/reschedule")]
        public async Task<ActionResult<AppointmentDto>> RescheduleAppointment(
            int id,
            [FromBody] RescheduleAppointmentRequest request)
        {
            try
            {
                var userId = User.FindFirst("sub")?.Value ?? User.FindFirst("id")?.Value;
                if (string.IsNullOrEmpty(userId))
                    return Unauthorized("User not authenticated");

                var command = new RescheduleAppointmentCommand
                {
                    AppointmentId = id,
                    NewScheduleId = request.NewScheduleId,
                    RescheduledByUserId = userId
                };

                var appointment = await _mediator.Send(command);
                return Ok(appointment);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Forbid(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// 9. Cancel Appointment
        /// POST: api/appointments/{id}/cancel
        [HttpPost("{id}/cancel")]
        public async Task<ActionResult> CancelAppointment(int id, [FromBody] CancelAppointmentRequest request)
        {
            try
            {
                var userId = User.FindFirst("sub")?.Value ?? User.FindFirst("id")?.Value;
                if (string.IsNullOrEmpty(userId))
                    return Unauthorized("User not authenticated");

                var command = new CancelAppointmentCommand
                {
                    AppointmentId = id,
                    Reason = request.Reason,
                    CancelledByUserId = userId
                };

                await _mediator.Send(command);
                return Ok(new { message = "Appointment cancelled successfully" });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Forbid(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// 10. Complete Appointment (NEW!)
        /// POST: api/appointments/{id}/complete
        [HttpPost("{id}/complete")]
        public async Task<ActionResult> CompleteAppointment(int id)
        {
            try
            {
                var userId = User.FindFirst("sub")?.Value ?? User.FindFirst("id")?.Value;
                if (string.IsNullOrEmpty(userId))
                    return Unauthorized("User not authenticated");

                var command = new CompleteAppointmentCommand
                {
                    AppointmentId = id,
                    CompletedByUserId = userId
                };

                await _mediator.Send(command);
                return Ok(new { message = "Appointment completed successfully" });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
