using System;
using System.Collections.Generic;
using GeekBurger.Production.Model;
using GeekBurger.Production.Repository;

namespace GeekBurger.Production.Extension
{
    public static class ProductionContextExtension
    {
        public static void Seed(this ProductionContext context)
        {
            context.ProductionAreas.RemoveRange(context.ProductionAreas);
            context.SaveChanges();
            context.ProductionAreas.AddRange(new List<ProductionArea>() {
                    new ProductionArea {   
                        Id = new Guid("9524c16b-7642-42f1-bd0b-9fcc9c7335c0"), 
                        Name = "Grill 1", 
                        Status = true, 
                        Restrictions = null
                    }, 
                    new ProductionArea {
                        Id = new Guid("1c8a9122-7d42-4884-90fd-cc90d830f723"), 
                        Name = "Grill 2 - No Gluten&Wheat", 
                        Status = true, 
                        Restrictions = new List<Restriction> 
                        {
                            new Restriction{Name = "gluten" }, new Restriction{Name = "wheat" }
                        }
                    }, 
                    new ProductionArea {   
                        Id = new Guid("7d0ab8cb-bda4-4008-a000-e6654ff3860e"), 
                        Name = "Grill 3 - No Soy", 
                        Status = true, 
                        Restrictions = new List<Restriction>
                        {
                            new Restriction{Name = "soy" }
                        }
                    }, 
                    new ProductionArea {
                        Id = new Guid("7c48711e-2eea-4707-bf2a-db1b39efe88a"), 
                        Name = "Grill 4 - No Milk",
                        Status = true, 
                        Restrictions = new List<Restriction>
                        {
                            new Restriction{Name = "milk" }
                        }
                    }, 
                    new ProductionArea {   
                        Id = new Guid("81b5b371-1fe5-4be6-9eb6-e2a81aa62174"), 
                        Name = "Grill 4 - No Soy, Milk & Gluten", 
                        Status = true, 
                        Restrictions = new List<Restriction> 
                        {
                            new Restriction{Name = "milk" }, 
                            new Restriction{Name = "soy" }, 
                            new Restriction{Name = "gluten" }, new Restriction{Name = "wheat" }
                        }
                    }, 
                    new ProductionArea { 
                        Id = new Guid("58431438-145b-43f5-8136-095cb1622d1c"), 
                        Name = "Grill 5 - No Peanuts", 
                        Status = true, 
                        Restrictions = new List<Restriction>
                        {
                            new Restriction{Name = "peanuts" }
                        }
                    }, 
                    new ProductionArea {
                        Id = new Guid("bb38b77a-6706-46b0-973c-8084bbb42ece"), 
                        Name = "Grill 5 - No Sugar", 
                        Status = true, 
                        Restrictions = new List<Restriction>
                        {
                            new Restriction{Name = "sugar" }
                        }
                    }
                });

            context.SaveChanges();
        }
    }
}
