namespace ApplicationCore.Entities
{
    public class Product
    {
        public int Id { get; set; } 
        public string Title { get; set; }
        public string Description { get; set; }
        public string Seller { get; set; }
        public int Price { get; set; }
        public int Quantity { get; set; }
        public string ImageUrl { get; set; }
    }
}
