public class CreateEventDto
{
    public string Name { get; set; }
    public string Description { get; set; }

    // Data de início do evento
    public DateTime Date { get; set; }

    // Nova propriedade: Data Final
    public DateTime EndDate { get; set; }

    public string Location { get; set; }
    public int TotalTickets { get; set; }
    public string ContactPhone { get; set; }
    public string ContactEmail { get; set; }
    public int CreatedBy { get; set; } // ID do gestor que cria o evento
}
