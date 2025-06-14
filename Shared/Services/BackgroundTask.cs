using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Services
{
    public class BackgroundTask
    {
        private Task? _timerTask;
        private readonly PeriodicTimer _timer;
        private readonly CancellationTokenSource _cts = new();
        private readonly OTPGenerator _OTPGenerator;
        private readonly SMSService sMSService;
        private string phoneNumber;
        public BackgroundTask(TimeSpan interval, string phoneNumber, OTPGenerator oTPGenerator, SMSService sMSService)
        {
            _timer = new(interval);
            _OTPGenerator=oTPGenerator;
            this.sMSService=sMSService;
            this.phoneNumber=phoneNumber;
        }

        public void Start()
        {
            _timerTask = RunAsync();
        }

        private async Task RunAsync()
        {
            try
            {
                
                while (await _timer.WaitForNextTickAsync(_cts.Token))
                {
                    Console.WriteLine($"Task worked: {DateTime.Now:O}");
                    var code = _OTPGenerator.GenerateRandomOTP(6);
                    sMSService.SendSMS(PhoneNumber: this.phoneNumber, code, "WEB");
                }
            }
            catch (OperationCanceledException)
            {

            }
        }

        public async Task StopAsync()
        {
            if (_timerTask is null)
            {
                return;
            }

            _cts.Cancel();
            await _timerTask;
            _cts.Dispose();
            Console.WriteLine("Task has just been stopped.");
        }
    }
}
