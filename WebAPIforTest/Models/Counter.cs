using System.ComponentModel.DataAnnotations;


namespace WebAPIforTest.Models
{
    public class Counter
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int Key { get; set; }
        [Required]
        public int Value { get; set; } 
    }
}
