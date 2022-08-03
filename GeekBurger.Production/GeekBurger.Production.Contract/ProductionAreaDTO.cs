using System;
using System.Collections.Generic;

namespace GeekBurger.Production.Contract
{
    public class ProductionAreaDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public bool Status { get; set; }
        public List<string> Restrictions { get; set; }
    }
}
