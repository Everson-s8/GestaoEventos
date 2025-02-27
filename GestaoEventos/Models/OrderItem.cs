using System.ComponentModel.DataAnnotations;

namespace GestaoEventos.Models
{
    public class OrderItem
    {
        public int Id { get; set; }

        [Required]
        public int OrderId { get; set; }

        [Required]
        public int ProductId { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        public decimal UnitPrice { get; set; }

        // Propriedade de navegação para o pedido
        public Order Order { get; set; }

        // Propriedade de navegação para o produto
        public EventProduct Product { get; set; }
    }
}
