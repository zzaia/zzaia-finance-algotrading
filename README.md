# Description

This codebase was made in my free time as a side project in the field of algorithmic trading, 
the idea behind this was to try to develop a functional system able to have one or more strategies running in crypto markets, 
and stress out some ideas of an ideal architectural system for that. As the project showed more complex and work-intensive I started 
using it to develop some technics that were not developed in my regular job like Rest API polling and WebSocket connection with crypto exchanges and 
GRPC communication between internal backend applications using the reactive pattern. 

To have this solution running, the following applications can be executed:
- Zzaia.Finance.Applications.Adapter.Ftx (Not working anymore because of the FTX API being down)
- Zzaia.Finance.Applications.Adapter.Binance 
- Zzaia.Finance.Applications.Adapter.MercadoBitcoin 

Those three are responsible to connect to the following exchanges to get the order book or any other information by polling or WebSocket and publishing to a
Event manager application that centralizes the data flow to the rest of the system.

- Zzaia.Finance.Applications.EventManager

The Event Manager than would be responsible to order the data by domain type, persist and publish to the application containing one or more strategies 

- Zzaia.Finance.Application.Strategies

To use a specific logic or machine learning algorithm the strategy could use applications dedicated to that from exposed APIs.

- Zzaia.Finance.Application.MachineLearning

And finally, the strategy would publish the output of the operation resulting from the logic to an order event manager application that would order the operation data, 
persist and publish to the respective exchange adapter to be executed.

- Zzaia.Finance.Application.OrderEventManager

All remaining projects are libraries made available to all applications.

- Zzaia.Finance.Core
- Zzaia.Finance.Connector
- Zzaia.Finance.EventManager
- Zzaia.Finance.Web.Grpc
- Zzaia.Finance.WebSocket
- Zzaia.Finance.Exchange.Binance
- Zzaia.Finance.Exchange.Ftx
- Zzaia.Finance.Exchange.MercadoBitcoin

## Final Considerations

This project was the first attempt to develop a distributed system following the experience I had in the time from my regular job, 
but realized that some bad choices were made: not using a publish/subscribe brokerage system like Kafka for the internal reactive communications;
centralizing the data flow in one application that could be a possible bottleneck; not separating the components by domain models, having all types of domains data 
flowing in the same applications, making it impossible to scale granularly on demand; by sharing the same domain models to all components like Utils models, breaking 
the segregation pattern of microservices.
