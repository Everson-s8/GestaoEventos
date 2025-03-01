using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GestaoEventos.Models
{
    public enum OrderStatus
    {
        Pending,    // Pendente
        Delivered   // Entregue
    }

    public class Order
    {
        public int Id { get; set; }

        // BuyerId agora é opcional (se a compra for manual, pode ser nulo)
        public int? BuyerId { get; set; }

        // Nome do comprador, sempre será salvo, seja obtido via usuário ou informado manualmente
        [Required]
        public string BuyerName { get; set; }

        public DateTime OrderDate { get; set; }

        public decimal TotalAmount { get; set; }

        // Novo campo de status
        public OrderStatus Status { get; set; } = OrderStatus.Pending;

        public List<OrderItem> Items { get; set; }

        public User Buyer { get; set; }
    }
}
