﻿using System;
using System.ComponentModel.DataAnnotations;

namespace GestaoEventos.Models
{
    public class EventProduct
    {
        public int Id { get; set; }

        [Required]
        public int EventId { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public decimal Price { get; set; }

        [Required]
        public int Quantity { get; set; }

        // Armazena a URL da imagem (após upload)
        [Required]
        public string ImageUrl { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
