#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["MarketIntelligency.Application.SA0001/MarketIntelligency.Application.SA0001.csproj", "MarketIntelligency.Application.SA0001/"]
COPY ["MarketIntelligency.WebGrpc/MarketIntelligency.WebGrpc.csproj", "MarketIntelligency.WebGrpc/"]
COPY ["MarketIntelligency.Connector/MarketIntelligency.Connector.csproj", "MarketIntelligency.Connector/"]
COPY ["MarketIntelligency.EventManager/MarketIntelligency.EventManager.csproj", "MarketIntelligency.EventManager/"]
COPY ["MarketIntelligency.Core/MarketIntelligency.Core.csproj", "MarketIntelligency.Core/"]
COPY ["MarketIntelligency.WebSocket/MarketIntelligency.WebSocket.csproj", "MarketIntelligency.WebSocket/"]
COPY ["MarketIntelligency.Exchange.MercadoBitcoin/MarketIntelligency.Exchange.MercadoBitcoin.csproj", "MarketIntelligency.Exchange.MercadoBitcoin/"]
COPY ["MarketIntelligency.Exchange.Binance/MarketIntelligency.Exchange.Binance.csproj", "MarketIntelligency.Exchange.Binance/"]
RUN dotnet restore "MarketIntelligency.Application.SA0001/MarketIntelligency.Application.SA0001.csproj"
COPY . .
WORKDIR "/src/MarketIntelligency.Application.SA0001"
RUN dotnet build "MarketIntelligency.Application.SA0001.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MarketIntelligency.Application.SA0001.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MarketIntelligency.Application.SA0001.dll"]