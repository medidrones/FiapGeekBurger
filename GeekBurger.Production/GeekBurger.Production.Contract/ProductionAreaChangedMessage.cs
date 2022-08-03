namespace GeekBurger.Production.Contract
{
    public class ProductionAreaChangedMessage
    {
        public ProductionAreaState State { get; set; }
        public ProductionAreaDTO ProductionArea { get; set; }

        public enum ProductionAreaState
        {
            Deleted = 2,
            Modified = 3,
            Added = 4
        }
    }
}
