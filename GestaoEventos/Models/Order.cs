using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GestaoEventos.Models
{
    public class Order
    {
        public int Id { get; set; }

        [Required]
        public int BuyerId { get; set; }

        // Propriedade de navegação para o usuário (comprador)
        public User Buyer { get; set; }

        public DateTime OrderDate { get; set; }

        public decimal TotalAmount { get; set; }

        public List<OrderItem> Items { get; set; }
    }
}
