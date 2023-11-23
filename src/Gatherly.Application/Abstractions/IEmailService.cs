using Gatherly.Domain.Entities;

namespace Gatherly.Application.Abstractions
{
  internal interface IEmailService
  {
    Task SendInvitationSendEmailAsync(Member member, Gathering gathering);
  }
}
