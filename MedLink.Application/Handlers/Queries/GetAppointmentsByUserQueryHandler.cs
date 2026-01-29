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

    public class GetAppointmentsByUserQueryHandler : IRequestHandler<GetAppointmentsByUserQuery, List<AppointmentDto>>
    {
        private readonly IAppointmentRepository _repository;
        private readonly IMapper _mapper;

        public GetAppointmentsByUserQueryHandler(IAppointmentRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<List<AppointmentDto>> Handle(GetAppointmentsByUserQuery request, CancellationToken cancellationToken)
        {
            var appointments = await _repository.GetAppointmentsByUserAsync(request.UserId);
            return _mapper.Map<List<AppointmentDto>>(appointments);
        }
    }
}
