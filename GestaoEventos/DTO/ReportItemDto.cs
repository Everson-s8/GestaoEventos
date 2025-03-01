// DTOs/ReportItemDto.cs
namespace GestaoEventos.DTOs
{
    public class ReportItemDto
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int TotalSold { get; set; }
        public decimal Revenue { get; set; }
        public int CurrentStock { get; set; }
    }
}
