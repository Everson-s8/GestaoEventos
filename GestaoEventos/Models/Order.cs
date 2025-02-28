using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GestaoEventos.Models
{
    public class Order
    {
        public int Id { get; set; }

        public int? BuyerId { get; set; }

        public string BuyerName { get; set; }

        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public List<OrderItem> Items { get; set; }

        // Propriedade de navegação – opcional, pois pode causar ciclo, por isso não a usaremos no DTO
        public User Buyer { get; set; }
    }
}
