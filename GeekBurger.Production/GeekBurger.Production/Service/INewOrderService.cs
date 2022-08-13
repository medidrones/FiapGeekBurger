using System.Collections.Generic;
using GeekBurger.Production.Model;

namespace GeekBurger.Production.Service
{
    /// <summary>
    /// Interface que será utilizada para implementar métodos relacionados ao recebimento de novos pedidos.
    /// </summary>
    public interface INewOrderService
    {
        void SubscribeToTopic(string topicName, List<ProductionArea> productionAreas);
    }
}
