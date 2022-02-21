using MarketIntelligency.Connector;
using MarketIntelligency.Core.Interfaces.ExchangeAggregate;
using MarketIntelligency.Core.Models.EnumerationAggregate;
using MarketIntelligency.Core.Models.OrderBookAggregate;
using MarketIntelligency.EventManager;
using MarketIntelligency.EventManager.Models;
using MarketIntelligency.Exchange.Binance;
using MarketIntelligency.Web.Grpc;
using MarketIntelligency.Web.Grpc.Protos;
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
    options.DataIn = BinanceExchange.Information.Markets;
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

builder.Services.AddHostedService<CommunicationHandler>();
builder.Services.AddGrpcClient<StreamEventGrpc.StreamEventGrpcClient>(opt => opt.Address = new Uri(builder.Configuration["DataEventManagerService"]));
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
