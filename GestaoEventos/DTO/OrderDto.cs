using System;
using System.Collections.Generic;

namespace GestaoEventos.DTOs
{
    public class OrderDto
    {
        public int Id { get; set; }
        public int? BuyerId { get; set; }
        public string BuyerName { get; set; }  // Nome do comprador (ou informado pelo funcionário)
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public List<OrderItemDto> Items { get; set; }
    }
}
