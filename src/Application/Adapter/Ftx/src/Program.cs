using Zzaia.Finance.Connector;
using Zzaia.Finance.Core.Interfaces.ExchangeAggregate;
using Zzaia.Finance.Core.Models.EnumerationAggregate;
using Zzaia.Finance.Core.Models.OrderBookAggregate;
using Zzaia.Finance.EventManager;
using Zzaia.Finance.EventManager.Models;
using Zzaia.Finance.Exchange.Ftx;
using Zzaia.Finance.Web.Grpc;
using Microsoft.Extensions.Logging.Console;

var builder = WebApplication.CreateBuilder(args);


//----------- Exchange API Clients -------------------
builder.Services.AddHttpClient();
builder.Services.AddExchange(ExchangeName.Ftx,
        privateCredential => builder.Configuration.Bind("Exchange:Ftx:Private", privateCredential),
        tradeCredential => builder.Configuration.Bind("Exchange:Ftx:Trade", tradeCredential));
builder.Services.AddSingleton<IExchangeSelector, ExchangeSelector>();

//----------- Data Event Connectors -------------------

builder.Services.AddWebSocketConnector(options =>
{
    options.ExchangeName = ExchangeName.Ftx;
    options.DataIn = FtxExchange.Information.Markets;
    options.DataOut = new List<Type> { typeof(OrderBook) };
});

builder.Services.AddApplicationInsightsTelemetry(options =>
{
    options.EnableAdaptiveSampling = false;
});

builder.Services.AddEventManager(options =>
{
    options.PublishStrategy = PublishStrategy.ParallelNoWait;
}, typeof(Program).Assembly, typeof(EventManagerExtension).Assembly);

builder.Services.AddGrpcEventCommunication(builder.Configuration["DataEventManagerService"]);

builder.Host.ConfigureLogging(
    logging =>
    {
        logging.ClearProviders();
        logging.AddSimpleConsole(options =>
        {
            options.ColorBehavior = LoggerColorBehavior.Enabled;
            options.SingleLine = true;
            options.TimestampFormat = " hh:mm:ss.ffffff | ";
        });
        logging.AddFilter("System.Net.Http.HttpClient.Default.LogicalHandler", LogLevel.None);
        logging.AddFilter("System.Net.Http.HttpClient.Default.ClientHandler", LogLevel.None);
    });
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.Run();
