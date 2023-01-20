# Description

I have made this codebase in my free time as a side project in algorithmic trading. 
The idea behind this was to develop a functional system to have one or more strategies
running in crypto markets and stress out some concepts of an ideal architectural system. 
As the project showed more complex and labor-intensive, I started using it to practice 
some exciting techniques like Rest API polling and WebSocket connection with crypto exchanges. 
Also, GRPC communication between internal backend applications using the reactive pattern.

For the system to function, run one of these three applications:

- Zzaia.Finance.Applications.Adapter.Ftx (Not working anymore because of the FTX API being down)
- Zzaia.Finance.Applications.Adapter.Binance 
- Zzaia.Finance.Applications.Adapter.MercadoBitcoin 

Those three are responsible for connecting to the following exchanges to get the order book
or any other information by polling or WebSocket and publishing to an Event Manager application 
that centralizes the data flow to the rest of the system.

- Zzaia.Finance.Applications.EventManager

The Event Manager then would be responsible for grouping the data by domain type persist and 
publishing to the application containing one or more strategies.

- Zzaia.Finance.Application.Strategies

The strategy could use applications dedicated to specific logic and machine learning algorithm 
exposed from APIs.

- Zzaia.Finance.Application.MachineLearning

And finally, the strategy would publish the order output from the logic to an order event manager 
application that would group the operation data, persist, and post to the respective exchange adapter to execute.

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

This project was the first attempt to develop a distributed system following the experience 
I had in the time of my regular job. However, I realized that I had made some bad choices: 
not using a publish/subscribe brokerage system like Kafka for the internal reactive communications, 
centralizing the data flow in one application that could be a possible bottleneck, not separating 
the components by domain models, having all types of domains data flowing in the same applications, 
making it impossible to scale granularly on demand, sharing the same domain models to all components like Utils models, 
breaking the segregation of concern pattern of microservices.
