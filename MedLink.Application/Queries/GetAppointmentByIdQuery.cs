using MedLink_Application.Responses;
using MediatR;


namespace MedLink_Application.Queries
{
    public class GetAppointmentByIdQuery : IRequest<AppointmentDto>
    {
        public int AppointmentId { get; set; }

        public GetAppointmentByIdQuery(int appointmentId)
        {
            AppointmentId = appointmentId;
        }

    }
}
