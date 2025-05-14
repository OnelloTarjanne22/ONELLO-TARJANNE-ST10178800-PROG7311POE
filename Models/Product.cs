namespace PROG7311POE_ST10178800.Models
{
    public class Product
    {
        public int Id { get; set; } 
        public string Name { get; set; }
        public string Category { get; set; }
        public DateTime DateAdded { get; set; }
        public string? FarmerId { get; set; } 
    }
}
