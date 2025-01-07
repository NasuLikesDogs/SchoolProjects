namespace OO_Harjoitus_1.Details
{
    public class ProductEntity
    {
        public int TimeUsed { get; set; }
        public double TimeCost { get; set; }
        public string ProductName { get; set; }
        public double ProductQuantity { get; set; }
        public string ProductUnit { get; set; }
        public double ProductUnitPrice { get; set; }

        // Oletuskonstruktori
        public ProductEntity() { }

        public ProductEntity(int timeUsed, double timeCost, string productName, double quantity, string unit, double unitPrice)
        {
            TimeUsed = timeUsed;
            TimeCost = timeCost;
            ProductName = productName;
            ProductQuantity = quantity;
            ProductUnit = unit;
            ProductUnitPrice = unitPrice;
        }
    }
}
