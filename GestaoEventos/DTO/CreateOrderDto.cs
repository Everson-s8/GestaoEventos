using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using GestaoEventos.DTO;

namespace GestaoEventos.DTOs
{
    public class CreateOrderDto
    {
        // Se a compra for feita virtualmente, você pode obter o BuyerId do token do usuário.
        // Aqui está de forma explícita para facilitar o exemplo:
        [Required]
        public int BuyerId { get; set; }

        [Required]
        public List<CreateOrderItemDto> Items { get; set; }
    }

}
