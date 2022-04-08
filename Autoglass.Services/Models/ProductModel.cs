using System;
using System.ComponentModel.DataAnnotations;

namespace Autoglass.Services.Models
{
    public class ProductModel
    {
        public int Id { get; set; }
        
        [Required]
        public string Description { get; set; }

        public DateTime ManufacturingDate { get; set; }

        public DateTime DueDate { get; set; }

        public int SupplierId { get; set; }

        public string SupplierDescription { get; set; }

        public string SupplierPhoneNumber { get; set; }
    }
}
