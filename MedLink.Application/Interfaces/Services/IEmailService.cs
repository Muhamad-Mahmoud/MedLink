using MedLink_Application.DTOs.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace MedLink_Application.Interfaces.Services
{
    public interface IEmailService
    {
        Task SendEmailAsync(EmailMessage message);

    }
}
