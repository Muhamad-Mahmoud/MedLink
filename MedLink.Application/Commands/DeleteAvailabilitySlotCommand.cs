using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedLink.Application.Commands
{
    public class DeleteAvailabilitySlotCommand : IRequest<Unit>
    {
        public int SlotId { get; set; }
        public string DeletedByUserId { get; set; } = string.Empty;
    }

}
