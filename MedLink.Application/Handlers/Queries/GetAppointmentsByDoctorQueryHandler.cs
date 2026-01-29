using AutoMapper;
using MediatR;
using MedLink_Application.Interfaces.Repositories;
using MedLink_Application.Queries;
using MedLink_Application.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedLink_Application.Handlers.Queries
{

    public class GetAppointmentsByDoctorQueryHandler : IRequestHandler<GetAppointmentsByDoctorQuery, List<AppointmentDto>>
    {
        private readonly IAppointmentRepository _repository;
        private readonly IMapper _mapper;

        public GetAppointmentsByDoctorQueryHandler(IAppointmentRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<List<AppointmentDto>> Handle(GetAppointmentsByDoctorQuery request, CancellationToken cancellationToken)
        {
            var appointments = await _repository.GetAppointmentsByDoctorAsync(request.DoctorId, request.Date);
            return _mapper.Map<List<AppointmentDto>>(appointments);
        }
    }
}
