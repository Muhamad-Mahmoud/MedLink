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

    public class GetPaymentByAppointmentQueryHandler : IRequestHandler<GetPaymentByAppointmentQuery, PaymentDto>
    {
        private readonly IPaymentRepository _repository;
        private readonly IMapper _mapper;

        public GetPaymentByAppointmentQueryHandler(IPaymentRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<PaymentDto> Handle(GetPaymentByAppointmentQuery request, CancellationToken cancellationToken)
        {
            var payment = await _repository.GetByAppointmentIdAsync(request.AppointmentId);

            if (payment == null)
                throw new KeyNotFoundException($"No payment found for appointment {request.AppointmentId}");

            return _mapper.Map<PaymentDto>(payment);
        }
    }
}
