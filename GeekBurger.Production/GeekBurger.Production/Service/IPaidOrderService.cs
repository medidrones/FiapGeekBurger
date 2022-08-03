using System.Collections.Generic;
using GeekBurger.Production.Model;

namespace GeekBurger.Production.Service
{
    public interface IPaidOrderService
    {
        void SubscribeToTopic(string topicName, List<ProductionArea> productionAreas);
    }
}
