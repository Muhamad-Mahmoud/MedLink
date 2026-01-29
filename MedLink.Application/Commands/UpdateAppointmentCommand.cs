using MediatR;
using MedLink.Domain.Entities.Appointments;
using MedLink_Application.Responses;
namespace MedLink_Application.Commands
{


    public class UpdateAppointmentCommand : IRequest<AppointmentDto>
    {
        public Appointment Appointment { get; set; }

        public UpdateAppointmentCommand(Appointment appointment)
        {
            Appointment = appointment;
        }
    }

}
