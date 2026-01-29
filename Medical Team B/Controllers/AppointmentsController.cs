using MediatR;
using MedLink_Application.Commands;
using MedLink_Application.Queries;
using MedLink_Application.Responses;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace Medical_Team_B.Controllers
{

    public class AppointmentsController : BaseApiController
    {
        private readonly IMediator _mediator;

        public AppointmentsController(IMediator mediator)
        {
            _mediator = mediator;
        }
        // GET: api/appointments/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<AppointmentDto>> GetById(int id)
        {
            try
            {
                var appointment = await _mediator.Send(new GetAppointmentByIdQuery(id));
                return Ok(appointment);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
        // GET: api/appointments/doctor/{doctorId}?date=2026-01-29
        [HttpGet("doctor/{doctorId}")]
        public async Task<ActionResult<List<AppointmentDto>>> GetByDoctor(int doctorId, [FromQuery] DateTime? date = null)
        {
            var appointments = await _mediator.Send(new GetAppointmentsByDoctorQuery(doctorId, date));
            return Ok(appointments);
        }

        // GET: api/appointments/user/{userId}
        [HttpGet("user/{userId}")]
        public async Task<ActionResult<List<AppointmentDto>>> GetByUser(string userId)
        {
            var appointments = await _mediator.Send(new GetAppointmentsByUserQuery(userId));
            return Ok(appointments);
        }

        // POST: api/appointments
        [HttpPost]
        public async Task<ActionResult<AppointmentDto>> Create([FromBody] AddAppointmentCommand command)
        {
            var appointmentDto = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetById), new { id = appointmentDto.Id }, appointmentDto);
        }

        // PUT: api/appointments
        [HttpPut]
        public async Task<ActionResult<AppointmentDto>> Update([FromBody] UpdateAppointmentCommand command)
        {
            var appointmentDto = await _mediator.Send(command);
            return Ok(appointmentDto);
        }

        // POST: api/appointments/cancel
        [HttpPost("cancel")]
        public async Task<ActionResult> Cancel([FromBody] CancelAppointmentCommand command)
        {
            try
            {
                await _mediator.Send(command);
                return Ok("Appointment cancelled successfully");
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        // POST: api/appointments/confirm
        [HttpPost("confirm")]
        public async Task<ActionResult> Confirm([FromBody] ConfirmAppointmentCommand command)
        {
            try
            {
                await _mediator.Send(command);
                return Ok("Appointment confirmed successfully");
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

    }
}
