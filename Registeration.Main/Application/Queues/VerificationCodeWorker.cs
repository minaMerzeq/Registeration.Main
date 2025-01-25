using Registeration.Main.Application.Helpers;
using Registeration.Main.Application.Services;
using Registeration.Main.Domain.Models;

namespace Registeration.Main.Application.Queues
{
    public class VerificationCodeWorker(IServiceProvider provider) : BackgroundService
    {
        private readonly IServiceProvider _provider = provider;
        private VerificationCodeQueue? _queue;
        private IVerificationService? _verifyService;
        private IEmailService? _emailService;
        private ISmsService? _smsService;
        private ILogger<VerificationCodeWorker>? _logger;

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            return Task.Run(async () =>
            {
                _queue = _provider.GetRequiredService<VerificationCodeQueue>();
                
                while (!stoppingToken.IsCancellationRequested)
                {
                    _verifyService = _provider.GetRequiredService<IVerificationService>();
                    _logger = _provider.GetRequiredService<ILogger<VerificationCodeWorker>>();

                    var message = await _queue.DequeueAsync();

                    if (message != null)
                    {
                        try
                        {
                            var verification = new VerificationCode
                            {
                                Code = _verifyService.GenerateCode(),
                                UserId = message.UserId,
                                Type = message.Type,
                            };

                            // Save the verification code to the database
                            await _verifyService.AddVerificationAsync(verification);

                            string messageBody = $"Your verification code is {verification.Code}";

                            if (message.Type == VerificationCodeType.Email)
                            {
                                _emailService = _provider.GetRequiredService<IEmailService>();
                                await _emailService.SendEmail(message.Recipient, "Verification Code", messageBody);

                                _logger.LogInformation($"Sending email to {message.Recipient} with code {verification.Code}");
                            }
                            else if (message.Type == VerificationCodeType.Phone)
                            {
                                _smsService = _provider.GetRequiredService<ISmsService>();
                                _smsService.SendSms(message.Recipient, messageBody);

                                _logger.LogInformation($"Sending SMS to {message.Recipient} with code {verification.Code}");
                            }
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError($"Error processing message: {message}\n  Exception message: {ex.Message} {ex.InnerException?.Message}");
                            // Optionally re-enqueue the message for retry
                        }
                    }
                }
            }, stoppingToken);
        }
    }
}
