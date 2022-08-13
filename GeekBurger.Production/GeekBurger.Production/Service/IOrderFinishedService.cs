using GeekBurger.Production.Contract;

namespace GeekBurger.Production.Service
{
    /// <summary>
    /// Interface que será utilizada para implementar métodos relacionados com a finalização da produção da ordem
    /// </summary>
    public interface IOrderFinishedService
    {
        void SendMessagesAsync();
        void AddToMessageList(OrderFinishedMessage orderFinished);
    }
}
