// DTOs/EventReportDto.cs
using System.Collections.Generic;

namespace GestaoEventos.DTOs
{
    public class EventReportDto
    {
        public int TotalProductsSold { get; set; }
        public decimal TotalRevenue { get; set; }
        public List<ReportItemDto> SoldOutProducts { get; set; }
        public List<ReportItemDto> RemainingProducts { get; set; }
        public ReportItemDto ProductWithHighestStock { get; set; }
        public ReportItemDto ProductMostSold { get; set; }
        public List<ReportItemDto> ReportItems { get; set; }
    }
}
