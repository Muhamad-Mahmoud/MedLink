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

    public class GetAvailableSlotsQueryHandler : IRequestHandler<GetAvailableSlotsQuery, List<DoctorAvailabilityDto>>
    {
        private readonly IDoctorAvailabilityRepository _repository;
        private readonly IMapper _mapper;

        public GetAvailableSlotsQueryHandler(IDoctorAvailabilityRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<List<DoctorAvailabilityDto>> Handle(GetAvailableSlotsQuery request, CancellationToken cancellationToken)
        {
            var availableSlots = await _repository.GetAvailableSlotsByDoctorAndDateAsync(
                request.DoctorId,
                request.Date.Date  // تأكد من أخذ التاريخ فقط
            );

            return _mapper.Map<List<DoctorAvailabilityDto>>(availableSlots);
        }
    }
}
