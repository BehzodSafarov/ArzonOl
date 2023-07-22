namespace ArzonOL.Services.AuthService.Interfaces
{
    public interface ISmsService
    {
        Task SendSmsAsync(string phoneNumber);
        bool ValidateSmsCode(string phoneNumber, string code);
        Task SaveSmsCodeToCache(string phoneNumber, string code);
    }
}