using MediatR;
using MedLink_Application.Responses;


namespace MedLink_Application.Queries
{

    public class GetMyPaymentsQuery : IRequest<List<PaymentDto>>
    {
        public string UserId { get; set; }

        public GetMyPaymentsQuery(string userId)
        {
            UserId = userId;
        }
    }

}
