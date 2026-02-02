using AutoMapper;
using MediatR;
using MedLink_Application.Interfaces.Repositories;
using MedLink_Application.Queries;
using MedLink_Application.Responses;
namespace MedLink_Application.Handlers.Queries
{
    public class GetAppointmentByIdQueryHandler : IRequestHandler<GetAppointmentByIdQuery, AppointmentDto>
    {
        private readonly IAppointmentRepository _repository;
        private readonly IMapper _mapper;

        public GetAppointmentByIdQueryHandler(IAppointmentRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<AppointmentDto> Handle(GetAppointmentByIdQuery request, CancellationToken cancellationToken)
        {
            var appointment = await _repository.GetByIdWithDetailsAsync(request.AppointmentId);

            if (appointment == null)
                throw new KeyNotFoundException($"Appointment with ID {request.AppointmentId} not found");

            return _mapper.Map<AppointmentDto>(appointment);
        }
    }
}
