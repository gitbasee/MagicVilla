
using System.ComponentModel.DataAnnotations;

namespace MagicVilla_VillaAPI.Models.Dto
{
    public class VillaDTO
    {
        [Required]
        public int ID { get; set; }
        [Required]
        [MaxLength(30,ErrorMessage ="Tooooo muchhhh lenght!!!!!!")]
        public string Name { get; set; }
        public string Details { get; set; }
        public int Sqft { get; set; }
        public int Occupancy { get; set; }
        public double Rate { get; set; }
        public string Amenity { get; set; }
        public string ImageUrl { get; set; }
    }
}
