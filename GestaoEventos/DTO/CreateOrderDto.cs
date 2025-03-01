using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using GestaoEventos.DTO;

namespace GestaoEventos.DTOs
{
    public class CreateOrderDto
    {
        // Se for compra online, o BuyerId será fornecido (e o nome é obtido do usuário);
        // se for manual (pelo funcionário), BuyerId pode ser 0 e o nome deverá ser informado.
        public int BuyerId { get; set; }

        // Campo opcional para o nome do comprador (obrigatório se BuyerId for 0)
        public string BuyerName { get; set; }

        [Required]
        public List<CreateOrderItemDto> Items { get; set; }
    }
}
