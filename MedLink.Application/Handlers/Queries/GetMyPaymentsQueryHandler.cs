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

    public class GetMyPaymentsQueryHandler : IRequestHandler<GetMyPaymentsQuery, List<PaymentDto>>
    {
        private readonly IPaymentRepository _repository;
        private readonly IMapper _mapper;

        public GetMyPaymentsQueryHandler(IPaymentRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<List<PaymentDto>> Handle(GetMyPaymentsQuery request, CancellationToken cancellationToken)
        {
            var payments = await _repository.GetPaymentsByUserAsync(request.UserId);
            return _mapper.Map<List<PaymentDto>>(payments);
        }
    }
}
