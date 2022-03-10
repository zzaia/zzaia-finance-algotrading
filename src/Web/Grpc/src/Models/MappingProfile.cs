using AutoMapper;
using Google.Protobuf.WellKnownTypes;
using MarketIntelligency.Core.Models;
using MarketIntelligency.Core.Models.EnumerationAggregate;
using MarketIntelligency.Core.Models.MarketAgregate;
using MarketIntelligency.Core.Models.OrderBookAggregate;
using MarketIntelligency.Web.Grpc.Protos;

namespace MarketIntelligency.Web.Grpc.Models
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<OrderBook, OrderBookDTO>()
                .ForMember(dst => dst.ExchangeName, opt => opt.MapFrom(src => src.Exchange.DisplayName))
                .ForMember(dst => dst.Market, opt => opt.MapFrom(src => src.Market.Ticker))
                .ForMember(dst => dst.ServerTimeStamp, opt => opt.MapFrom(src => Timestamp.FromDateTimeOffset(src.ServerTimeStamp)));
            CreateMap<OrderBookDTO, OrderBook>()
                .ForMember(dst => dst.Exchange, opt => opt.MapFrom(src => Enumeration.FromDisplayName<ExchangeName>(src.ExchangeName)))
                .ForMember(dst => dst.Market, opt => opt.MapFrom(src => new Market(src.Market)))
                .ForMember(dst => dst.ServerTimeStamp, opt => opt.MapFrom(src => src.ServerTimeStamp.ToDateTimeOffset()));
            CreateMap<OrderBookLevelDTO, OrderBookLevel>()
                .ForMember(dst => dst.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dst => dst.Price, opt => opt.MapFrom(src => (decimal)src.Price))
                .ForMember(dst => dst.Amount, opt => opt.MapFrom(src => (decimal)src.Amount));
            CreateMap<OrderBookLevel, OrderBookLevelDTO>()
                .ForMember(dst => dst.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dst => dst.Price, opt => opt.MapFrom(src => (DecimalValue)src.Price))
                .ForMember(dst => dst.Amount, opt => opt.MapFrom(src => (DecimalValue)src.Amount));
            CreateMap<EventSourceDTO, EventSource<OrderBook>>().ConvertUsing(new EventSourceRightConverter());
            CreateMap<EventSource<OrderBook>, EventSourceDTO>().ConvertUsing(new EventSourceLeftConverter());
        }

        public class EventSourceRightConverter : ITypeConverter<EventSourceDTO, EventSource<OrderBook>>
        {
            public EventSource<OrderBook> Convert(EventSourceDTO source, EventSource<OrderBook> destination, ResolutionContext context)
            {
                return new EventSource<OrderBook>()
                {
                    Content = context.Mapper.Map<OrderBook>(source.Content.Unpack<OrderBookDTO>()),
                    OcurredAt = source.OcurredAt.ToDateTimeOffset(),
                    RecordedAt = source.RecordedAt.ToDateTimeOffset()
                };
            }
        }

        public class EventSourceLeftConverter : ITypeConverter<EventSource<OrderBook>, EventSourceDTO>
        {
            public EventSourceDTO Convert(EventSource<OrderBook> source, EventSourceDTO destination, ResolutionContext context)
            {
                return new EventSourceDTO()
                {
                    Content = Any.Pack(context.Mapper.Map<OrderBookDTO>(source.Content)),
                    OcurredAt = source.OcurredAt.ToTimestamp(),
                    RecordedAt = source.RecordedAt.ToTimestamp()
                };
            }
        }
    }
}
