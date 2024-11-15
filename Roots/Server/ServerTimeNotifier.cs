//using EDC.Server.Controllers;
//using EDC.Server.Data;
//using Microsoft.AspNetCore.Components;
//using Microsoft.AspNetCore.SignalR;
//using System.Text.Json;
//using EDC.Client.Model;

//namespace EDC.Server;

//public class ServerTimeNotifier : BackgroundService
//{
//    [Inject]
//    protected HttpClient HttpClient { get; set; }
//    private static readonly TimeSpan Period = TimeSpan.FromSeconds(30);
//    private readonly ILogger<ServerTimeNotifier> _logger;
//    private readonly IHubContext<NotificationsHub,INotificationClient> _context;
//    public List<NotificationModel> NotificationList { get; set; } = new List<NotificationModel>();

//    public ServerTimeNotifier(ILogger<ServerTimeNotifier> logger, IHubContext<NotificationsHub, INotificationClient> context)
//    {
//        _logger = logger;
//        _context = context;
//    }
//    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
//    {
//        using var timer = new PeriodicTimer(Period);
//        while(!stoppingToken.IsCancellationRequested && await timer.WaitForNextTickAsync(stoppingToken))
//        {
//            var dateTime = DateTime.Now;
//            _logger.LogInformation("Executing {Service} {Time}", nameof(ServerTimeNotifier), dateTime);
//        }
//    }
//}