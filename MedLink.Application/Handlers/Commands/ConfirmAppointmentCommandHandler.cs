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

    public class ConfirmAppointmentCommandHandler : IRequestHandler<ConfirmAppointmentCommand, bool>
    {
        private readonly IAppointmentRepository _repository;

        public ConfirmAppointmentCommandHandler(IAppointmentRepository repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(ConfirmAppointmentCommand request, CancellationToken cancellationToken)
        {
            var appointment = await _repository.GetByIdAsync(request.AppointmentId);
            if (appointment == null)
                throw new KeyNotFoundException("Appointment not found");

            await _repository.ConfirmAsync(appointment);
            return true;
        }
    }
}
