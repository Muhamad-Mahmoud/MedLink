using MediatR;
using MedLink.Application.Commands;
using MedLink.Application.Queries;
using MedLink.Application.Responses;
using MedLink_Application.Queries;
using MedLink_Application.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Medical_Team_B.Controllers
{
    [Authorize(Roles = "Admin")]
    public class DoctorAvailabilityController : BaseApiController
    {
        private readonly IMediator _mediator;

        public DoctorAvailabilityController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// 1. Get Available Slots (Public)
        /// GET: api/doctoravailability/available?doctorId=1&date=2026-02-01
        /// </summary>
        [HttpGet("available")]
        [AllowAnonymous]
        public async Task<ActionResult<List<DoctorAvailabilityDto>>> GetAvailableSlots(
            [FromQuery] int doctorId,
            [FromQuery] DateTime date)
        {
            var query = new GetAvailableSlotsQuery(doctorId, date);
            var slots = await _mediator.Send(query);
            return Ok(slots);
        }

        /// <summary>
        /// 2. Add Single Slot (Admin)
        /// POST: api/doctoravailability/slot
        /// Body: { "doctorId": 1, "date": "2026-02-01", "startTime": "09:00", "endTime": "09:30" }
        /// </summary>
        [HttpPost("slot")]
    //  [AllowAnonymous] 
        public async Task<ActionResult<DoctorAvailabilityDto>> AddSlot([FromBody] AddSlotRequest request)
        {
            try
            {
                var userId = User.FindFirst("sub")?.Value ?? User.FindFirst("id")?.Value ?? "admin";

                var command = new AddAvailabilitySlotCommand
                {
                    DoctorId = request.DoctorId,
                    Date = request.Date,
                    StartTime = TimeSpan.Parse(request.StartTime),
                    EndTime = TimeSpan.Parse(request.EndTime),
                    CreatedByUserId = userId
                };

                var slot = await _mediator.Send(command);
                return CreatedAtAction(nameof(GetAvailableSlots),
                    new { doctorId = slot.DoctorId, date = slot.Date },
                    slot);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// 3. Add Multiple Slots (Admin - Bulk)
        /// POST: api/doctoravailability/slots/bulk
        /// Body:  AddMultipleSlotsRequest
        /// </summary>
        [HttpPost("slots/bulk")]
      //  [AllowAnonymous] 
        public async Task<ActionResult<BulkSlotsResponse>> AddMultipleSlots([FromBody] AddMultipleSlotsRequest request)
        {
            try
            {
                var userId = User.FindFirst("sub")?.Value ?? User.FindFirst("id")?.Value ?? "admin";

                var command = new AddMultipleSlotsCommand
                {
                    DoctorId = request.DoctorId,
                    StartDate = request.StartDate,
                    EndDate = request.EndDate,
                    WorkingDays = request.WorkingDays,
                    TimeSlots = request.TimeSlots.Select(ts => new TimeSlot
                    {
                        StartTime = ts.StartTime,
                        EndTime = ts.EndTime
                    }).ToList(),
                    CreatedByUserId = userId
                };

                var slots = await _mediator.Send(command);

                return Ok(new BulkSlotsResponse
                {
                    Success = true,
                    TotalSlotsCreated = slots.Count,
                    Message = $"Successfully created {slots.Count} availability slots",
                    Slots = slots
                });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// 4. Add Week Schedule (سهل - لإضافة أسبوع كامل)
        /// POST: api/doctoravailability/week
        /// Body: { "doctorId": 1, "startDate": "2026-02-01" }
        /// </summary>
        [HttpPost("week")]
     //   [AllowAnonymous]
        public async Task<ActionResult<BulkSlotsResponse>> AddWeekSchedule([FromBody] AddWeekScheduleRequest request)
        {
            try
            {
                var userId = User.FindFirst("sub")?.Value ?? User.FindFirst("id")?.Value ?? "admin";

                // Default: أيام العمل (الأحد - الخميس) - بدون الجمعة
                var workingDays = new List<DayOfWeek>
            {
                DayOfWeek.Saturday,
                DayOfWeek.Sunday,
                DayOfWeek.Monday,
                DayOfWeek.Tuesday,
                DayOfWeek.Wednesday,
                DayOfWeek.Thursday
            };

                // Default: مواعيد الصباح والمساء
                var timeSlots = new List<TimeSlot>
            {
                // Morning slots (9:00 - 12:00)
                new TimeSlot { StartTime = TimeSpan.Parse("09:00"), EndTime = TimeSpan.Parse("09:30") },
                new TimeSlot { StartTime = TimeSpan.Parse("09:30"), EndTime = TimeSpan.Parse("10:00") },
                new TimeSlot { StartTime = TimeSpan.Parse("10:00"), EndTime = TimeSpan.Parse("10:30") },
                new TimeSlot { StartTime = TimeSpan.Parse("10:30"), EndTime = TimeSpan.Parse("11:00") },
                new TimeSlot { StartTime = TimeSpan.Parse("11:00"), EndTime = TimeSpan.Parse("11:30") },
                new TimeSlot { StartTime = TimeSpan.Parse("11:30"), EndTime = TimeSpan.Parse("12:00") },
                
                // Evening slots (5:00 PM - 8:00 PM)
                new TimeSlot { StartTime = TimeSpan.Parse("17:00"), EndTime = TimeSpan.Parse("17:30") },
                new TimeSlot { StartTime = TimeSpan.Parse("17:30"), EndTime = TimeSpan.Parse("18:00") },
                new TimeSlot { StartTime = TimeSpan.Parse("18:00"), EndTime = TimeSpan.Parse("18:30") },
                new TimeSlot { StartTime = TimeSpan.Parse("18:30"), EndTime = TimeSpan.Parse("19:00") },
                new TimeSlot { StartTime = TimeSpan.Parse("19:00"), EndTime = TimeSpan.Parse("19:30") },
                new TimeSlot { StartTime = TimeSpan.Parse("19:30"), EndTime = TimeSpan.Parse("20:00") }
            };

                var command = new AddMultipleSlotsCommand
                {
                    DoctorId = request.DoctorId,
                    StartDate = request.StartDate,
                    EndDate = request.StartDate.AddDays(6), // أسبوع كامل
                    WorkingDays = workingDays,
                    TimeSlots = timeSlots,
                    CreatedByUserId = userId
                };

                var slots = await _mediator.Send(command);

                return Ok(new BulkSlotsResponse
                {
                    Success = true,
                    TotalSlotsCreated = slots.Count,
                    Message = $"Successfully created {slots.Count} slots for week starting {request.StartDate:yyyy-MM-dd}",
                    Slots = slots
                });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// 5. Add Day Schedule (سهل - يوم واحد بفترة زمنية)
        /// POST: api/doctoravailability/day
        /// Body: { "doctorId": 1, "date": "2026-02-02", "startTime": "10:00", "endTime": "17:00", "slotDurationMinutes": 15 }
        /// </summary>
        [HttpPost("day")]
       // [AllowAnonymous]
        public async Task<ActionResult<DayScheduleResponse>> AddDaySchedule([FromBody] AddDayScheduleRequest request)
        {
            try
            {
                var userId = User.FindFirst("sub")?.Value ?? User.FindFirst("id")?.Value ?? "admin";

                var command = new AddDayScheduleCommand
                {
                    DoctorId = request.DoctorId,
                    Date = request.Date,
                    StartTime = request.StartTime,
                    EndTime = request.EndTime,
                    SlotDurationMinutes = request.SlotDurationMinutes ?? 15, // Default: 15 min
                    CreatedByUserId = userId
                };

                var result = await _mediator.Send(command);
                return Ok(result);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (FormatException ex)
            {
                return BadRequest(new { message = "Invalid time format. Use HH:mm format (e.g., '10:00')" });
            }
        }


        /// /// add custom schedule with specified working days and time slots
        /// POST: api/doctoravailability/custom

        [HttpPost("custom")]
       // [AllowAnonymous]
        public async Task<ActionResult<BulkSlotsResponse>> AddCustomSchedule([FromBody] AddCustomScheduleRequest request)
        {
            try
            {
                var userId = User.FindFirst("sub")?.Value ?? User.FindFirst("id")?.Value ?? "admin";

                var command = new AddMultipleSlotsCommand
                {
                    DoctorId = request.DoctorId,
                    StartDate = request.StartDate,
                    EndDate = request.EndDate,
                    WorkingDays = request.WorkingDays ?? GetDefaultWorkingDays(),
                    TimeSlots = (request.TimeSlots?.Any() ?? false)
                        ? request.TimeSlots.Select(ts => new TimeSlot
                        {
                            StartTime = ts.StartTime,
                            EndTime = ts.EndTime
                        }).ToList()
                        : GetDefaultTimeSlots(),
                    CreatedByUserId = userId
                };

                var slots = await _mediator.Send(command);

                return Ok(new BulkSlotsResponse
                {
                    Success = true,
                    TotalSlotsCreated = slots.Count,
                    Message = $"Successfully created {slots.Count} custom slots",
                    Slots = slots
                });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// 6. Delete Slot (Admin)
        /// DELETE: api/doctoravailability/slot/{id}
     
        [HttpDelete("slot/{id}")]
     //   [AllowAnonymous]
        public async Task<ActionResult> DeleteSlot(int id)
        {
            try
            {
                var userId = User.FindFirst("sub")?.Value ?? User.FindFirst("id")?.Value ?? "admin";

                var command = new DeleteAvailabilitySlotCommand
                {
                    SlotId = id,
                    DeletedByUserId = userId
                };

                await _mediator.Send(command);
                return Ok(new { message = "Slot deleted successfully" });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

     
        /// 7. Get All Slots for Doctor (Admin)
        /// GET: api/doctoravailability/doctor/{doctorId}/all

        [HttpGet("doctor/{doctorId}/all")]
       // [AllowAnonymous]
        public async Task<ActionResult<List<DoctorAvailabilityDto>>> GetAllDoctorSlots(
            int doctorId,
            [FromQuery] DateTime? fromDate = null,
            [FromQuery] DateTime? toDate = null)
        {
            var query = new GetDoctorSlotsQuery(doctorId, fromDate, toDate);
            var slots = await _mediator.Send(query);
            return Ok(slots);
        }

        // Helper methods
        private List<DayOfWeek> GetDefaultWorkingDays()
        {
            return new List<DayOfWeek>
        {
            DayOfWeek.Saturday,
            DayOfWeek.Sunday,
            DayOfWeek.Monday,
            DayOfWeek.Tuesday,
            DayOfWeek.Wednesday,
            DayOfWeek.Thursday
        };
        }

        private List<TimeSlot> GetDefaultTimeSlots()
        {
            return new List<TimeSlot>
        {
            new TimeSlot { StartTime = TimeSpan.Parse("09:00"), EndTime = TimeSpan.Parse("09:30") },
            new TimeSlot { StartTime = TimeSpan.Parse("09:30"), EndTime = TimeSpan.Parse("10:00") },
            new TimeSlot { StartTime = TimeSpan.Parse("10:00"), EndTime = TimeSpan.Parse("10:30") },
            new TimeSlot { StartTime = TimeSpan.Parse("10:30"), EndTime = TimeSpan.Parse("11:00") },
            new TimeSlot { StartTime = TimeSpan.Parse("11:00"), EndTime = TimeSpan.Parse("11:30") },
            new TimeSlot { StartTime = TimeSpan.Parse("11:30"), EndTime = TimeSpan.Parse("12:00") },
            new TimeSlot { StartTime = TimeSpan.Parse("17:00"), EndTime = TimeSpan.Parse("17:30") },
            new TimeSlot { StartTime = TimeSpan.Parse("17:30"), EndTime = TimeSpan.Parse("18:00") },
            new TimeSlot { StartTime = TimeSpan.Parse("18:00"), EndTime = TimeSpan.Parse("18:30") },
            new TimeSlot { StartTime = TimeSpan.Parse("18:30"), EndTime = TimeSpan.Parse("19:00") },
            new TimeSlot { StartTime = TimeSpan.Parse("19:00"), EndTime = TimeSpan.Parse("19:30") },
            new TimeSlot { StartTime = TimeSpan.Parse("19:30"), EndTime = TimeSpan.Parse("20:00") }
        };
        }
    }

}

