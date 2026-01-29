using MediatR;
using MedLink_Application.Commands;
using MedLink_Application.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedLink_Application.Handlers.Commands
{

    public class CancelAppointmentCommandHandler : IRequestHandler<CancelAppointmentCommand, bool>
    {
        private readonly IAppointmentRepository _repository;

        public CancelAppointmentCommandHandler(IAppointmentRepository repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(CancelAppointmentCommand request, CancellationToken cancellationToken)
        {
            var appointment = await _repository.GetByIdAsync(request.AppointmentId);
            if (appointment == null)
                throw new KeyNotFoundException("Appointment not found");

            await _repository.CancelAsync(appointment, request.Reason);
            return true;
        }
    }
}
