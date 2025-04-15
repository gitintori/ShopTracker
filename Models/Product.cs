using SQLite;

namespace ShopTracker.Models
{
    public class Product
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Description { get; set; }
        public double Quantity { get; set; }
        public double Price { get; set; }
        public double Total { get => Quantity * Price; }
        public string Category { get; set; }
    }

}