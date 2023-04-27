using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace WindowService
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private HttpClient client;

        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
        }

        public override async Task StartAsync(CancellationToken cancellationToken)
        {
            int callTime = Convert.ToInt32(30);
            var executeat2358PM = GetIntervalatParticularTime();

            //every 30 second
            //await Task.Delay(TimeSpan.FromSeconds(callTime), cancellationToken);

            //every day at 23:58 PM
            await Task.Delay(executeat2358PM, cancellationToken);

            try
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    Console.WriteLine("Sechduler 1:- " + DateTime.Now.ToString("hh:mm:ss"));
                    _logger.LogInformation("Scheduler one called at:- " + DateTime.Now.ToString());
                    await Task.Delay(TimeSpan.FromSeconds(callTime), cancellationToken);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Scheduler Error at:- " + DateTime.Now.ToString(), ex);
            }
        }
        public TimeSpan GetIntervalatParticularTime()
        {
            TimeSpan interval;
            DateTime currentDate = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fffffffK"));
            DateTime nextDate = Convert.ToDateTime(DateTime.Now.AddDays(1).ToString("yyyy-MM-ddT23:58:00.fffffffK"));
            
            //it will execute every day exacte 06:00 AM
            //DateTime nextDate = Convert.ToDateTime(DateTime.Now.AddMinutes(1).ToString("yyyy-MM-ddT06:00:00.fffffffK"));

            interval = nextDate - currentDate;
            return interval;
        }
        public override Task StopAsync(CancellationToken cancellationToken)
        {
            client.Dispose();
            _logger.LogInformation("The service has been stopped...");
            return base.StopAsync(cancellationToken);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var result = await client.GetAsync("https://www.google.com");

                if (result.IsSuccessStatusCode)
                {
                    _logger.LogInformation("The website is up. Status code {StatusCode}", result.StatusCode);
                }
                else
                {
                    _logger.LogError("The website is down. Status code {StatusCode}", result.StatusCode);
                }

                await Task.Delay(5000, stoppingToken);
            }

        }
    }
}
