namespace AdvancedMappingSamples.Entities
{
    public class Order
    {
        public int Id { get; set; }
        public OrderStatus? Status { get; set; }
        public DetailedOrder DetailedOrder { get; set; }
    }

    public enum OrderStatus
    {
        New,
        Delivered,
    }
}
