namespace Zzaia.Finance.Web.Grpc.Services
{
    public interface IConnectorControl
    {
        void Activate();
        void Deactivate();
    }
}