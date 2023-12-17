using Gatherly.Domain.Entities;

namespace Gatherly.Application.Abstractions.Email
{
    internal interface IEmailService
    {
        Task SendInvitationSendEmailAsync(Member member, Gathering gathering);
    }
}
