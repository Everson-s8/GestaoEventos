using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

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

        public string ProductName { get; set; }

        // Quebra o ciclo – essa propriedade não será serializada para JSON.
        [JsonIgnore]
        public Order Order { get; set; }

        // Se precisar dos dados do produto, pode manter essa propriedade.
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public EventProduct Product { get; set; }
    }
}
