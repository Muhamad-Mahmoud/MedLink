using MedLink_Application.Responses;
using MediatR;


namespace MedLink_Application.Queries
{
    public class GetAppointmentByIdQuery : IRequest<AppointmentDto>
    {
        public int Id { get; set; }

        public GetAppointmentByIdQuery(int id)
        {
            Id = id;
        }

    }
}
