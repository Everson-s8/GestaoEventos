namespace GestaoEventos.DTOs
{
    public class OrderDto
    {
        public int Id { get; set; }
        public int? BuyerId { get; set; }
        public string BuyerName { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public string Status { get; set; }
        public List<OrderItemDto> Items { get; set; }
    }
}