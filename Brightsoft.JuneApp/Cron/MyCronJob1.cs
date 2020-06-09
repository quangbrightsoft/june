using System;
using System.Threading;
using System.Threading.Tasks;
using Brightsoft.CronJob;
using Microsoft.Extensions.Logging;
using RestSharp;
using SendGrid;

namespace Brightsoft.JuneApp.Cron
{
    public class MyCronJob1 : CronJobService
    {
        private readonly ILogger<MyCronJob1> _logger;

        public MyCronJob1(IScheduleConfig<MyCronJob1> config, ILogger<MyCronJob1> logger)
            : base(config.CronExpression, config.TimeZoneInfo)
        {
            _logger = logger;
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("CronJob 1 starts.");
            return base.StartAsync(cancellationToken);
        }

        public override Task DoWork(CancellationToken cancellationToken)
        {
            _logger.LogInformation($"{DateTime.Now:hh:mm:ss} calling API.");
            var client = new RestClient("https://reqres.in/api/users/2");
            var request = new RestRequest();
            request.AddHeader("accept", "application/json; charset=utf-8");
            var response = client.Execute(request);
            var responseContent = response.Content;
            Console.Write(responseContent);
            return Task.CompletedTask;
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("CronJob 1 is stopping.");
            return base.StopAsync(cancellationToken);
        }
    }
}
