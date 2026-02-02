using MediatR;


namespace MedLink_Application.Commands
{
    public class CancelAppointmentCommand : IRequest<Unit>
    {
        public int AppointmentId { get; set; }
        public string Reason { get; set; } = string.Empty;
        public string CancelledByUserId { get; set; } = string.Empty;
    }
}
