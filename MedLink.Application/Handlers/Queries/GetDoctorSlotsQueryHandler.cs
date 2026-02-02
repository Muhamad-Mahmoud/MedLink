using AutoMapper;
using MediatR;
using MedLink.Application.Queries;
using MedLink_Application.Interfaces.Repositories;
using MedLink_Application.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedLink.Application.Handlers.Queries
{

    public class GetDoctorSlotsQueryHandler : IRequestHandler<GetDoctorSlotsQuery, List<DoctorAvailabilityDto>>
    {
        private readonly IDoctorAvailabilityRepository _repository;
        private readonly IMapper _mapper;

        public GetDoctorSlotsQueryHandler(
            IDoctorAvailabilityRepository repository,
            IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<List<DoctorAvailabilityDto>> Handle(GetDoctorSlotsQuery request, CancellationToken cancellationToken)
        {
            var slots = await _repository.GetSlotsByDoctorAsync(
                request.DoctorId,
                request.FromDate,
                request.ToDate);

            return _mapper.Map<List<DoctorAvailabilityDto>>(slots);
        }
    }
}
