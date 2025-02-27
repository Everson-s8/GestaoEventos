using System;
using System.ComponentModel.DataAnnotations;
using GestaoEventos.Models;

namespace GestaoEventos.Models
{
    public class Ticket
    {
        public int Id { get; set; }

        [Required]
        public int EventId { get; set; }
        public Event Event { get; set; }

        [Required]
        public int BuyerId { get; set; }
        public User Buyer { get; set; }

        public DateTime PurchaseDate { get; set; }
    }
}
