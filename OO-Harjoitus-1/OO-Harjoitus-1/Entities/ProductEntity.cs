namespace OO_Harjoitus_1.Details
{
    public class ProductEntity
    {
        public string ProductName { get; set; }
        public double Quantity { get; set; }
        public string Unit { get; set; }
        public double UnitPrice { get; set; }

        public ProductEntity(string productName, double quantity, string unit, double unitPrice)
        {
            ProductName = productName;
            Quantity = quantity;
            Unit = unit;
            UnitPrice = unitPrice;
        }
    }
}
