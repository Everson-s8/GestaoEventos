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

            // Cria o pedido
            var order = new Order
            {
                BuyerId = dto.BuyerId,  // Esse valor virá do token (online) ou pode ser nulo
                OrderDate = DateTime.UtcNow,
                Items = new System.Collections.Generic.List<OrderItem>()
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

                // Atualiza o estoque
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

            // Aqui, definimos o BuyerName:
            // Se o pedido for online, o BuyerId já está preenchido e podemos buscar o nome do usuário.
            // Se for manual (compra feita pelo funcionário), o front-end deve enviar o nome do cliente através de um campo adicional.
            // Por exemplo, se o dto tiver um campo opcional BuyerName (que você pode adicionar ao CreateOrderDto).
            if (dto is IHaveBuyerName dtoWithName && !string.IsNullOrEmpty(dtoWithName.BuyerName))
            {
                order.BuyerName = dtoWithName.BuyerName;
            }
            else if (order.BuyerId.HasValue)
            {
                var user = await _context.Users.FindAsync(order.BuyerId.Value);
                order.BuyerName = user?.Name;
            }

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            // Mapeia o pedido para o DTO de saída
            var orderDto = new OrderDto
            {
                Id = order.Id,
                BuyerId = order.BuyerId,
                BuyerName = order.BuyerName,
                OrderDate = order.OrderDate,
                TotalAmount = order.TotalAmount,
                Items = order.Items.Select(i => new OrderItemDto
                {
                    Id = i.Id,
                    ProductId = i.ProductId,
                    ProductName = _context.EventProducts.FirstOrDefault(p => p.Id == i.ProductId)?.Name,
                    Quantity = i.Quantity,
                    UnitPrice = i.UnitPrice
                }).ToList()
            };

            return Ok(orderDto);
        }
    }

    // Interface opcional para que o CreateOrderDto possa incluir o BuyerName (quando necessário)
    public interface IHaveBuyerName
    {
        string BuyerName { get; set; }
    }
}
