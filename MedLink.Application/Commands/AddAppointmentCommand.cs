using MediatR;
using MedLink.Domain.Entities.Appointments;
using MedLink_Application.Responses;

public class AddAppointmentCommand : IRequest<AppointmentDto>
{
    public Appointment Appointment { get; set; }

    public AddAppointmentCommand(Appointment appointment)
    {
        Appointment = appointment;
    }
}
