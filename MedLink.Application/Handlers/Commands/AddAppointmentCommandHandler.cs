using AutoMapper;
using MediatR;
using MedLink_Application.Interfaces.Repositories;
using MedLink_Application.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedLink_Application.Handlers.Commands
{

    public class AddAppointmentCommandHandler : IRequestHandler<AddAppointmentCommand, AppointmentDto>
    {
        private readonly IAppointmentRepository _repository;
        private readonly IMapper _mapper;

        public AddAppointmentCommandHandler(IAppointmentRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<AppointmentDto> Handle(AddAppointmentCommand request, CancellationToken cancellationToken)
        {
            await _repository.AddAsync(request.Appointment);
            return _mapper.Map<AppointmentDto>(request.Appointment);
        }
    }
}
