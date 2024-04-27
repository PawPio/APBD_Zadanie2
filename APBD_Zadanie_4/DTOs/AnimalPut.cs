using System.ComponentModel.DataAnnotations;

namespace APBD_Zadanie_4.DTOs
{
    public class AnimalPut
    {
        [Required]
        [StringLength(185, MinimumLength = 5)]
        public string Name { get; set; }
        [StringLength(185, MinimumLength = 5)]
        public string? Description { get; set; } 
        [Required]
        [StringLength(185, MinimumLength = 5)]
        public string Category { get; set; } 
        [Required]
        [StringLength(185, MinimumLength = 5)]
        public string Area { get; set; } 
    }
}