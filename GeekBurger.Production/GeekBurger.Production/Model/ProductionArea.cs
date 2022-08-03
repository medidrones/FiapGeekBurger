using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GeekBurger.Production.Model
{
    public class ProductionArea
    {
        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public bool Status { get; set; }
        public List<Restriction> Restrictions { get; set; }
    }
}
