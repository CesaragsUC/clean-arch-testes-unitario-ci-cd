using CleanArch.Application.Dtos;

namespace CleanArch.Application.Abstractions
{
    public interface IEmailService
    {
        Task SendAsync(EmailRequestDto request);
    }
}
