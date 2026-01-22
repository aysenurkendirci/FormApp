using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;
namespace FormsApp.Models
{
  
    public class Product
    {
        [Display(Name="Urun Id")]
        public int ProductId { get; set; }
        [Required]
        [StringLength(1000)]
        [Display(Name = "Urun Adı")]
        public string Name { get; set; } = null!;

        [Display(Name = "Fiyatı")]
       
        [Range(0,100000)]
        public decimal? Price{ get; set; }
        [Display(Name = "Resim")]
        public string Image { get; set; } = string.Empty;
        public bool IsActive{ get; set; }
        [Display(Name = "Category")]
        [Required]
        public int CategoryId { get; set; }

        [NotMapped]
        public IFormFile? imageFile { get; set; }

    }
}