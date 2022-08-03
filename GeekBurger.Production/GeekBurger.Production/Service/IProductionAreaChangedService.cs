using System.Collections.Generic;
using GeekBurger.Production.Model;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace GeekBurger.Production.Service
{
    public interface IProductionAreaChangedService
    {
        void SendMessagesAsync();
        void AddToMessageList(IEnumerable<EntityEntry<ProductionArea>> changes);
    }
}
