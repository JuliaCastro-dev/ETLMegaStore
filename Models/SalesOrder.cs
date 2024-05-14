namespace ETLMegaStore.Models
{
    public class SalesOrder
    {
        public required int Id { get; set; }
        public required string OrderId { get; set; }
        public required DateTime OrderDate { get; set; }
        public required DateTime ShipDate { get; set; }
        public required string ShipMode { get; set; }
        public required string CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string Segment { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public required string PostalCode { get; set; }
        public string Region { get; set; }
        public required int ProductId { get; set; }
        public required string Category { get; set; }
        public required string ProductName { get; set; }
        public decimal Sales { get; set; }
        public required int Quantity { get; set; }
        public decimal Discount { get; set; }
        public decimal Profit { get; set; }
    }
}
