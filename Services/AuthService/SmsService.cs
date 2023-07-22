using ArzonOL.Services.AuthService.Interfaces;
using Microsoft.Extensions.Caching.Memory;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace ArzonOL.Services.AuthService;

public class SmsService : ISmsService
{
    private readonly IMemoryCache _cache;
    private readonly ILogger<SmsService> _logger;
    private readonly IConfiguration _config;
    public SmsService(IMemoryCache cache, ILogger<SmsService> logger, IConfiguration configuration)
    {
        _cache = cache;
        _logger = logger;
        _config = configuration;
    }
    public async Task SaveSmsCodeToCache(string phoneNumber, string code)
    {
      if (string.IsNullOrEmpty(phoneNumber) || string.IsNullOrEmpty(code))
      {
        throw new ArgumentNullException();
      }

        _logger.LogInformation($"Code {code} saving with key {phoneNumber}");

        var cacheEntryOptions = new MemoryCacheEntryOptions()
            .SetSlidingExpiration(TimeSpan.FromMinutes(3));

        if (_cache.TryGetValue(phoneNumber, out string? existingCode))
        {
            _cache.Remove(phoneNumber);
        }

        _cache.Set(phoneNumber, code, cacheEntryOptions);

        await Task.CompletedTask;
    }

    public async Task SendSmsAsync(string phoneNumber)
    {
        _logger.LogInformation($"Sending Sms to {phoneNumber} number");

        var code = GenerateRandomCode();
        await SaveSmsCodeToCache(phoneNumber, code);
        
        // var twilioPhone = _config.GetSection("Twilio")["phoneNumber"];
        // var accountSid = _config.GetSection("Twilio")["accountSid"];
        // var authToken = _config.GetSection("Twilio")["authToken"];

        // TwilioClient.Init(accountSid, authToken);

        // string messageText = $"Skidka Access Code: {code}";

        // var message = MessageResource.Create(
        //     from: new PhoneNumber(twilioPhone),
        //     to: new PhoneNumber(phoneNumber),
        //     body: messageText
        // );
    }

    public bool ValidateSmsCode(string phoneNumber, string code)
    {
        if (string.IsNullOrEmpty(phoneNumber) || string.IsNullOrEmpty(code))
        {
            throw new ArgumentNullException();
        }

        if (_cache.TryGetValue(phoneNumber, out string? existingCode))
        {
            if (existingCode == code)
            {
                _cache.Remove(phoneNumber);
                return true;
            }
        }

        return false;
    }

    private string GenerateRandomCode()
    {
        Random random = new();
        _logger.LogInformation("Random code generating");
        return random.Next(1000, 9999).ToString();
    }
}