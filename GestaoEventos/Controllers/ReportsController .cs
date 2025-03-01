using GestaoEventos.Data;
using GestaoEventos.DTOs;
using GestaoEventos.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace GestaoEventos.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReportsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public ReportsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Reports/{eventId}
        [HttpGet("{eventId}")]
        public async Task<IActionResult> GetEventReport(int eventId)
        {
            // Recupera os produtos cadastrados para o evento
            var products = await _context.EventProducts
                .Where(p => p.EventId == eventId)
                .ToListAsync();

            if (!products.Any())
            {
                return NotFound("Nenhum produto encontrado para este evento.");
            }

            // Para cada produto, agrega os dados dos itens de pedido (OrderItems) relacionados
            var reportData = from product in _context.EventProducts
                             where product.EventId == eventId
                             join orderItem in _context.OrderItems
                                 on product.Id equals orderItem.ProductId into orderGroup
                             select new ReportItemDto
                             {
                                 ProductId = product.Id,
                                 ProductName = product.Name,
                                 TotalSold = orderGroup.Sum(oi => (int?)oi.Quantity) ?? 0,
                                 Revenue = orderGroup.Sum(oi => (decimal?)(oi.Quantity * oi.UnitPrice)) ?? 0,
                                 CurrentStock = product.Quantity
                             };

            var reportItems = await reportData.ToListAsync();


            // Dados gerais
            var totalProductsSold = reportItems.Sum(r => r.TotalSold);
            var totalRevenue = reportItems.Sum(r => r.Revenue);
            var soldOutProducts = reportItems.Where(r => r.CurrentStock == 0).ToList();
            var remainingProducts = reportItems.Where(r => r.CurrentStock > 0).ToList();
            var productWithHighestStock = reportItems.OrderByDescending(r => r.CurrentStock).FirstOrDefault();
            var productMostSold = reportItems.OrderByDescending(r => r.TotalSold).FirstOrDefault();

            var report = new EventReportDto
            {
                TotalProductsSold = totalProductsSold,
                TotalRevenue = totalRevenue,
                SoldOutProducts = soldOutProducts,
                RemainingProducts = remainingProducts,
                ProductWithHighestStock = productWithHighestStock,
                ProductMostSold = productMostSold,
                ReportItems = reportItems
            };

            return Ok(report);
        }
    }
}
