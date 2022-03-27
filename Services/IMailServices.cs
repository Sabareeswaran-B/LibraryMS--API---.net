using LibraryMS.Model;

namespace LibraryMS.Services
{
  public interface IMailService
  {
    public Task SendEmailAsync(MailRequest mailrequest);
  }
}