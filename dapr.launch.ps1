# Adapters applications:
dapr run --app-id "binance-adapter" --app-port "5001" --dapr-grpc-port "50010" --dapr-http-port "5010" -- dotnet run --project .src/Application/Adapter/Binance/src/MarketIntelligency.Application.Adapter.Binance.csproj --urls="http://+:5001"

# Event Managers:
dapr run --app-id "data-event-manager" --app-port "5002" --dapr-grpc-port "50011" --dapr-http-port "5011" -- dotnet run --project .src/Application/DataEventManager/src/MarketIntelligency.Application.DataEventManager.csproj --urls="http://+:5002"

# Strategies:
dapr run --app-id "strategies-services" --app-port "5003" --dapr-grpc-port "50012" --dapr-http-port "5012" -- dotnet run --project .src/Application/Strategies/src/MarketIntelligency.Application.Strategies.csproj --urls="http://+:5003"