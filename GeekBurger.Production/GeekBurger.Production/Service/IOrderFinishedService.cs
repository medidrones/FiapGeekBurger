using GeekBurger.Production.Contract;

namespace GeekBurger.Production.Service
{
    public interface IOrderFinishedService
    {
        void SendMessagesAsync();
        void AddToMessageList(OrderFinishedMessage orderFinished);
    }
}
