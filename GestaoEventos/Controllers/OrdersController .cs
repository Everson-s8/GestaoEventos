using GestaoEventos.Data;
using GestaoEventos.DTOs;
using GestaoEventos.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace GestaoEventos.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public OrdersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // POST: api/Orders
        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderDto dto)
        {
            if (dto.Items == null || !dto.Items.Any())
            {
                return BadRequest("O pedido deve conter pelo menos um item.");
            }

            var order = new Order
            {
                BuyerId = dto.BuyerId,
                OrderDate = DateTime.UtcNow,
                Items = new List<OrderItem>()
            };

            decimal totalAmount = 0;

            foreach (var itemDto in dto.Items)
            {
                // Recupera o produto
                var product = await _context.EventProducts.FindAsync(itemDto.ProductId);
                if (product == null)
                {
                    return BadRequest($"Produto com ID {itemDto.ProductId} não foi encontrado.");
                }

                if (product.Quantity < itemDto.Quantity)
                {
                    return BadRequest($"Estoque insuficiente para o produto {product.Name}. Disponível: {product.Quantity}.");
                }

                // Atualiza o estoque do produto
                product.Quantity -= itemDto.Quantity;

                // Cria o item do pedido
                var orderItem = new OrderItem
                {
                    ProductId = itemDto.ProductId,
                    Quantity = itemDto.Quantity,
                    UnitPrice = product.Price
                };

                totalAmount += product.Price * itemDto.Quantity;
                order.Items.Add(orderItem);
            }

            order.TotalAmount = totalAmount;

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            // Mapeamento para OrderDto
            var orderDto = new OrderDto
            {
                Id = order.Id,
                BuyerId = order.BuyerId,
                OrderDate = order.OrderDate,
                TotalAmount = order.TotalAmount,
                Items = order.Items.Select(oi => new OrderItemDto
                {
                    ProductId = oi.ProductId,
                    Quantity = oi.Quantity,
                    UnitPrice = oi.UnitPrice
                }).ToList()
            };

            return Ok(orderDto);
        }


    }
}
