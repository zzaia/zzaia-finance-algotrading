namespace MarketIntelligency.DataEventManager.ConnectorAggregate
{
    public interface IConnectorControl
    {
        void Activate();
        void Deactivate();
    }
}