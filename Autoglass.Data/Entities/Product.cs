using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Autoglass.Data.Entities
{
    public class Product
    {
        [Required]
        public int Id { get; set; }

        [MaxLength(500)]
        public string Description { get; set; }

        [Required]
        [DefaultValue(true)]
        public bool Active { get; set; }

        public DateTime ManufacturingDate { get; set; }

        public DateTime DueDate { get; set; }

        public Supplier Supplier { get; set; }
    }
}
