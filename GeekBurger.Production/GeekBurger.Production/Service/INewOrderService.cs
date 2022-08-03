using System.Collections.Generic;
using GeekBurger.Production.Model;

namespace GeekBurger.Production.Service
{
    public interface INewOrderService
    {
        void SubscribeToTopic(string topicName, List<ProductionArea> productionAreas);
    }
}
