namespace GestaoEventos.DTOs
{
    public class OrderItemDto
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }  // Nome do produto (opcional)
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }
}