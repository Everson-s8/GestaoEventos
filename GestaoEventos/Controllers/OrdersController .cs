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

        // GET: api/Orders
        [HttpGet]
        public async Task<IActionResult> GetOrders()
        {
            var orders = await _context.Orders
                .Include(o => o.Items) // Inclui os itens do pedido
                .OrderBy(o => o.OrderDate) // Opcional: ordena por data
                .ToListAsync();

            // Mapeia cada Order para OrderDto
            var orderDtos = orders.Select(o => new OrderDto
            {
                Id = o.Id,
                BuyerId = o.BuyerId,
                BuyerName = o.BuyerName,
                OrderDate = o.OrderDate,
                TotalAmount = o.TotalAmount,
                Status = o.Status.ToString(),
                Items = o.Items.Select(i => new OrderItemDto
                {
                    Id = i.Id,
                    ProductId = i.ProductId,
                    ProductName = i.ProductName,
                    Quantity = i.Quantity,
                    UnitPrice = i.UnitPrice
                }).ToList()
            }).ToList();

            return Ok(orderDtos);
        }

        // POST: api/Orders
        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderDto dto)
        {
            if (dto.Items == null || !dto.Items.Any())
            {
                return BadRequest("O pedido deve conter pelo menos um item.");
            }

            string buyerName = string.Empty;
            if (dto.BuyerId != 0)
            {
                // Compra online: busca o usuário e obtém o nome
                var user = await _context.Users.FindAsync(dto.BuyerId);
                if (user == null)
                    return BadRequest("Usuário não encontrado.");
                buyerName = user.Name;
            }
            else if (!string.IsNullOrWhiteSpace(dto.BuyerName))
            {
                // Compra manual: usa o nome informado pelo funcionário
                buyerName = dto.BuyerName;
            }
            else
            {
                return BadRequest("É necessário informar o BuyerId ou o BuyerName.");
            }

            var order = new Order
            {
                BuyerId = dto.BuyerId != 0 ? dto.BuyerId : null,
                BuyerName = buyerName,
                OrderDate = DateTime.UtcNow,
                Status = OrderStatus.Pending,
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

                // Cria o item do pedido e salva o nome do produto
                var orderItem = new OrderItem
                {
                    ProductId = product.Id,
                    ProductName = product.Name,
                    Quantity = itemDto.Quantity,
                    UnitPrice = product.Price
                };

                totalAmount += product.Price * itemDto.Quantity;
                order.Items.Add(orderItem);
            }

            order.TotalAmount = totalAmount;
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            return Ok(order);
        }

        [HttpPut("{id}/status")]
        public async Task<IActionResult> UpdateOrderStatus(int id, [FromBody] UpdateOrderStatusDto dto)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order == null)
                return NotFound("Pedido não encontrado.");

            order.Status = dto.Status;
            await _context.SaveChangesAsync();
            return Ok(order);
        }



    }

    // Interface opcional para que o CreateOrderDto possa incluir o BuyerName (quando necessário)
    public interface IHaveBuyerName
    {
        string BuyerName { get; set; }
    }
}
