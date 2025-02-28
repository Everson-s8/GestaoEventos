using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using GestaoEventos.Controllers;
using GestaoEventos.DTO;

namespace GestaoEventos.DTOs
{
    public class CreateOrderDto : IHaveBuyerName
    {
        [Required]
        public int? BuyerId { get; set; }  // Pode ser nulo para compras manuais

        // Campo opcional para o nome do comprador (quando a compra é feita por um funcionário)
        public string BuyerName { get; set; }

        [Required]
        public List<CreateOrderItemDto> Items { get; set; }
    }
}
