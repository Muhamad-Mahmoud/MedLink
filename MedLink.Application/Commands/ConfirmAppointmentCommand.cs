using MediatR;


namespace MedLink_Application.Commands
{
    public class ConfirmAppointmentCommand : IRequest<bool>
    {
        public int AppointmentId { get; set; }

        public ConfirmAppointmentCommand(int appointmentId)
        {
            AppointmentId = appointmentId;
        }
    }
}
