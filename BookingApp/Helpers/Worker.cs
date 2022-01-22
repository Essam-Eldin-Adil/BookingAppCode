using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Data.Models.Chalets;
using Domain;
using Microsoft.EntityFrameworkCore;

namespace BookingApp
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly DataContext _context;
        // private readonly EProcurementSolutionContext _context;
        public Worker(ILogger<Worker> logger, DataContext context)
        {
            _logger = logger;
            _context = context;
            //_context = context;
        }
        public override async Task StartAsync(CancellationToken cancellationToken)
        {
            // DO YOUR STUFF HERE
            await base.StartAsync(cancellationToken);
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            // DO YOUR STUFF HERE
            await base.StopAsync(cancellationToken);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await CheckReservation();
                }
                catch(Exception ex)
                {
                    await Task.Delay(10000, stoppingToken);
                }
                await Task.Delay(10000, stoppingToken);
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
            }
            

        }

        private async Task<string> CheckReservation()
        {
            try
            {
                var reservations = await _context.Reservations.Where(c => c.CreatedDate.AddDays(1) >= DateTime.Now && c.Invoices.Sum(m => m.Amount) == 0&&c.Status==(int)Enums.Status.New).ToListAsync();
                foreach (var reservation in reservations)
                {
                    reservation.Status = (int)Enums.Status.Cancled;
                    reservation.CancelResones = "لم يتم دفع العربون في الوقت المحدد";
                    _context.Reservations.Update(reservation);
                }
            }
            catch (Exception ex)
            {

            }
            return "";
        }
    }
}
