using System.Collections.Generic;

namespace GeekBurger.Production.Contract
{
    public class ProductionAreaCRUD
    {
        public string Name { get; set; }
        public bool Status { get; set; }
        public List<string> Restrictions { get; set; }
    }
}
